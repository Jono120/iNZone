namespace Olympic.AutoDataLayer
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple=true)]
    public class IndexAttribute : Attribute
    {
        private bool _clustered;
        private string _indexName;
        private OrderType _sortOrder;
        private bool _unique;

        public IndexAttribute()
        {
        }

        public IndexAttribute(string indexName)
        {
            this._indexName = indexName;
        }

        public IndexAttribute(string indexName, OrderType sortOrder, bool clustered)
        {
            this._indexName = indexName;
            this._sortOrder = sortOrder;
            this._clustered = clustered;
        }

        public bool Clustered
        {
            get
            {
                return this._clustered;
            }
            set
            {
                this._clustered = value;
            }
        }

        public string Name
        {
            get
            {
                return this._indexName;
            }
            set
            {
                this._indexName = value;
            }
        }

        public OrderType SortOrder
        {
            get
            {
                return this._sortOrder;
            }
            set
            {
                this._sortOrder = value;
            }
        }

        public bool Unique
        {
            get
            {
                return this._unique;
            }
            set
            {
                this._unique = value;
            }
        }
    }
}

