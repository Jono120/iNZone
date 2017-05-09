namespace Olympic.AutoDataLayer.ClassSupport
{
    using Olympic.AutoDataLayer;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;

    internal class ClassFieldDefinition
    {
        private FieldInfo _fieldInfo;
        private System.Reflection.MemberInfo _memberInfo;
        private MemberTypes _memberType;
        private PropertyInfo _propertyInfo;
        public bool AggregateDistinct;
        public string AggregateFieldName;
        public Olympic.AutoDataLayer.AggregateFunction AggregateFunction;
        public bool AllowNull;
        public bool AutoNumber;
        public string DatabaseFieldName;
        public int DecimalPlaces;
        public DatabaseFieldType ForceFieldType;
        public IndexAttribute[] Indexes;
        public bool IsAggregate;
        public bool IsModifiedByField;
        public bool IsModifiedDateField;
        public bool IsVersionNumberField;
        public int MaxLength;
        public string Name;
        public bool ReadOnly;
        public Type SystemType;
        public bool Unique;
        public FieldValidationAttribute[] Validators;

        public ClassFieldDefinition()
        {
        }

        private ClassFieldDefinition(System.Reflection.MemberInfo memberInfo, ClassFieldAttributeInfo attributeInfo)
        {
            this.Name = memberInfo.Name;
            this.DatabaseFieldName = attributeInfo.DatabaseFieldName;
            this.SystemType = attributeInfo.Type;
            this.Unique = attributeInfo.Unique;
            this.AutoNumber = attributeInfo.AutoNumber;
            this.AllowNull = attributeInfo.AllowNull;
            this.MaxLength = attributeInfo.MaxLength;
            this.ReadOnly = attributeInfo.ReadOnly;
            this.ForceFieldType = attributeInfo.FieldType;
            this.DecimalPlaces = attributeInfo.DecimalPlaces;
            this.IsAggregate = attributeInfo.IsAggregate;
            this.AggregateFunction = attributeInfo.AggregateFunction;
            this.AggregateFieldName = attributeInfo.AggregateFieldName;
            this.AggregateDistinct = attributeInfo.AggregateDistinct;
            this._memberInfo = memberInfo;
            this._propertyInfo = null;
            this._fieldInfo = null;
            this._memberType = memberInfo.MemberType;
            if (this._memberType == MemberTypes.Field)
            {
                this._fieldInfo = this._memberInfo.DeclaringType.GetField(this._memberInfo.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }
            else if (this._memberType == MemberTypes.Property)
            {
                this._propertyInfo = this._memberInfo.DeclaringType.GetProperty(this._memberInfo.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }
            this.Validators = attributeInfo.Validators;
            this.Indexes = attributeInfo.Indexes;
        }

        public static ClassFieldDefinition Get(System.Reflection.MemberInfo memberInfo)
        {
            ClassFieldAttributeInfo attributeInfo = new ClassFieldAttributeInfo(memberInfo);
            if (attributeInfo.Include)
            {
                return new ClassFieldDefinition(memberInfo, attributeInfo);
            }
            return null;
        }

        private object GetPreparedByteArrayValue(byte[] value, DatabaseFieldType targetType)
        {
            if (targetType != DatabaseFieldType.Image)
            {
                return value;
            }
            SqlParameter parameter = new SqlParameter("", SqlDbType.Image);
            if (value == null)
            {
                parameter.Value = DBNull.Value;
                return parameter;
            }
            parameter.Value = value;
            return parameter;
        }

        private object GetPreparedEnumValue(Enum value, DatabaseFieldType targetType)
        {
            if ((((targetType == DatabaseFieldType.BigInt) || (targetType == DatabaseFieldType.Decimal)) || ((targetType == DatabaseFieldType.Float) || (targetType == DatabaseFieldType.Int))) || (((targetType == DatabaseFieldType.Money) || (targetType == DatabaseFieldType.SmallInt)) || ((targetType == DatabaseFieldType.SmallMoney) || (targetType == DatabaseFieldType.TinyInt))))
            {
                return Convert.ToInt32(value);
            }
            if (value.ToString() == "0")
            {
                return null;
            }
            return value.ToString();
        }

        public object GetPreparedValue(object sourceObject, DatabaseFieldType targetType)
        {
            object obj2 = this.GetValue(sourceObject);
            if (this.SystemType.IsEnum)
            {
                return this.GetPreparedEnumValue((Enum) obj2, targetType);
            }
            if (this.SystemType == typeof(byte[]))
            {
                return this.GetPreparedByteArrayValue((byte[]) obj2, targetType);
            }
            return obj2;
        }

        public object GetValue(object targetObject)
        {
            if (this._memberType == MemberTypes.Field)
            {
                return this._fieldInfo.GetValue(targetObject);
            }
            if (this._memberType == MemberTypes.Property)
            {
                return this._propertyInfo.GetValue(targetObject, null);
            }
            return null;
        }

        public void SetValue(object targetObject, object valueToAssign)
        {
            if (this._memberType == MemberTypes.Field)
            {
                this._fieldInfo.SetValue(targetObject, valueToAssign);
            }
            else if (this._memberType == MemberTypes.Property)
            {
                this._propertyInfo.SetValue(targetObject, valueToAssign, null);
            }
        }

        public void Validate(object objectToValidate)
        {
            object obj2 = this.GetValue(objectToValidate);
            foreach (FieldValidationAttribute attribute in this.Validators)
            {
                attribute.Test(objectToValidate.GetType().FullName, this.Name, obj2);
            }
        }

        public bool IsVersionField
        {
            get
            {
                if (!this.IsVersionNumberField && !this.IsModifiedByField)
                {
                    return this.IsModifiedDateField;
                }
                return true;
            }
        }

        public System.Reflection.MemberInfo MemberInfo
        {
            get
            {
                return this._memberInfo;
            }
        }
    }
}

