namespace Olympic.AutoDataLayer.BuildSupport
{
    using Olympic.AutoDataLayer.ClassSupport;
    using Olympic.AutoDataLayer.Data;
    using System;
    using System.Collections;
    using System.Data;

    internal class TableDefinition
    {
        public DatabaseFieldDefinitionCollection AggregateFields;
        public DatabaseFieldDefinitionCollection Fields;
        public IndexDefinitionCollection Indexes;
        public string TableName;
        public string VersionHistoryTable;
        internal const string VersionHistoryTablePrefix = "VersionHistory_";

        public TableDefinition()
        {
        }

        public TableDefinition(ClassDefinition classDefinition, DataTableTarget dataTableTarget)
        {
            this.TableName = (dataTableTarget == DataTableTarget.Main) ? classDefinition.TableName : classDefinition.VersionHistoryTableName;
            this.Fields = new DatabaseFieldDefinitionCollection();
            this.AggregateFields = new DatabaseFieldDefinitionCollection();
            foreach (ClassFieldDefinition definition in classDefinition.ClassFields)
            {
                if (definition.IsAggregate)
                {
                    this.AggregateFields.Add(new DatabaseFieldDefinition(definition));
                    continue;
                }
                DatabaseFieldDefinition fieldDefinition = new DatabaseFieldDefinition(definition);
                if (dataTableTarget == DataTableTarget.VersionHistory)
                {
                    if (definition.IsVersionNumberField)
                    {
                        fieldDefinition.PrimaryKey = true;
                        fieldDefinition.AllowNull = false;
                    }
                    if (definition.AutoNumber)
                    {
                        fieldDefinition.AutoNumber = false;
                    }
                }
                this.Fields.Add(fieldDefinition);
            }
            this.Indexes = new IndexDefinitionCollection(classDefinition);
            if ((dataTableTarget == DataTableTarget.Main) && classDefinition.KeepVersionHistory)
            {
                this.VersionHistoryTable = classDefinition.VersionHistoryTableName;
            }
        }

        public TableDefinition(string tableName, DatabaseFieldDefinitionCollection fields, IndexDefinitionCollection indexes, string versionHistoryTable)
        {
            this.TableName = tableName;
            this.Fields = fields;
            this.Indexes = indexes;
            this.VersionHistoryTable = versionHistoryTable;
        }

        public void CreateTable(string connectionString, bool preserveData, ClassContext context)
        {
            CommandBuilder.GetCreateTableCommand(connectionString, this, preserveData, context).ExecuteNonQuery();
            if (this.VersionHistoryTable != null)
            {
                CommandBuilder.GetCreateVersionHistoryTriggerCommand(connectionString, this, context).ExecuteNonQuery();
            }
        }

        public void DropTable(string connectionString, ClassContext context)
        {
            CommandBuilder.GetDropTableCommand(connectionString, this.TableName, context).ExecuteNonQuery();
        }

        public static TableDefinition LoadFromDatabase(string connectionString, string tableName, ClassContext context)
        {
            DataSet set = CommandBuilder.GetListTableColumnsCommand(connectionString, tableName, context).ExecuteDataSet();
            DataTable table = set.Tables[0];
            if (!Convert.ToBoolean(table.Rows[0]["Exists"]))
            {
                return null;
            }
            DataTable table2 = set.Tables[1];
            DataSet set2 = CommandBuilder.GetListContraintsCommand(connectionString, tableName, context).ExecuteDataSet();
            ArrayList list = new ArrayList();
            if (set2.Tables.Count > 0)
            {
                foreach (DataRow row in set2.Tables[0].Rows)
                {
                    string str = Convert.ToString(row["constraint_type"]);
                    string str2 = Convert.ToString(row["constraint_keys"]);
                    if (str.ToLower().StartsWith("primary key"))
                    {
                        list.AddRange(str2.Replace(" ", "").Split(new char[] { ',' }));
                        break;
                    }
                }
            }
            DatabaseFieldDefinitionCollection fields = new DatabaseFieldDefinitionCollection();
            foreach (DataRow row2 in table2.Rows)
            {
                string item = Convert.ToString(row2["ColumnName"]);
                bool primaryKey = list.Contains(item);
                fields.Add(new DatabaseFieldDefinition(row2, primaryKey));
            }
            IndexDefinitionCollection indexes = IndexDefinitionCollection.LoadFromDatabase(connectionString, tableName, context);
            string versionHistoryTable = null;
            DataSet set3 = CommandBuilder.GetListTriggersCommand(connectionString, tableName, context).ExecuteDataSet();
            if (set3.Tables.Count > 0)
            {
                foreach (DataRow row3 in set3.Tables[0].Rows)
                {
                    string str5 = Convert.ToString(row3["trigger_name"]);
                    if (str5.StartsWith("VersionHistory_"))
                    {
                        versionHistoryTable = str5.Substring("VersionHistory_".Length);
                        break;
                    }
                }
            }
            return new TableDefinition(tableName, fields, indexes, versionHistoryTable);
        }

        public bool Verify(TableDefinition tableDefinition, DataTableTarget dataTableTarget)
        {
            if (tableDefinition == null)
            {
                return false;
            }
            if (tableDefinition.TableName != this.TableName)
            {
                return false;
            }
            if (this.Fields.Count != tableDefinition.Fields.Count)
            {
                return false;
            }
            foreach (DatabaseFieldDefinition definition in this.Fields)
            {
                DatabaseFieldDefinition definition2 = tableDefinition.Fields[definition.FieldName];
                if (definition2 == null)
                {
                    return false;
                }
                if ((((definition2.AllowNull != definition.AllowNull) || (definition2.AutoNumber != definition.AutoNumber)) || ((definition2.FieldName != definition.FieldName) || (definition2.FieldSize != definition.FieldSize))) || ((definition2.FieldType != definition.FieldType) || (definition2.PrimaryKey != definition.PrimaryKey)))
                {
                    return false;
                }
            }
            if (this.Indexes.Count != tableDefinition.Indexes.Count)
            {
                return false;
            }
            foreach (IndexDefinition definition3 in this.Indexes)
            {
                IndexDefinition definition4 = tableDefinition.Indexes[definition3.Name];
                if (definition4 == null)
                {
                    return false;
                }
                if (definition4.Clustered != definition3.Clustered)
                {
                    return false;
                }
                if (definition4.Unique != definition3.Unique)
                {
                    return false;
                }
                if (definition4.Fields.Count != definition3.Fields.Count)
                {
                    return false;
                }
                foreach (IndexFieldDefinition definition5 in definition3.Fields)
                {
                    IndexFieldDefinition definition6 = definition4.Fields[definition5.Name];
                    if (definition5.SortOrder != definition6.SortOrder)
                    {
                        return false;
                    }
                }
            }
            return (((dataTableTarget != DataTableTarget.VersionHistory) || (tableDefinition.VersionHistoryTable == null)) && ((dataTableTarget != DataTableTarget.Main) || !(tableDefinition.VersionHistoryTable != this.VersionHistoryTable)));
        }
    }
}

