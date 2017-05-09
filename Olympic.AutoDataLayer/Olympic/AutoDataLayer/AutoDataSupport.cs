namespace Olympic.AutoDataLayer
{
    using Olympic.AutoDataLayer.ClassSupport;
    using Olympic.AutoDataLayer.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public abstract class AutoDataSupport
    {
        private static int _commandTimeout = 30;
        protected DictionaryEntry[] _loadedKeys;
        [IncludeDB]
        protected string _modifiedBy;
        [IncludeDB]
        protected DateTime _modifiedDate;
        [Index, IncludeDB]
        protected int _version;

        public static  event AfterCreateTableEventHandler AfterCreateTable;

        public event EventHandler AfterGet;

        public event EventHandler AfterSave;

        public static  event BeforeCreateTableEventHandler BeforeCreateTable;

        public event CancelEventHandler BeforeSave;

        public static  event EventHandler<PreExecuteCommandEventArgs> PreExecuteCommand;

        public static  event EventHandler<ResolveCurrentUserEventArgs> ResolveCurrentUser;

        static AutoDataSupport()
        {
            ClassDefinition.BeforeCreateTable += new BeforeCreateTableEventHandler(AutoDataSupport.ClassDefinition_BeforeCreateTable);
            ClassDefinition.AfterCreateTable += new AfterCreateTableEventHandler(AutoDataSupport.ClassDefinition_AfterCreateTable);
        }

        private static void ClassDefinition_AfterCreateTable(Type type)
        {
            if (AfterCreateTable != null)
            {
                AfterCreateTable(type);
            }
        }

        private static void ClassDefinition_BeforeCreateTable(Type type)
        {
            if (BeforeCreateTable != null)
            {
                BeforeCreateTable(type);
            }
        }

        protected static void Delete(Type objectType, SearchFilterCollection filters)
        {
            ClassDefinition.GetClassDefinition(objectType).DeleteObject(filters);
        }

        protected static void Delete(Type objectType, params object[] keyPairs)
        {
            ClassDefinition.GetClassDefinition(objectType).DeleteObject(keyPairs);
        }

        protected static object Get(Type objectType, params object[] keyPairs)
        {
            AutoDataSupport support = (AutoDataSupport) ClassDefinition.GetClassDefinition(objectType).GetObject(null, null, keyPairs);
            if (support != null)
            {
                support.OnAfterGet();
            }
            return support;
        }

        public static int GetCount(Type objectType)
        {
            return ClassDefinition.GetClassDefinition(objectType).GetObjectCount();
        }

        public static int GetCount(Type objectType, SearchFilterCollection filter)
        {
            return ClassDefinition.GetClassDefinition(objectType).GetObjectCount(filter);
        }

        protected static object GetVersion(Type objectType, DateTime versionDate, params object[] keyPairs)
        {
            AutoDataSupport support = (AutoDataSupport) ClassDefinition.GetClassDefinition(objectType).GetObject(null, new DateTime?(versionDate), keyPairs);
            if (support != null)
            {
                support.OnAfterGet();
            }
            return support;
        }

        protected static object GetVersion(Type objectType, int versionNumber, params object[] keyPairs)
        {
            AutoDataSupport support = (AutoDataSupport) ClassDefinition.GetClassDefinition(objectType).GetObject(new int?(versionNumber), null, keyPairs);
            if (support != null)
            {
                support.OnAfterGet();
            }
            return support;
        }

        protected static Array List(Type objectType)
        {
            SearchFilterCollection filters = null;
            OrderByClauseCollection orderBy = null;
            return List(objectType, filters, orderBy, 1, 0x7fffffff);
        }

        protected static Array List(Type objectType, OrderByClauseCollection orderBy)
        {
            return List(objectType, null, orderBy, 1, 0x7fffffff);
        }

        protected static Array List(Type objectType, SearchFilterCollection filters)
        {
            OrderByClauseCollection orderBy = null;
            return List(objectType, filters, orderBy, 1, 0x7fffffff);
        }

        protected static Array List(Type objectType, SearchFilterCollection filters, OrderByClauseCollection orderBy)
        {
            return List(objectType, filters, orderBy, 1, 0x7fffffff);
        }

        protected static Array List(Type objectType, SearchFilterCollection filters, int pageNo, int pageSize)
        {
            int num;
            return List(objectType, filters, pageNo, pageSize, out num);
        }

        protected static Array List(Type objectType, SearchFilterCollection filters, OrderByClauseCollection orderBy, int pageNo, int pageSize)
        {
            int num;
            return List(objectType, filters, orderBy, pageNo, pageSize, out num);
        }

        protected static Array List(Type objectType, SearchFilterCollection filters, int pageNo, int pageSize, out int totalRecordCount)
        {
            OrderByClauseCollection orderBy = null;
            return List(objectType, filters, orderBy, pageNo, pageSize, out totalRecordCount);
        }

        protected static Array List(Type objectType, SearchFilterCollection filters, OrderByClauseCollection orderBy, int pageNo, int pageSize, out int totalRecordCount)
        {
            return List(objectType, filters, orderBy, SearchOptions.Default, pageNo, pageSize, out totalRecordCount);
        }

        protected static Array List(Type objectType, SearchFilterCollection filters, OrderByClauseCollection orderBy, SearchOptions searchOptions, int pageNo, int pageSize, out int totalRecordCount)
        {
            Array array = ClassDefinition.GetClassDefinition(objectType).ListObjects(filters, orderBy, searchOptions, pageNo, pageSize, out totalRecordCount);
            foreach (object obj2 in array)
            {
                ((AutoDataSupport) obj2).OnAfterGet();
            }
            return array;
        }

        protected virtual void OnAfterGet()
        {
            if (this.AfterGet != null)
            {
                this.AfterGet(this, null);
            }
        }

        internal static void OnPreExecuteCommand(ClassContext classContext, ref string commandText)
        {
            if (PreExecuteCommand != null)
            {
                PreExecuteCommandEventArgs e = new PreExecuteCommandEventArgs(classContext.ClassType, classContext.Action, commandText);
                PreExecuteCommand(classContext.Sender, e);
                if (e.Changed)
                {
                    commandText = e.CommandText;
                }
            }
        }

        internal static string OnResolveCurrentUser(AutoDataSupport objectToSave)
        {
            if (ResolveCurrentUser == null)
            {
                throw new Exception("Unable to resolve current user.  In order to Auto Resolve the current user you must handle the event AutoDataSupport.ResolveCurrentUser and assign the user.");
            }
            ResolveCurrentUserEventArgs e = new ResolveCurrentUserEventArgs();
            ResolveCurrentUser(objectToSave, e);
            return e.User;
        }

        public static DataSet Query(string query)
        {
            return ClassDefinition.QueryDataSet(query);
        }

        protected static Array Query(Type objectType, string query)
        {
            int num;
            return Query(objectType, query, out num);
        }

        protected static Array Query(Type objectType, string query, out int totalRecordCount)
        {
            Array array = ClassDefinition.GetClassDefinition(objectType).QueryObjects(query, out totalRecordCount);
            foreach (object obj2 in array)
            {
                ((AutoDataSupport) obj2).OnAfterGet();
            }
            return array;
        }

        public static T Rehydrate<T>(DataRow dataRow)
        {
            if (!typeof(T).IsValueType && (typeof(T) != typeof(string)))
            {
                return (T) ClassDefinition.GetClassDefinition(typeof(T)).RehydrateObject(dataRow);
            }
            if (dataRow.IsNull(0))
            {
                return default(T);
            }
            if (typeof(T).IsEnum)
            {
                return (T) Enum.Parse(typeof(T), Convert.ToString(dataRow[0]), true);
            }
            return (T) Convert.ChangeType(dataRow[0], typeof(T));
        }

        public static List<T> Rehydrate<T>(DataTable dataTable)
        {
            ClassDefinition classDefinition = ClassDefinition.GetClassDefinition(typeof(T));
            List<T> list = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add((T) classDefinition.RehydrateObject(row));
            }
            return list;
        }

        public static List<T> Rehydrate<T>(DataSet dataSet, int tableIndex)
        {
            return Rehydrate<T>(dataSet.Tables[tableIndex]);
        }

        public void Save()
        {
            if (this.BeforeSave != null)
            {
                CancelEventArgs e = new CancelEventArgs();
                this.BeforeSave(this, e);
                if (e.Cancel)
                {
                    return;
                }
            }
            ClassDefinition.GetClassDefinition(base.GetType()).SaveObject(this);
            if (this.AfterSave != null)
            {
                this.AfterSave(this, new EventArgs());
            }
        }

        public static void SetConnectionString(Type type, string connectionString)
        {
            SetConnectionString(type, connectionString, false);
        }

        public static void SetConnectionString(Type type, string connectionString, bool currentThreadOnly)
        {
            ConnectionStringManager.SetConnectionString(type, connectionString, currentThreadOnly);
        }

        public static void SetDefaultConnectionString(string connectionString)
        {
            SetDefaultConnectionString(connectionString, false);
        }

        public static void SetDefaultConnectionString(string connectionString, bool currentThreadOnly)
        {
            ConnectionStringManager.SetDefaultConnectionString(connectionString, currentThreadOnly);
        }

        public static void Verify(Type objectType)
        {
            ClassDefinition.GetClassDefinition(objectType);
        }

        public static int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }
    }
}

