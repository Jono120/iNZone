namespace Olympic.AutoDataLayer.BuildSupport
{
    using Olympic.AutoDataLayer;
    using Olympic.AutoDataLayer.ClassSupport;
    using Olympic.AutoDataLayer.Data;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Reflection;

    internal class IndexDefinitionCollection : NameObjectCollectionBase
    {
        internal IndexDefinitionCollection()
        {
        }

        public IndexDefinitionCollection(ClassDefinition classDefinition)
        {
            bool flag = false;
            foreach (ClassFieldDefinition definition in classDefinition.ClassFields)
            {
                foreach (IndexAttribute attribute in definition.Indexes)
                {
                    string name = attribute.Name;
                    bool clustered = attribute.Clustered;
                    bool unique = attribute.Unique;
                    OrderType sortOrder = attribute.SortOrder;
                    if (name == null)
                    {
                        name = classDefinition.TableName + "__" + definition.DatabaseFieldName;
                    }
                    IndexDefinition fieldDefinition = this[name];
                    if (fieldDefinition == null)
                    {
                        fieldDefinition = new IndexDefinition();
                        fieldDefinition.Name = name;
                        this.Add(fieldDefinition);
                    }
                    if (clustered)
                    {
                        if (flag && !fieldDefinition.Clustered)
                        {
                            throw new Exception("You have defined more than one clustered index.  Can not apply clustered index to field: " + definition.Name);
                        }
                        fieldDefinition.Clustered = true;
                        flag = true;
                    }
                    if (unique)
                    {
                        fieldDefinition.Unique = true;
                    }
                    IndexFieldDefinition definition3 = new IndexFieldDefinition();
                    definition3.Name = definition.DatabaseFieldName;
                    definition3.SortOrder = sortOrder;
                    fieldDefinition.Fields.Add(definition3);
                }
            }
        }

        public void Add(IndexDefinition fieldDefinition)
        {
            base.BaseAdd(fieldDefinition.Name, fieldDefinition);
        }

        public bool Contains(string fieldName)
        {
            return (base.BaseGet(fieldName) != null);
        }

        public bool ContainsClusteredIndex()
        {
            IndexDefinitionCollectionEnumerator enumerator = this.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Clustered)
                {
                    return true;
                }
            }

            return false;
        }

        public IndexDefinitionCollectionEnumerator GetEnumerator()
        {
            return new IndexDefinitionCollectionEnumerator(this);
        }

        public static IndexDefinitionCollection LoadFromDatabase(string connectionString, string tableName, ClassContext context)
        {
            IndexDefinitionCollection definitions = new IndexDefinitionCollection();
            DataSet set = CommandBuilder.GetListIndexesCommand(connectionString, tableName, context).ExecuteDataSet();
            if (set.Tables.Count != 0)
            {
                DataTable table = set.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    string str = Convert.ToString(row["index_name"]);
                    string str2 = Convert.ToString(row["index_description"]).ToLower();
                    string str3 = Convert.ToString(row["index_keys"]);
                    if (str2.IndexOf("primary key") < 0)
                    {
                        IndexDefinition fieldDefinition = new IndexDefinition();
                        fieldDefinition.Name = str;
                        fieldDefinition.Clustered = (str2.IndexOf("clustered") > -1) && (str2.IndexOf("nonclustered") < 0);
                        fieldDefinition.Unique = str2.IndexOf("unique key") > -1;
                        foreach (string str4 in str3.Split(new char[] { ',' }))
                        {
                            OrderType ascending = OrderType.Ascending;
                            if (str4.IndexOf("(-)") > -1)
                            {
                                ascending = OrderType.Descending;
                            }
                            string str5 = str4.Replace("(+)", "").Replace("(-)", "").Trim();
                            IndexFieldDefinition definition2 = new IndexFieldDefinition();
                            definition2.Name = str5;
                            definition2.SortOrder = ascending;
                            fieldDefinition.Fields.Add(definition2);
                        }
                        definitions.Add(fieldDefinition);
                    }
                }
            }
            return definitions;
        }

        public void Remove(string fieldName)
        {
            base.BaseRemove(fieldName);
        }

        public IndexDefinition this[string fieldName]
        {
            get
            {
                return (IndexDefinition) base.BaseGet(fieldName);
            }
        }

        public IndexDefinition this[int index]
        {
            get
            {
                return (IndexDefinition) base.BaseGet(index);
            }
        }

        public class IndexDefinitionCollectionEnumerator
        {
            private IndexDefinitionCollection collection;
            private int nIndex;

            public IndexDefinitionCollectionEnumerator(IndexDefinitionCollection coll)
            {
                this.collection = coll;
                this.nIndex = -1;
            }

            public bool MoveNext()
            {
                this.nIndex++;
                return (this.nIndex < this.collection.Count);
            }

            public IndexDefinition Current
            {
                get
                {
                    return (IndexDefinition) this.collection.BaseGet(this.nIndex);
                }
            }
        }
    }
}

