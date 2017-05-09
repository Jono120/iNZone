namespace Olympic.AutoDataLayer
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AggregateAttribute : Attribute
    {
        private Olympic.AutoDataLayer.AggregateFunction _aggregateFunction;
        private bool _distinct;
        private string _fieldName;

        public AggregateAttribute()
        {
        }

        public AggregateAttribute(Olympic.AutoDataLayer.AggregateFunction aggregateFunction, string fieldName) : this(aggregateFunction, fieldName, false)
        {
        }

        public AggregateAttribute(Olympic.AutoDataLayer.AggregateFunction aggregateFunction, string fieldName, bool distinct)
        {
            this._aggregateFunction = aggregateFunction;
            this._fieldName = fieldName;
            this._distinct = distinct;
        }

        public Olympic.AutoDataLayer.AggregateFunction AggregateFunction
        {
            get
            {
                return this._aggregateFunction;
            }
            set
            {
                this._aggregateFunction = value;
            }
        }

        public bool Distinct
        {
            get
            {
                return this._distinct;
            }
            set
            {
                this._distinct = value;
            }
        }

        public string FieldName
        {
            get
            {
                return this._fieldName;
            }
            set
            {
                this._fieldName = value;
            }
        }
    }
}

