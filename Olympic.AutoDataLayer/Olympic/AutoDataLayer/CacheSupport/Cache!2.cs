namespace Olympic.AutoDataLayer.CacheSupport
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public abstract class Cache<TKey, TObject> : Cache
    {
        private Hashtable _cacheItems;
        private int _cacheSize;
        private LinkedList<TObject> _linkedList;
        private bool _prepared;
        private CacheDiagnostics Diagnostics;

        protected Cache()
        {
            this._linkedList = new LinkedList<TObject>();
            this._cacheItems = new Hashtable();
            this._cacheSize = 0x7fffffff;
            Cache.ClearAll += new EventHandler(this.Cache_ClearAll);
            this.Diagnostics = new CacheDiagnostics(this, 0x7fffffff);
        }

        protected Cache(int maximumSize) : this()
        {
            this._cacheSize = maximumSize;
            this.Diagnostics.MaxCacheSize = maximumSize;
        }

        protected Cache(params CacheValidator[] validators) : this(0x7fffffff, validators)
        {
        }

        protected Cache(int maximumSize, params CacheValidator[] validators) : this()
        {
            this._cacheSize = maximumSize;
            this.Diagnostics.MaxCacheSize = maximumSize;
            foreach (CacheValidator validator in validators)
            {
                this.AddValidator(validator);
            }
        }

        protected virtual LinkedListNode<TObject> Add(TObject item)
        {
            lock (((Cache<TKey, TObject>) this))
            {
                LinkedListNode<TObject> node = new LinkedListNode<TObject>(item);
                this._cacheItems[this.GetKeyForItem(item)] = node;
                this._linkedList.AddLast(node);
                if (this._linkedList.Count > this._cacheSize)
                {
                    LinkedListNode<TObject> first = this._linkedList.First;
                    this._linkedList.RemoveFirst();
                    TKey keyForItem = this.GetKeyForItem(first.Value);
                    this._cacheItems.Remove(keyForItem);
                    base.OnCacheItemRemoved(first.Value);
                }
                return node;
            }
        }

        public void AddValidator(CacheValidator validator)
        {
            lock (((Cache<TKey, TObject>) this))
            {
                base._validators.Add(validator);
                validator.SetCache(this);
                validator.Invalid += new EventHandler(this.Validator_Invalid);
            }
        }

        private void Cache_ClearAll(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected void Clear()
        {
            lock (((Cache<TKey, TObject>) this))
            {
                TObject[] localArray = this.ToArray();
                this._linkedList.Clear();
                this._cacheItems.Clear();
                this.Diagnostics.LastCleared = new DateTime?(DateTime.Now);
                foreach (TObject local in localArray)
                {
                    base.OnCacheItemRemoved(local);
                }
                this._prepared = false;
                base.OnCleared();
            }
        }

        protected virtual TObject Get(TKey key)
        {
            lock (((Cache<TKey, TObject>) this))
            {
                base.Validate();
                if (!this._prepared)
                {
                    this.OnPrepare();
                }
                LinkedListNode<TObject> node = (LinkedListNode<TObject>) this._cacheItems[key];
                if (node == null)
                {
                    TObject item = this.GetItem(key);
                    if (item == null)
                    {
                        return default(TObject);
                    }
                    node = this.Add(item);
                }
                else if (this._cacheSize < 0x7fffffff)
                {
                    this._linkedList.Remove(node);
                    this._linkedList.AddLast(node);
                }
                return node.Value;
            }
        }

        protected override Array GetCacheItems()
        {
            lock (((Cache<TKey, TObject>) this))
            {
                return this.ToArray();
            }
        }

        protected abstract TObject GetItem(TKey key);
        protected abstract TKey GetKeyForItem(TObject item);
        public void Invalidate()
        {
            this.OnInvalidated(null);
        }

        public void InvalidateItem(TKey key)
        {
            LinkedListNode<TObject> node = (LinkedListNode<TObject>) this._cacheItems[key];
            if (node != null)
            {
                this._linkedList.Remove(node);
                this._cacheItems.Remove(key);
                base.OnCacheItemRemoved(node.Value);
            }
        }

        public void InvalidateItem(TObject item)
        {
            this.InvalidateItem(this.GetKeyForItem(item));
        }

        protected virtual void OnInvalidated(CacheValidator validator)
        {
            this.Clear();
        }

        protected virtual void OnPrepare()
        {
            this._prepared = true;
        }

        public TObject[] ToArray()
        {
            lock (((Cache<TKey, TObject>) this))
            {
                List<TObject> list = new List<TObject>();
                list.AddRange(this._linkedList);
                return list.ToArray();
            }
        }

        private void Validator_Invalid(object sender, EventArgs e)
        {
            this.OnInvalidated((CacheValidator) sender);
        }

        public override int CachedItemCount
        {
            get
            {
                return this._linkedList.Count;
            }
        }

        public TObject this[TKey key]
        {
            get
            {
                return this.Get(key);
            }
        }
    }
}

