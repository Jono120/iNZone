namespace Olympic.AutoDataLayer
{
    using System;

    public class TableAttribute : DataSourceAttribute
    {
        private Olympic.AutoDataLayer.BuildMode _buildMode;
        private bool _perserveData;
        private string _tableName;
        public static Olympic.AutoDataLayer.BuildMode DefaultAutoBuild = Olympic.AutoDataLayer.BuildMode.DebugOnly;
        public static bool DefaultPreserveData = true;

        public TableAttribute()
        {
            this._buildMode = DefaultAutoBuild;
            this._perserveData = DefaultPreserveData;
        }

        public TableAttribute(string tableName)
        {
            this._buildMode = DefaultAutoBuild;
            this._perserveData = DefaultPreserveData;
            this._tableName = tableName;
        }

        public Olympic.AutoDataLayer.BuildMode BuildMode
        {
            get
            {
                return this._buildMode;
            }
            set
            {
                this._buildMode = value;
            }
        }

        public override string DatasourceValidationKey
        {
            get
            {
                return ("Table(" + this.TableName + ")");
            }
        }

        public bool PreserveData
        {
            get
            {
                return this._perserveData;
            }
            set
            {
                this._perserveData = value;
            }
        }

        public string TableName
        {
            get
            {
                return this._tableName;
            }
        }
    }
}

