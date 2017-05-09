namespace Olympic.AutoDataLayer.ClassSupport
{
    using Olympic.AutoDataLayer;
    using Olympic.AutoDataLayer.Data;
    using Olympic.AutoDataLayer.Tools;
    using System;
    using System.Collections;
    using System.Reflection;

    internal class ClassFieldAttributeInfo
    {
        public bool AggregateDistinct;
        public string AggregateFieldName;
        public Olympic.AutoDataLayer.AggregateFunction AggregateFunction;
        public bool AllowNull = true;
        public bool AutoNumber;
        public string DatabaseFieldName;
        public int DecimalPlaces;
        public DatabaseFieldType FieldType = ((DatabaseFieldType) BaseTypeNullHelper.NullEnum(typeof(DatabaseFieldType)));
        public bool Include;
        public IndexAttribute[] Indexes;
        public bool IsAggregate;
        public int MaxLength;
        public bool ReadOnly;
        public System.Type Type;
        public bool Unique;
        public FieldValidationAttribute[] Validators;

        public ClassFieldAttributeInfo(MemberInfo memberInfo)
        {
            this.DatabaseFieldName = memberInfo.Name;
            bool isPublic = false;
            bool canRead = true;
            bool canWrite = true;
            bool flag4 = false;
            bool flag5 = false;
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                FieldInfo field = memberInfo.DeclaringType.GetField(memberInfo.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                this.Type = NullableTypeHelper.GetUnderlyingTypeFromNullableType(field.FieldType);
                isPublic = field.IsPublic;
            }
            else
            {
                if (memberInfo.MemberType != MemberTypes.Property)
                {
                    throw new Exception("Member Type: " + memberInfo.MemberType + " is not supported by AutoDataSupport.");
                }
                PropertyInfo property = memberInfo.DeclaringType.GetProperty(memberInfo.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                this.Type = NullableTypeHelper.GetUnderlyingTypeFromNullableType(property.PropertyType);
                isPublic = true;
                foreach (MethodInfo info3 in property.GetAccessors(true))
                {
                    if (info3.IsPrivate)
                    {
                        isPublic = false;
                    }
                }
                canRead = property.CanRead;
                canWrite = property.CanWrite;
            }
            foreach (object obj2 in memberInfo.GetCustomAttributes(typeof(UniqueAttribute), true))
            {
                this.Unique = true;
                this.AllowNull = false;
                UniqueAttribute attribute = (UniqueAttribute) obj2;
                this.AutoNumber = attribute.AutoNumber;
            }
            if (this.Type == typeof(string))
            {
                this.MaxLength = 0x100;
            }
            if (this.Type.IsEnum)
            {
                string[] names = Enum.GetNames(this.Type);
                int num = 0;
                foreach (string str in names)
                {
                    num = Math.Max(num, str.Length);
                }
                this.MaxLength = num;
            }
            foreach (object obj3 in memberInfo.GetCustomAttributes(typeof(AggregateAttribute), true))
            {
                AggregateAttribute attribute2 = (AggregateAttribute) obj3;
                this.IsAggregate = true;
                this.AggregateFunction = attribute2.AggregateFunction;
                this.AggregateFieldName = attribute2.FieldName;
                this.AggregateDistinct = attribute2.Distinct;
            }
            ArrayList list = new ArrayList();
            foreach (object obj4 in memberInfo.GetCustomAttributes(typeof(FieldValidationAttribute), true))
            {
                FieldValidationAttribute attribute3 = (FieldValidationAttribute) obj4;
                if (attribute3 is AllowNullAttribute)
                {
                    this.AllowNull = ((AllowNullAttribute) attribute3).AllowNull;
                }
                if (attribute3 is StringValidationAttribute)
                {
                    this.MaxLength = ((StringValidationAttribute) attribute3).MaxLength;
                }
                list.Add(attribute3);
            }
            this.Validators = (FieldValidationAttribute[]) list.ToArray(typeof(FieldValidationAttribute));
            object[] customAttributes = memberInfo.GetCustomAttributes(typeof(ExcludeDBAttribute), true);
            int index = 0;
            while (index < customAttributes.Length)
            {
                object obj1 = customAttributes[index];
                flag5 = true;
                break;
            }
            foreach (object obj5 in memberInfo.GetCustomAttributes(typeof(IncludeDBAttribute), true))
            {
                flag4 = true;
                IncludeDBAttribute attribute4 = (IncludeDBAttribute) obj5;
                if ((attribute4.FieldName != null) && (attribute4.FieldName.Length > 0))
                {
                    this.DatabaseFieldName = attribute4.FieldName;
                }
                if (!BaseTypeNullHelper.IsNull(attribute4.FieldType))
                {
                    this.FieldType = attribute4.FieldType;
                    this.MaxLength = attribute4.FieldSize;
                    this.DecimalPlaces = attribute4.DecimalPlaces;
                }
            }
            ArrayList list2 = new ArrayList();
            foreach (object obj6 in memberInfo.GetCustomAttributes(typeof(IndexAttribute), true))
            {
                IndexAttribute attribute5 = (IndexAttribute) obj6;
                list2.Add(attribute5);
            }
            this.Indexes = (IndexAttribute[]) list2.ToArray(typeof(IndexAttribute));
            if ((canRead && !canWrite) && flag4)
            {
                this.ReadOnly = true;
            }
            if ((!canRead || (!canWrite && !this.ReadOnly)) || flag5)
            {
                this.Include = false;
            }
            else if (isPublic || flag4)
            {
                this.Include = true;
            }
            else
            {
                this.Include = false;
            }
        }
    }
}

