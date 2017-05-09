namespace Olympic.AutoDataLayer.CacheSupport
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class CacheValidator
    {
        protected Olympic.AutoDataLayer.CacheSupport.Cache Cache;

        public event EventHandler Invalid;

        protected CacheValidator()
        {
        }

        protected void OnInvalid()
        {
            if (this.Invalid != null)
            {
                this.Invalid(this, null);
            }
        }

        public void SetCache(Olympic.AutoDataLayer.CacheSupport.Cache cache)
        {
            this.Cache = cache;
        }

        public abstract void Validate();
    }
}

