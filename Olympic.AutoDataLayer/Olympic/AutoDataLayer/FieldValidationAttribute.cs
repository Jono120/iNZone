namespace Olympic.AutoDataLayer
{
    using System;

    [AttributeUsage(AttributeTargets.All)]
    public abstract class FieldValidationAttribute : Attribute
    {
        protected FieldValidationAttribute()
        {
        }

        public abstract void Test(string className, string classFieldName, object value);
    }
}

