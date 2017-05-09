namespace Olympic.AutoDataLayer
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public abstract class DataSourceAttribute : Attribute
    {
        protected DataSourceAttribute()
        {
        }

        public abstract string DatasourceValidationKey { get; }
    }
}

