namespace Olympic.AutoDataLayer.CacheSupport
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    public class CacheDiagnostics
    {
        private Olympic.AutoDataLayer.CacheSupport.Cache _cache;
        public string CacheType;
        public DateTime Initialized;
        public DateTime? LastCleared;
        public int MaxCacheSize;

        private static  event CacheDiagnosticsRequestHandler RequestDiagnostics;

        public CacheDiagnostics()
        {
            this.Initialized = DateTime.Now;
        }

        public CacheDiagnostics(Olympic.AutoDataLayer.CacheSupport.Cache cache, int maxCacheSize)
        {
            this.Initialized = DateTime.Now;
            this._cache = cache;
            this.CacheType = this._cache.GetType().FullName;
            this.MaxCacheSize = maxCacheSize;
            RequestDiagnostics = (CacheDiagnosticsRequestHandler) Delegate.Combine(RequestDiagnostics, new CacheDiagnosticsRequestHandler(this.CacheDiagnostics_RequestDiagnostics));
        }

        private void CacheDiagnostics_RequestDiagnostics(List<CacheDiagnostics> diagnostics)
        {
            diagnostics.Add(this);
        }

        public static List<CacheDiagnostics> ListAll()
        {
            List<CacheDiagnostics> diagnostics = new List<CacheDiagnostics>();
            if (RequestDiagnostics != null)
            {
                RequestDiagnostics(diagnostics);
            }
            diagnostics.Sort(new DiagnosticsSorter());
            return diagnostics;
        }

        [XmlIgnore]
        public Olympic.AutoDataLayer.CacheSupport.Cache Cache
        {
            get
            {
                return this._cache;
            }
            set
            {
                this._cache = value;
            }
        }

        public int ItemsInCache
        {
            get
            {
                return this._cache.CachedItemCount;
            }
            set
            {
            }
        }

        private class DiagnosticsSorter : Comparer<CacheDiagnostics>
        {
            public override int Compare(CacheDiagnostics x, CacheDiagnostics y)
            {
                return string.Compare(x.CacheType, y.CacheType);
            }
        }
    }
}

