namespace Olympic.AutoDataLayer
{
    using System;

    public class StringValidationAttribute : FieldValidationAttribute
    {
        private int _maxLength;
        public const int DefaultMaxLength = 0x100;

        public StringValidationAttribute()
        {
            this._maxLength = 0x100;
        }

        public StringValidationAttribute(int maxLength)
        {
            this._maxLength = 0x100;
            this._maxLength = maxLength;
        }

        public override void Test(string className, string classFieldName, object value)
        {
            if (value != null)
            {
                string str = value.ToString();
                if ((this._maxLength > 0) && (str.Length > this._maxLength))
                {
                    throw new Exception(string.Concat(new object[] { "Error validating field '", classFieldName, "' of class: ", className, ".  String length of ", str.Length, " is greater than the allowed length of ", this._maxLength }));
                }
            }
        }

        public int MaxLength
        {
            get
            {
                return this._maxLength;
            }
            set
            {
                this._maxLength = value;
            }
        }
    }
}

