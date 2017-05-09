namespace Olympic.AutoDataLayer.CacheSupport
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public abstract class Cache
    {
        protected List<CacheValidator> _validators = new List<CacheValidator>();

        public event EventHandler CacheItemRemoved;

        protected static  event EventHandler ClearAll;

        public event EventHandler Cleared;

        protected Cache()
        {
        }

        protected abstract Array GetCacheItems();
        public static void InvalidateAll()
        {
            if (ClearAll != null)
            {
                ClearAll(null, null);
            }
        }

        protected void OnCacheItemRemoved(object item)
        {
            if (this.CacheItemRemoved != null)
            {
                this.CacheItemRemoved(item, null);
            }
        }

        protected void OnCleared()
        {
            if (this.Cleared != null)
            {
                this.Cleared(this, null);
            }
        }

        public void Validate()
        {
            if (this._validators != null)
            {
                foreach (CacheValidator validator in this._validators)
                {
                    validator.Validate();
                }
            }
        }

        public abstract int CachedItemCount { get; }
    }
}

