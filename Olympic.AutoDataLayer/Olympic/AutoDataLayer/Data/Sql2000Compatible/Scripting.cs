namespace Olympic.AutoDataLayer.Data.Sql2000Compatible
{
    using Olympic.AutoDataLayer.BuildSupport;
    using Olympic.AutoDataLayer.ClassSupport;
    using Olympic.AutoDataLayer.Tools;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class Scripting
    {
        public static string GetCreateIndexScript(string tableName, IndexDefinition indexDefinition)
        {
            string text;
            if (indexDefinition.Unique)
            {
                text = ResourceAccessor.GetText("CreateUniqueContraint.sql");
            }
            else
            {
                text = ResourceAccessor.GetText("CreateIndex.sql");
            }
            ArrayList list = new ArrayList();
            foreach (IndexFieldDefinition definition in indexDefinition.Fields)
            {
                list.Add(definition.ToIndexFieldListString());
            }
            string newValue = string.Join(",", (string[]) list.ToArray(typeof(string)));
            text = text.Replace("{IndexFieldList}", newValue).Replace("{TableName}", tableName).Replace("{IndexName}", indexDefinition.Name);
            string str3 = indexDefinition.Clustered ? "CLUSTERED" : "";
            return text.Replace("{Clustered}", str3);
        }

        public static string GetCreateTableScript(TableDefinition tableDefinition)
        {
            return GetCreateTableScript(tableDefinition, null);
        }

        public static string GetCreateTableScript(TableDefinition tableDefinition, TableDefinition tableToPreserve)
        {
            bool flag = tableToPreserve != null;
            bool flag2 = false;
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            ArrayList list3 = new ArrayList();
            foreach (DatabaseFieldDefinition definition in tableDefinition.Fields)
            {
                list.Add(definition.ToTableDeclarationScript(true));
                if (definition.PrimaryKey)
                {
                    list2.Add(definition.ToQuotedSqlObjectString());
                }
                if (flag && tableToPreserve.Fields.Contains(definition.FieldName))
                {
                    list3.Add(definition.ToQuotedSqlObjectString());
                    if (definition.AutoNumber)
                    {
                        flag2 = true;
                    }
                }
            }
            flag = list3.Count > 0;
            string newValue = tableDefinition.Indexes.ContainsClusteredIndex() ? "NONCLUSTERED" : "CLUSTERED";
            string resourceName = "";
            if (list2.Count > 0)
            {
                resourceName = flag ? "PreserveTableWithPrimaryKey.sql" : "CreateTableWithPrimaryKey.sql";
            }
            else
            {
                resourceName = flag ? "PreserveTableWithoutPrimaryKey.sql" : "CreateTableWithoutPrimaryKey.sql";
            }
            int num = flag2 ? 1 : 0;
            string str3 = ResourceAccessor.GetText(resourceName).Replace("{TableName}", tableDefinition.TableName).Replace("{TableFieldDeclarations}", string.Join(",\r\n", (string[]) list.ToArray(typeof(string)))).Replace("{TableKeyFieldList}", string.Join(",", (string[]) list2.ToArray(typeof(string)))).Replace("{PreserveFieldList}", string.Join(",", (string[]) list3.ToArray(typeof(string)))).Replace("{IdentityInsert}", num.ToString()).Replace("{Clustered}", newValue);
            foreach (IndexDefinition definition2 in tableDefinition.Indexes)
            {
                str3 = str3 + "\r\n\r\n" + GetCreateIndexScript(tableDefinition.TableName, definition2);
            }
            return str3;
        }

        public static string GetCreateVersionHistoryTriggerScript(TableDefinition tableDefinition)
        {
            string text = ResourceAccessor.GetText("CreateVersionHistoryTrigger.sql");
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            foreach (DatabaseFieldDefinition definition in tableDefinition.Fields)
            {
                list.Add("[" + definition.FieldName + "]");
                list2.Add("d.[" + definition.FieldName + "]");
                if (definition.PrimaryKey)
                {
                    list3.Add("i.[" + definition.FieldName + "] = d.[" + definition.FieldName + "]");
                }
            }
            text = text.Replace("{TableName}", tableDefinition.TableName);
            string newValue = "VersionHistory_" + tableDefinition.VersionHistoryTable;
            return text.Replace("{TriggerName}", newValue).Replace("{VersionHistoryTableName}", tableDefinition.VersionHistoryTable).Replace("{FieldList}", string.Join(",", list.ToArray())).Replace("{DeletedFieldList}", string.Join(",", list2.ToArray())).Replace("{PrimaryKeyInnerJoin}", string.Join(" AND ", list3.ToArray()));
        }

        public static string GetDropTableScript(string tableName)
        {
            return ResourceAccessor.GetText("DropTable.sql").Replace("{TableName}", tableName);
        }

        public static string GetListTableColumnsScript(string tableName)
        {
            return ResourceAccessor.GetText("ListTableColumns.sql").Replace("{TableName}", tableName);
        }

        public static string GetObjectDeleteByKeyScript(ClassDefinition classDefinition)
        {
            if (classDefinition.DataSourceType == DataSourceType.Query)
            {
                return null;
            }
            string str = "[" + classDefinition.TableName + "]";
            string primaryKeyFilterScript = GetPrimaryKeyFilterScript(classDefinition.PrimaryKeyFields);
            return string.Format("\r\nDELETE\r\n\t{0}\r\nWHERE\r\n\t{1}\r\n", str, primaryKeyFilterScript);
        }

        public static string GetObjectGetScript(ClassDefinition classDefinition, VersionTarget versionTarget)
        {
            string templateString = "[{DatabaseFieldName}]";
            string str2 = classDefinition.ClassFields.ToString(templateString, ",", true, false);
            string str3 = null;
            string str4 = null;
            if (classDefinition.DataSourceType == DataSourceType.Table)
            {
                str3 = "[" + classDefinition.TableName + "]";
                if (versionTarget != VersionTarget.None)
                {
                    str4 = "[" + classDefinition.VersionHistoryTableName + "]";
                }
            }
            else if (classDefinition.DataSourceType == DataSourceType.Query)
            {
                str3 = "\r\n(\r\n" + classDefinition.Query + "\r\n) s";
            }
            string primaryKeyFilterScript = GetPrimaryKeyFilterScript(classDefinition.PrimaryKeyFields);
            string str6 = "";
            string str7 = "";
            if (versionTarget == VersionTarget.Number)
            {
                primaryKeyFilterScript = primaryKeyFilterScript + " AND [_version] = @Version";
            }
            else if (versionTarget == VersionTarget.Date)
            {
                primaryKeyFilterScript = primaryKeyFilterScript + " AND [_modifiedDate] <= @Version";
                str6 = "TOP 1";
                str7 = "ORDER BY [_version] DESC";
            }
            string str8 = string.Format("\r\nSELECT {3}\r\n\t{0}\r\nFROM\r\n\t{1}\r\nWHERE\r\n\t{2}\r\n{4}\r\n", new object[] { str2, str3, primaryKeyFilterScript, str6, str7 });
            if (str4 != null)
            {
                str8 = str8 + string.Format("\r\nIF @@ROWCOUNT = 0\r\n    SELECT {3}\r\n\t    {0}\r\n    FROM\r\n\t    {1}\r\n    WHERE\r\n\t    {2}\r\n    {4}\r\n", new object[] { str2, str4, primaryKeyFilterScript, str6, str7 });
            }
            return str8;
        }

        public static string GetObjectInsertScript(ClassDefinition classDefinition)
        {
            if (classDefinition.DataSourceType == DataSourceType.Query)
            {
                return null;
            }
            string templateString = "[{DatabaseFieldName}]";
            string str2 = "@Field_{Index}";
            string str3 = classDefinition.ClassFields.ToString(templateString, ",", false, false);
            string str4 = classDefinition.ClassFields.ToString(str2, ",", false, false, "1", "GETDATE()");
            string str5 = "[{DatabaseFieldName}]";
            string str6 = classDefinition.ClassFields.ToString(str5, ",", true, false);
            string str7 = "[" + classDefinition.TableName + "]";
            ClassFieldDefinition definition = null;
            foreach (ClassFieldDefinition definition2 in classDefinition.PrimaryKeyFields)
            {
                if (definition2.AutoNumber)
                {
                    definition = definition2;
                    break;
                }
            }
            string str8 = "";
            if (classDefinition.PrimaryKeyFields.Count > 0)
            {
                if (definition != null)
                {
                    str8 = "[" + definition.DatabaseFieldName + "] = @@IDENTITY";
                }
                else
                {
                    string str9 = "[{DatabaseFieldName}] = @Field_{Index}";
                    str8 = classDefinition.ClassFields.ToString(str9, " AND ", true, true);
                }
            }
            string str10 = string.Format("\r\nINSERT INTO  \r\n\t{0} \r\n( \r\n\t{1}\r\n) VALUES (\r\n\t{2}\r\n)\r\n", str7, str3, str4);
            string str11 = "";
            if (classDefinition.PrimaryKeyFields.Count > 0)
            {
                str11 = string.Format("\r\nSELECT\r\n\t{0}\r\nFROM\r\n\t{1}\r\nWHERE\r\n\t{2}\r\n", str6, str7, str8);
            }
            return (str10 + str11);
        }

        public static string GetObjectListScript(ClassDefinition classDefinition)
        {
            string str = ((classDefinition.DataSourceType == DataSourceType.Table) && (classDefinition.PrimaryKeyFields.Count > 0)) ? ResourceAccessor.GetText("ListKeyedObjects.sql") : ResourceAccessor.GetText("ListObjects.sql");
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            foreach (DatabaseFieldDefinition definition in classDefinition.TableDefinition.Fields)
            {
                list.Add(definition.ToTableDeclarationScript(false));
                if (definition.PrimaryKey)
                {
                    list2.Add(definition.ToTableDeclarationScript(false));
                }
            }
            foreach (DatabaseFieldDefinition definition2 in classDefinition.TableDefinition.AggregateFields)
            {
                list.Add(definition2.ToTableDeclarationScript(false));
            }
            string newValue = null;
            if (classDefinition.DataSourceType == DataSourceType.Table)
            {
                newValue = "[" + classDefinition.TableName + "]";
            }
            else if (classDefinition.DataSourceType == DataSourceType.Query)
            {
                newValue = "\r\n(\r\n" + classDefinition.Query + "\r\n) s";
            }
            str = str.Replace("{TableName}", newValue).Replace("{TableFieldDeclarations}", string.Join(",\r\n", (string[]) list.ToArray(typeof(string)))).Replace("{PrimaryKeyFieldDeclarations}", string.Join(",\r\n", (string[]) list2.ToArray(typeof(string))));
            string templateString = "[{DatabaseFieldName}]";
            string str4 = "t.[{DatabaseFieldName}]";
            string str5 = classDefinition.ClassFields.ToString(templateString, ",", true, false);
            string str6 = classDefinition.ClassFields.ToString(str4, ",", true, false);
            string str7 = classDefinition.PrimaryKeyFields.ToString(templateString, ",", true, false);
            string str8 = str5;
            string str9 = str5;
            string str10 = (classDefinition.TableDefinition.AggregateFields.Count > 0) ? ("GROUP BY " + str5) : "";
            string str11 = "t.[{DatabaseFieldName}] = r.[{DatabaseFieldName}]";
            string str12 = classDefinition.PrimaryKeyFields.ToString(str11, " AND ", true, false);
            foreach (DatabaseFieldDefinition definition3 in classDefinition.TableDefinition.AggregateFields)
            {
                if (str8.Length > 0)
                {
                    str8 = str8 + ",";
                    str6 = str6 + ",";
                    str9 = str9 + ",";
                }
                str8 = str8 + definition3.AggregateFunction;
                str6 = str6 + "[" + definition3.FieldName + "]";
                str9 = str9 + "[" + definition3.FieldName + "]";
            }
            return str.Replace("{SelectFieldList}", str8).Replace("{TableFieldList}", str6).Replace("{ResultTableFieldList}", str9).Replace("{GroupByList}", str10).Replace("{PrimaryKeyFieldList}", str7).Replace("{PrimaryKeyInnerJoinList}", str12);
        }

        public static string GetObjectUpdateScript(ClassDefinition classDefinition)
        {
            if (classDefinition.DataSourceType == DataSourceType.Query)
            {
                return null;
            }
            string templateString = "[{DatabaseFieldName}] = @Field_{Index}";
            string str2 = classDefinition.ClassFields.ToString(templateString, ",", false, false, "[_version] = ISNULL([_version], 0) + 1", "[_modifiedDate] = GETDATE()");
            string str3 = "[{DatabaseFieldName}]";
            string str4 = classDefinition.ClassFields.ToString(str3, ",", true, false);
            string str5 = "[" + classDefinition.TableName + "]";
            string primaryKeyFilterScript = GetPrimaryKeyFilterScript(classDefinition.PrimaryKeyFields);
            string str7 = "[{DatabaseFieldName}] = @Field_{Index}";
            string str8 = classDefinition.ClassFields.ToString(str7, " AND ", true, true);
            string str9 = "";
            if (classDefinition.VersioningEnabled && classDefinition.CheckForStaleData)
            {
                int num = classDefinition.ClassFields.Count - 2;
                str9 = string.Format("AND [_version] = @Field_{0}\r\n\r\n-- Check for stale data\r\nIF (@@ROWCOUNT = 0)\r\n\tRAISERROR ('<StaleData>', 16, 1) WITH NOWAIT\r\n\r\n", num);
            }
            return string.Format("\r\nUPDATE \r\n\t{0} \r\nSET \r\n\t{1}\r\nWHERE\r\n\t{2}\r\n    {5}\r\n\r\nSELECT\r\n\t{3}\r\nFROM\r\n\t{0}\r\nWHERE\r\n\t{4}\r\n", new object[] { str5, str2, primaryKeyFilterScript, str4, str8, str9 });
        }

        public static string GetPrimaryKeyFilterScript(ClassFieldDefinitionCollection primaryKeys)
        {
            string templateString = "[{DatabaseFieldName}] = @PrimaryKey_{Index}";
            return primaryKeys.ToString(templateString, " AND ", true, false);
        }

        public enum VersionTarget
        {
            None,
            Number,
            Date
        }
    }
}

