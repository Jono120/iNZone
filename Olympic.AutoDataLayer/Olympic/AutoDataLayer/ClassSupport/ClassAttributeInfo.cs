namespace Olympic.AutoDataLayer.ClassSupport
{
    using Olympic.AutoDataLayer;
    using System;
    using System.Diagnostics;
    using System.Reflection;

    internal class ClassAttributeInfo
    {
        public bool AutoBuild;
        public bool AutoResolveCurrentUser;
        public bool CheckForStaleData;
        public Olympic.AutoDataLayer.ClassSupport.DataSourceType DataSourceType;
        public bool KeepVersionHistory;
        public bool PreserveData;
        public string Query;
        public string TableName;
        public string VersionHistoryTableName;
        public bool VersioningEnabled;

        public ClassAttributeInfo(Type type, DataSourceAttribute overrideDataSource)
        {
            this.TableName = type.Name;
            DataSourceAttribute attribute = overrideDataSource ?? ((DataSourceAttribute) this.GetFirstAttributeOfType(type, typeof(DataSourceAttribute)));
            if (attribute == null)
            {
                attribute = new TableAttribute();
            }
            if (attribute is TableAttribute)
            {
                this.DataSourceType = Olympic.AutoDataLayer.ClassSupport.DataSourceType.Table;
                TableAttribute attribute2 = (TableAttribute) attribute;
                if ((attribute2.TableName != null) && (attribute2.TableName.Length > 0))
                {
                    this.TableName = attribute2.TableName;
                }
                bool flag = false;
                if (attribute2.BuildMode == BuildMode.DebugOnly)
                {
                    flag = this.IsAssemblyBuildDebug(type.Assembly);
                }
                else
                {
                    flag = attribute2.BuildMode == BuildMode.AllowBuild;
                }
                this.AutoBuild = flag && type.IsSubclassOf(typeof(AutoDataSupport));
                this.PreserveData = attribute2.PreserveData;
            }
            else if (attribute is QueryAttribute)
            {
                this.DataSourceType = Olympic.AutoDataLayer.ClassSupport.DataSourceType.Query;
                QueryAttribute attribute3 = (QueryAttribute) attribute;
                this.AutoBuild = false;
                this.PreserveData = false;
                this.TableName = null;
                this.Query = attribute3.Query;
            }
            VersionAttribute firstAttributeOfType = (VersionAttribute) this.GetFirstAttributeOfType(type, typeof(VersionAttribute));
            if (firstAttributeOfType != null)
            {
                this.VersioningEnabled = true;
                this.KeepVersionHistory = firstAttributeOfType.KeepVersionHistory;
                this.VersionHistoryTableName = firstAttributeOfType.VersionHistoryTableName ?? this.GetDefaultVersionHistoryTableName(this.TableName);
                this.AutoResolveCurrentUser = firstAttributeOfType.AutoResolveCurrentUser;
                this.CheckForStaleData = firstAttributeOfType.CheckForStaleData;
            }
        }

        private string GetDefaultVersionHistoryTableName(string tableName)
        {
            return ("_history_" + tableName);
        }

        private Attribute GetFirstAttributeOfType(Type objectType, Type attributeType)
        {
            object[] customAttributes = objectType.GetCustomAttributes(attributeType, true);
            int index = 0;
            while (index < customAttributes.Length)
            {
                return (Attribute) customAttributes[index];
            }
            return null;
        }

        private bool IsAssemblyBuildDebug(Assembly assembly)
        {
            foreach (object obj2 in assembly.GetCustomAttributes(false))
            {
                if (obj2 is DebuggableAttribute)
                {
                    return ((DebuggableAttribute) obj2).IsJITTrackingEnabled;
                }
            }
            return false;
        }
    }
}

