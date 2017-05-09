namespace Olympic.AutoDataLayer
{
    using Olympic.AutoDataLayer.Data;
    using System;

    public class AllowNullAttribute : FieldValidationAttribute
    {
        private bool _allowNull;

        public AllowNullAttribute()
        {
        }

        public AllowNullAttribute(bool allowNull)
        {
            this._allowNull = allowNull;
        }

        public override void Test(string className, string classFieldName, object value)
        {
            if (!this._allowNull && BaseTypeNullHelper.IsNull(value))
            {
                throw new Exception("Error validating field '" + classFieldName + "' of class: " + className + ".  Value assigned to field is null, but null is not allowed.");
            }
        }

        public bool AllowNull
        {
            get
            {
                return this._allowNull;
            }
            set
            {
                this._allowNull = value;
            }
        }
    }
}

