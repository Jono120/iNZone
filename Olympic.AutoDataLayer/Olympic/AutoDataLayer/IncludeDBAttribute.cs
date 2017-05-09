namespace Olympic.AutoDataLayer
{
    using Olympic.AutoDataLayer.Data;
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IncludeDBAttribute : Attribute
    {
        private int _decimalPlaces;
        private string _fieldName;
        private int _fieldSize;
        private DatabaseFieldType _fieldType;

        public IncludeDBAttribute()
        {
            this._fieldType = (DatabaseFieldType) BaseTypeNullHelper.NullEnum(typeof(DatabaseFieldType));
        }

        public IncludeDBAttribute(DatabaseFieldType fieldType)
        {
            this._fieldType = (DatabaseFieldType) BaseTypeNullHelper.NullEnum(typeof(DatabaseFieldType));
            this._fieldType = fieldType;
        }

        public IncludeDBAttribute(string fieldName)
        {
            this._fieldType = (DatabaseFieldType) BaseTypeNullHelper.NullEnum(typeof(DatabaseFieldType));
            this._fieldName = fieldName;
        }

        public IncludeDBAttribute(DatabaseFieldType fieldType, int fieldSize)
        {
            this._fieldType = (DatabaseFieldType) BaseTypeNullHelper.NullEnum(typeof(DatabaseFieldType));
            this._fieldType = fieldType;
            this._fieldSize = fieldSize;
        }

        public IncludeDBAttribute(int fieldSize, int decimalPlaces)
        {
            this._fieldType = (DatabaseFieldType) BaseTypeNullHelper.NullEnum(typeof(DatabaseFieldType));
            this._fieldType = DatabaseFieldType.Decimal;
            this._fieldSize = fieldSize;
            this._decimalPlaces = decimalPlaces;
        }

        public IncludeDBAttribute(string fieldName, DatabaseFieldType fieldType)
        {
            this._fieldType = (DatabaseFieldType) BaseTypeNullHelper.NullEnum(typeof(DatabaseFieldType));
            this._fieldName = fieldName;
            this._fieldType = fieldType;
        }

        public IncludeDBAttribute(string fieldName, DatabaseFieldType fieldType, int fieldSize)
        {
            this._fieldType = (DatabaseFieldType) BaseTypeNullHelper.NullEnum(typeof(DatabaseFieldType));
            this._fieldName = fieldName;
            this._fieldType = fieldType;
            this._fieldSize = fieldSize;
        }

        public IncludeDBAttribute(string fieldName, int fieldSize, int decimalPlaces)
        {
            this._fieldType = (DatabaseFieldType) BaseTypeNullHelper.NullEnum(typeof(DatabaseFieldType));
            this._fieldName = fieldName;
            this._fieldType = DatabaseFieldType.Decimal;
            this._fieldSize = fieldSize;
            this._decimalPlaces = decimalPlaces;
        }

        public int DecimalPlaces
        {
            get
            {
                return this._decimalPlaces;
            }
            set
            {
                this._decimalPlaces = value;
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

        public int FieldSize
        {
            get
            {
                return this._fieldSize;
            }
            set
            {
                this._fieldSize = value;
            }
        }

        public DatabaseFieldType FieldType
        {
            get
            {
                return this._fieldType;
            }
            set
            {
                this._fieldType = value;
            }
        }
    }
}

