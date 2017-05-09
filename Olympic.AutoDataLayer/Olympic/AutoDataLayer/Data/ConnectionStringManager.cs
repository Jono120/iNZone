namespace Olympic.AutoDataLayer.Data
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Runtime.CompilerServices;

    internal class ConnectionStringManager
    {
        private static Hashtable _connectionStrings = new Hashtable();
        private static string _defaultConnectionString;
        [ThreadStatic]
        private static Hashtable _threadScopeConnectionStrings;
        [ThreadStatic]
        private static string _threadScopeDefaultConnectionString;
        [ThreadStatic]
        private static string _threadScopeOverrideConnectionString;

        public static string Get(Type objectType)
        {
            string key = (objectType == null) ? null : objectType.FullName;
            if (_threadScopeOverrideConnectionString != null)
            {
                return _threadScopeOverrideConnectionString;
            }
            if ((_threadScopeConnectionStrings != null) && _threadScopeConnectionStrings.ContainsKey(key))
            {
                return (string) _threadScopeConnectionStrings[key];
            }
            if (_threadScopeDefaultConnectionString != null)
            {
                return _threadScopeDefaultConnectionString;
            }
            if (_connectionStrings[key] != null)
            {
                return (string) _connectionStrings[key];
            }
            if (_defaultConnectionString != null)
            {
                return _defaultConnectionString;
            }
            string str2 = ConfigurationSettings.AppSettings["DefaultConnectionString"];
            if (str2 == null)
            {
                throw new Exception("No connection string has been set for type: " + key);
            }
            return str2;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void InitializeThreadConnectionStringStorage()
        {
            if (_threadScopeConnectionStrings == null)
            {
                _threadScopeConnectionStrings = new Hashtable();
            }
        }

        public static void SetConnectionString(Type objectType, string connectionString, bool currentThreadOnly)
        {
            string fullName = objectType.FullName;
            if (currentThreadOnly)
            {
                InitializeThreadConnectionStringStorage();
                _threadScopeConnectionStrings[fullName] = connectionString;
            }
            else
            {
                _connectionStrings[fullName] = connectionString;
            }
        }

        public static void SetDefaultConnectionString(string connectionString, bool currentThreadOnly)
        {
            if (currentThreadOnly)
            {
                InitializeThreadConnectionStringStorage();
                _threadScopeDefaultConnectionString = connectionString;
            }
            else
            {
                _defaultConnectionString = connectionString;
            }
        }

        public static string OverrideThreadConnectionString
        {
            get
            {
                return _threadScopeOverrideConnectionString;
            }
            set
            {
                _threadScopeOverrideConnectionString = value;
            }
        }
    }
}

