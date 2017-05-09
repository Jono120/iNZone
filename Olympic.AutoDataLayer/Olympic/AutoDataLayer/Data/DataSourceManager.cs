namespace Olympic.AutoDataLayer.Data
{
    using Olympic.AutoDataLayer;
    using System;

    internal class DataSourceManager
    {
        [ThreadStatic]
        private static DataSourceAttribute _threadScopeOverrideDataSource;

        public static DataSourceAttribute Get()
        {
            return _threadScopeOverrideDataSource;
        }

        public static DataSourceAttribute OverrideThreadDataSource
        {
            get
            {
                return _threadScopeOverrideDataSource;
            }
            set
            {
                _threadScopeOverrideDataSource = value;
            }
        }
    }
}

