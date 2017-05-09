namespace Olympic.AutoDataLayer
{
    using Olympic.AutoDataLayer.Data;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public abstract class AutoDataSupport<TObject> : AutoDataSupport
    {
        protected AutoDataSupport()
        {
        }

        public static void Delete(SearchFilterCollection filters)
        {
            AutoDataSupport.Delete(typeof(TObject), filters);
        }

        public static void Delete(params object[] keyPairs)
        {
            AutoDataSupport.Delete(typeof(TObject), keyPairs);
        }

        public static TObject Get(params object[] keyPairs)
        {
            return (TObject) AutoDataSupport.Get(typeof(TObject), keyPairs);
        }

        public static int GetCount()
        {
            return AutoDataSupport.GetCount(typeof(TObject));
        }

        public static int GetCount(SearchFilterCollection filter)
        {
            return AutoDataSupport.GetCount(typeof(TObject), filter);
        }

        public static TObject GetVersion(DateTime versionDate, params object[] keyPairs)
        {
            return (TObject) AutoDataSupport.GetVersion(typeof(TObject), versionDate, keyPairs);
        }

        public static TObject GetVersion(int versionNumber, params object[] keyPairs)
        {
            return (TObject) AutoDataSupport.GetVersion(typeof(TObject), versionNumber, keyPairs);
        }

        public static List<TObject> List()
        {
            return new List<TObject>((TObject[]) AutoDataSupport.List(typeof(TObject)));
        }

        public static List<TObject> List(OrderByClauseCollection orderBy)
        {
            return new List<TObject>((TObject[]) AutoDataSupport.List(typeof(TObject), orderBy));
        }

        public static List<TObject> List(SearchFilterCollection filters)
        {
            return new List<TObject>((TObject[]) AutoDataSupport.List(typeof(TObject), filters));
        }

        public static List<TObject> List(SearchFilterCollection filters, OrderByClauseCollection orderBy)
        {
            return new List<TObject>((TObject[]) AutoDataSupport.List(typeof(TObject), filters, orderBy));
        }

        public static List<TObject> List(SearchFilterCollection filters, int pageNo, int pageSize)
        {
            return new List<TObject>((TObject[]) AutoDataSupport.List(typeof(TObject), filters, pageNo, pageSize));
        }

        public static List<TObject> List(SearchFilterCollection filters, OrderByClauseCollection orderBy, int pageNo, int pageSize)
        {
            return new List<TObject>((TObject[]) AutoDataSupport.List(typeof(TObject), filters, orderBy, pageNo, pageSize));
        }

        public static List<TObject> List(SearchFilterCollection filters, int pageNo, int pageSize, out int totalRecordCount)
        {
            return new List<TObject>((TObject[]) AutoDataSupport.List(typeof(TObject), filters, pageNo, pageSize, out totalRecordCount));
        }

        public static List<TObject> List(SearchFilterCollection filters, OrderByClauseCollection orderBy, int pageNo, int pageSize, out int totalRecordCount)
        {
            return new List<TObject>((TObject[]) AutoDataSupport.List(typeof(TObject), filters, orderBy, pageNo, pageSize, out totalRecordCount));
        }

        public static List<TObject> List(SearchFilterCollection filters, OrderByClauseCollection orderBy, SearchOptions searchOptions, int pageNo, int pageSize, out int totalRecordCount)
        {
            return new List<TObject>((TObject[]) AutoDataSupport.List(typeof(TObject), filters, orderBy, searchOptions, pageNo, pageSize, out totalRecordCount));
        }

        public static List<TObject> Query(string query)
        {
            return new List<TObject>((TObject[]) AutoDataSupport.Query(typeof(TObject), query));
        }

        public static List<TObject> Query(string query, out int totalRecordCount)
        {
            return new List<TObject>((TObject[]) AutoDataSupport.Query(typeof(TObject), query, out totalRecordCount));
        }

        public static void SetConnectionString(string connectionString)
        {
            AutoDataSupport.SetConnectionString(typeof(TObject), connectionString, false);
        }

        public static void SetConnectionString(string connectionString, bool currentThreadOnly)
        {
            ConnectionStringManager.SetConnectionString(typeof(TObject), connectionString, currentThreadOnly);
        }

        public static void Verify()
        {
            AutoDataSupport.Verify(typeof(TObject));
        }
    }
}

