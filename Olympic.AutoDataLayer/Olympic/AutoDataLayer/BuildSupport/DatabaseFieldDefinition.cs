namespace Olympic.AutoDataLayer.BuildSupport
{
    using Olympic.AutoDataLayer;
    using Olympic.AutoDataLayer.ClassSupport;
    using Olympic.AutoDataLayer.Data;
    using System;
    using System.Data;

    internal class DatabaseFieldDefinition
    {
        public string AggregateFunction;
        public bool AllowNull;
        public bool AutoNumber;
        public int DecimalPlaces;
        public string FieldName;
        public int FieldSize;
        public DatabaseFieldType FieldType;
        public bool IsAggregate;
        public bool PrimaryKey;

        public DatabaseFieldDefinition()
        {
        }

        public DatabaseFieldDefinition(ClassFieldDefinition classFieldDefinition)
        {
            this.FieldName = classFieldDefinition.DatabaseFieldName;
            this.PrimaryKey = classFieldDefinition.Unique;
            this.AutoNumber = classFieldDefinition.AutoNumber;
            this.AllowNull = classFieldDefinition.AllowNull;
            this.IsAggregate = classFieldDefinition.IsAggregate;
            if (this.IsAggregate)
            {
                this.AggregateFunction = this.CalculateAggregateFunction(classFieldDefinition.AggregateFunction, classFieldDefinition.AggregateFieldName, classFieldDefinition.AggregateDistinct);
            }
            if (BaseTypeNullHelper.IsNull(classFieldDefinition.ForceFieldType))
            {
                this.FieldType = this.GetDatabaseFieldType(classFieldDefinition.SystemType, classFieldDefinition.MaxLength);
            }
            else
            {
                this.FieldType = classFieldDefinition.ForceFieldType;
            }
            if ((((this.FieldType == DatabaseFieldType.Binary) || (this.FieldType == DatabaseFieldType.Char)) || ((this.FieldType == DatabaseFieldType.Decimal) || (this.FieldType == DatabaseFieldType.NChar))) || (((this.FieldType == DatabaseFieldType.NVarChar) || (this.FieldType == DatabaseFieldType.VarBinary)) || (this.FieldType == DatabaseFieldType.VarChar)))
            {
                this.FieldSize = classFieldDefinition.MaxLength;
            }
            else
            {
                this.FieldSize = 0;
            }
            if (this.FieldType == DatabaseFieldType.Decimal)
            {
                this.DecimalPlaces = classFieldDefinition.DecimalPlaces;
            }
        }

        public DatabaseFieldDefinition(DataRow columnRow, bool primaryKey)
        {
            string str = Convert.ToString(columnRow["ColumnName"]);
            string str2 = Convert.ToString(columnRow["Type"]);
            int num = Convert.ToInt32(columnRow["Length"]);
            string str3 = Convert.ToString(columnRow["Precision"]);
            string str4 = Convert.ToString(columnRow["Scale"]);
            bool flag = Convert.ToBoolean(columnRow["Nullable"]);
            bool flag2 = Convert.ToBoolean(columnRow["Identity"]);
            this.FieldName = str;
            this.FieldType = (DatabaseFieldType) Enum.Parse(typeof(DatabaseFieldType), str2, true);
            if ((this.FieldType == DatabaseFieldType.NVarChar) || (this.FieldType == DatabaseFieldType.NChar))
            {
                this.FieldSize = num / 2;
            }
            if (((this.FieldType == DatabaseFieldType.Binary) || (this.FieldType == DatabaseFieldType.Char)) || ((this.FieldType == DatabaseFieldType.VarBinary) || (this.FieldType == DatabaseFieldType.VarChar)))
            {
                this.FieldSize = num;
            }
            this.AutoNumber = flag2;
            this.AllowNull = flag;
            if (this.FieldType == DatabaseFieldType.Decimal)
            {
                this.FieldSize = Convert.ToInt32(str3);
                this.DecimalPlaces = Convert.ToInt32(str4);
            }
            this.PrimaryKey = primaryKey;
        }

        private string CalculateAggregateFunction(Olympic.AutoDataLayer.AggregateFunction aggregateFunction, string fieldName, bool distinct)
        {
            if (fieldName != "*")
            {
                fieldName = "[" + fieldName + "]";
            }
            switch (aggregateFunction)
            {
                case Olympic.AutoDataLayer.AggregateFunction.Count:
                    return ("COUNT(" + (distinct ? "DISTINCT " : "") + fieldName + ")");

                case Olympic.AutoDataLayer.AggregateFunction.Average:
                    return ("AVG(" + (distinct ? "DISTINCT " : "") + fieldName + ")");

                case Olympic.AutoDataLayer.AggregateFunction.Sum:
                    return ("SUM(" + (distinct ? "DISTINCT " : "") + fieldName + ")");

                case Olympic.AutoDataLayer.AggregateFunction.Minimum:
                    return ("MIN(" + fieldName + ")");

                case Olympic.AutoDataLayer.AggregateFunction.Maximum:
                    return ("MAX(" + fieldName + ")");
            }
            throw new Exception("Unsupported aggregation function supplied");
        }

        private DatabaseFieldType GetDatabaseFieldType(Type type, int maxLength)
        {
            if (type == typeof(string))
            {
                if ((maxLength != 0) && (maxLength <= 0xfa0))
                {
                    return DatabaseFieldType.NVarChar;
                }
                return DatabaseFieldType.NText;
            }
            if ((type == typeof(double)) || (type == typeof(decimal)))
            {
                return DatabaseFieldType.Float;
            }
            if (type == typeof(int))
            {
                return DatabaseFieldType.Int;
            }
            if (type == typeof(long))
            {
                return DatabaseFieldType.BigInt;
            }
            if (type == typeof(bool))
            {
                return DatabaseFieldType.Bit;
            }
            if (type == typeof(DateTime))
            {
                return DatabaseFieldType.DateTime;
            }
            if (type == typeof(byte[]))
            {
                return DatabaseFieldType.Image;
            }
            if (type == typeof(byte))
            {
                return DatabaseFieldType.TinyInt;
            }
            if (type == typeof(short))
            {
                return DatabaseFieldType.SmallInt;
            }
            if (type.IsEnum)
            {
                return DatabaseFieldType.NVarChar;
            }
            if (type != typeof(Guid))
            {
                throw new Exception("Type: " + type.ToString() + " is not supported by Auto Data Layer.");
            }
            return DatabaseFieldType.UniqueIdentifier;
        }

        public string ToQuotedSqlObjectString()
        {
            return ("[" + this.FieldName + "]");
        }

        public string ToTableDeclarationScript(bool allowIdentity)
        {
            string str = this.ToQuotedSqlObjectString();
            string str2 = " " + ((this.AutoNumber && allowIdentity) ? "IDENTITY (1, 1)" : "");
            string str3 = " " + (this.AllowNull ? "NULL" : "NOT NULL");
            return (str + " " + this.ToTypeDeclarationScript() + str2.TrimEnd(new char[0]) + str3.TrimEnd(new char[0]));
        }

        public string ToTypeDeclarationScript()
        {
            string str = "[" + this.FieldType.ToString() + "]";
            string str2 = " ";
            if ((((this.FieldType == DatabaseFieldType.NVarChar) || (this.FieldType == DatabaseFieldType.Binary)) || ((this.FieldType == DatabaseFieldType.Char) || (this.FieldType == DatabaseFieldType.NChar))) || ((this.FieldType == DatabaseFieldType.VarBinary) || (this.FieldType == DatabaseFieldType.VarChar)))
            {
                object obj2 = str2;
                str2 = string.Concat(new object[] { obj2, "(", this.FieldSize, ")" });
            }
            if (this.FieldType == DatabaseFieldType.Decimal)
            {
                object obj3 = str2;
                str2 = string.Concat(new object[] { obj3, "(", this.FieldSize, ", ", this.DecimalPlaces, ")" });
            }
            return (str + str2.TrimEnd(new char[0]));
        }
    }
}

