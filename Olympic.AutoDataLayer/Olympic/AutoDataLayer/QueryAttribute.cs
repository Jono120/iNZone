namespace Olympic.AutoDataLayer
{
    using System;

    public class QueryAttribute : DataSourceAttribute
    {
        private string _query;

        public QueryAttribute()
        {
        }

        public QueryAttribute(string query)
        {
            this._query = query;
        }

        public override string DatasourceValidationKey
        {
            get
            {
                return ("Query(" + this._query + ")");
            }
        }

        public string Query
        {
            get
            {
                return this._query;
            }
        }
    }
}

