namespace Olympic.AutoDataLayer
{
    using System;

    [AttributeUsage(AttributeTargets.All)]
    public class UniqueAttribute : Attribute
    {
        private bool _autoNumber;

        public UniqueAttribute()
        {
            this._autoNumber = false;
        }

        public UniqueAttribute(bool autoNumber)
        {
            this._autoNumber = autoNumber;
        }

        public bool AutoNumber
        {
            get
            {
                return this._autoNumber;
            }
        }
    }
}

