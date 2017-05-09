namespace Olympic.AutoDataLayer
{
    using Olympic.AutoDataLayer.Data;
    using System;

    public class ConnectionTarget : IDisposable
    {
        private string _previousConnectionString;
        private DataSourceAttribute _previousDataSource;

        public ConnectionTarget(DataSourceAttribute dataSource)
        {
            this.OverrideConnectionString(ConnectionStringManager.OverrideThreadConnectionString);
            this.OverrideDataSource(dataSource);
        }

        public ConnectionTarget(string connectionString)
        {
            this.OverrideConnectionString(connectionString);
            this.OverrideDataSource(DataSourceManager.OverrideThreadDataSource);
        }

        public ConnectionTarget(string connectionString, DataSourceAttribute dataSource)
        {
            this.OverrideConnectionString(connectionString);
            this.OverrideDataSource(dataSource);
        }

        public void Dispose()
        {
            ConnectionStringManager.OverrideThreadConnectionString = this._previousConnectionString;
            DataSourceManager.OverrideThreadDataSource = this._previousDataSource;
        }

        private void OverrideConnectionString(string connectionString)
        {
            this._previousConnectionString = ConnectionStringManager.OverrideThreadConnectionString;
            ConnectionStringManager.OverrideThreadConnectionString = connectionString;
        }

        private void OverrideDataSource(DataSourceAttribute dataSource)
        {
            this._previousDataSource = DataSourceManager.OverrideThreadDataSource;
            DataSourceManager.OverrideThreadDataSource = dataSource;
        }
    }
}

