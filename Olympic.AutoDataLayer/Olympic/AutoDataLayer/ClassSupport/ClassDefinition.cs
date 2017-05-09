namespace Olympic.AutoDataLayer.ClassSupport
{
    using Olympic.AutoDataLayer;
    using Olympic.AutoDataLayer.BuildSupport;
    using Olympic.AutoDataLayer.Data;
    using Olympic.AutoDataLayer.Data.Sql2000Compatible;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class ClassDefinition
    {
        private static Hashtable _classDefinitions = new Hashtable();
        private string _deleteObjectByKeyScript;
        private string _getObjectScript;
        private string _getVersionDateObjectScript;
        private string _getVersionNumberObjectScript;
        private string _insertObjectScript;
        private string _listObjectScript;
        private FieldInfo _loadedKeyCollectionField;
        private static Hashtable _typeValidated = new Hashtable();
        private string _updateObjectScript;
        private FieldInfo _versionField;
        public bool AutoBuild;
        public bool AutoResolveCurrentUser;
        public bool CheckForStaleData;
        public ClassFieldDefinitionCollection ClassFields;
        public Olympic.AutoDataLayer.ClassSupport.DataSourceType DataSourceType;
        public bool KeepVersionHistory;
        public bool PreserveData;
        public ClassFieldDefinitionCollection PrimaryKeyFields;
        public string Query;
        public Olympic.AutoDataLayer.BuildSupport.TableDefinition TableDefinition;
        public string TableName;
        public System.Type Type;
        public Olympic.AutoDataLayer.BuildSupport.TableDefinition VersionHistoryTableDefinition;
        public string VersionHistoryTableName;
        public bool VersioningEnabled;

        public static  event AfterCreateTableEventHandler AfterCreateTable;

        public static  event BeforeCreateTableEventHandler BeforeCreateTable;

        public ClassDefinition()
        {
        }

        private ClassDefinition(System.Type objectType, DataSourceAttribute dataSource)
        {
            this.Type = objectType;
            ClassAttributeInfo info = new ClassAttributeInfo(objectType, dataSource);
            this.DataSourceType = info.DataSourceType;
            this.TableName = info.TableName;
            this.Query = info.Query;
            this.AutoBuild = info.AutoBuild;
            this.PreserveData = info.PreserveData;
            this.KeepVersionHistory = info.KeepVersionHistory;
            this.VersionHistoryTableName = info.VersionHistoryTableName;
            this.AutoResolveCurrentUser = info.AutoResolveCurrentUser;
            this.CheckForStaleData = info.CheckForStaleData;
            this.VersioningEnabled = info.VersioningEnabled;
            this.ClassFields = new ClassFieldDefinitionCollection();
            this.PrimaryKeyFields = new ClassFieldDefinitionCollection();
            foreach (MemberInfo info2 in objectType.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if ((info2.DeclaringType != typeof(AutoDataSupport)) && ((info2.MemberType == MemberTypes.Field) || (info2.MemberType == MemberTypes.Property)))
                {
                    string name = info2.Name;
                    ClassFieldDefinition fieldDefinition = ClassFieldDefinition.Get(info2);
                    if (fieldDefinition != null)
                    {
                        this.ClassFields.Add(fieldDefinition);
                        if (fieldDefinition.Unique)
                        {
                            this.PrimaryKeyFields.Add(fieldDefinition);
                        }
                    }
                }
            }
            if (this.VersioningEnabled)
            {
                if (this.PrimaryKeyFields.Count == 0)
                {
                    throw new Exception("In order to support versioning, your class must have a primary key(s).  Use the Unique attribute to indicate which properties/fields make up the primary key for your class.");
                }
                ClassFieldDefinition definition2 = ClassFieldDefinition.Get(this.Type.GetMember("_version", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0]);
                ClassFieldDefinition definition3 = ClassFieldDefinition.Get(this.Type.GetMember("_modifiedDate", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0]);
                ClassFieldDefinition definition4 = ClassFieldDefinition.Get(this.Type.GetMember("_modifiedBy", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0]);
                definition2.IsVersionNumberField = true;
                definition3.IsModifiedDateField = true;
                definition4.IsModifiedByField = true;
                this.ClassFields.Add(definition2);
                this.ClassFields.Add(definition3);
                this.ClassFields.Add(definition4);
            }
            if (this.IsAggregated)
            {
                this.AutoBuild = false;
            }
            this.TableDefinition = new Olympic.AutoDataLayer.BuildSupport.TableDefinition(this, DataTableTarget.Main);
            this.VersionHistoryTableDefinition = new Olympic.AutoDataLayer.BuildSupport.TableDefinition(this, DataTableTarget.VersionHistory);
            this._loadedKeyCollectionField = this.Type.GetField("_loadedKeys", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            this._versionField = this.Type.GetField("_version", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            this._getObjectScript = Scripting.GetObjectGetScript(this, Scripting.VersionTarget.None);
            this._getVersionNumberObjectScript = Scripting.GetObjectGetScript(this, Scripting.VersionTarget.Number);
            this._getVersionDateObjectScript = Scripting.GetObjectGetScript(this, Scripting.VersionTarget.Date);
            this._listObjectScript = Scripting.GetObjectListScript(this);
            this._updateObjectScript = Scripting.GetObjectUpdateScript(this);
            this._insertObjectScript = Scripting.GetObjectInsertScript(this);
            this._deleteObjectByKeyScript = Scripting.GetObjectDeleteByKeyScript(this);
            if (this.AutoBuild)
            {
                bool flag = this.Verify(DataTableTarget.Main);
                bool flag2 = !this.KeepVersionHistory || this.Verify(DataTableTarget.VersionHistory);
                if (!flag || !flag2)
                {
                    using (SqlTransactionScope scope = new SqlTransactionScope())
                    {
                        if (!flag2)
                        {
                            if (!this.PreserveData)
                            {
                                this.DropDatabaseTable(DataTableTarget.VersionHistory);
                            }
                            this.CreateDatabaseTable(DataTableTarget.VersionHistory);
                        }
                        if (!flag)
                        {
                            if (!this.PreserveData)
                            {
                                this.DropDatabaseTable(DataTableTarget.Main);
                            }
                            this.CreateDatabaseTable(DataTableTarget.Main);
                        }
                        scope.Complete();
                    }
                }
            }
        }

        public void CreateDatabaseTable(DataTableTarget dataTableTarget)
        {
            string connectionString = ConnectionStringManager.Get(this.Type);
            this.OnBeforeCreateTable();
            ((dataTableTarget == DataTableTarget.Main) ? this.TableDefinition : this.VersionHistoryTableDefinition).CreateTable(connectionString, this.PreserveData, new ClassContext(null, DatabaseAction.CreateTable, this.Type));
            this.OnAfterCreateTable();
        }

        public void DeleteObject(params object[] nameValuePairs)
        {
            if (this.IsAggregated)
            {
                throw new Exception("Can not 'Delete' objects that have aggregated fields defined.  Aggregated classes support 'List' only.");
            }
            if (this.PrimaryKeyFields.Count == 0)
            {
                throw new Exception("Can not use 'Delete(nameValuePairs)' on classes that do not have a primary key defined.  Use 'Delete(searchFilter)' instead.");
            }
            if ((nameValuePairs.Length % 2) != 0)
            {
                throw new Exception("An incorrect number of parameters was supplied for the Delete method.");
            }
            Hashtable hashtable = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            for (int i = 0; i < nameValuePairs.Length; i += 2)
            {
                string str = nameValuePairs[i].ToString();
                object obj2 = nameValuePairs[i + 1];
                hashtable[str] = obj2;
            }
            string statement = this._deleteObjectByKeyScript;
            string connectionString = ConnectionStringManager.Get(this.Type);
            ArrayList list = new ArrayList();
            Hashtable hashtable2 = new Hashtable();
            int num2 = 1;
            foreach (ClassFieldDefinition definition in this.PrimaryKeyFields)
            {
                if (!hashtable.ContainsKey(definition.Name))
                {
                    throw new Exception("Missing unique field: " + definition.Name + " in the name/value pairs.");
                }
                object obj3 = hashtable[definition.Name];
                hashtable2["PrimaryKey_" + num2.ToString()] = obj3;
                list.Add("@PrimaryKey_" + num2++.ToString() + " " + this.TableDefinition.Fields[definition.DatabaseFieldName].ToTypeDeclarationScript());
            }
            string parameters = string.Join(",", (string[]) list.ToArray(typeof(string)));
            SpExecuteSqlCommand command = new SpExecuteSqlCommand(connectionString, statement, parameters, new ClassContext(null, DatabaseAction.Delete, this.Type));
            foreach (DictionaryEntry entry in hashtable2)
            {
                command.Parameters[entry.Key] = entry.Value;
            }
            command.ExecuteNonQuery();
        }

        public void DeleteObject(SearchFilterCollection filters)
        {
            if (this.IsAggregated)
            {
                throw new Exception("Can not 'Delete' objects that have aggregated fields defined.  Aggregated classes support 'List' only.");
            }
            string connectionString = ConnectionStringManager.Get(this.Type);
            string str2 = this.TranslateSearchFilterCollection(filters).ToSQLWhereClause();
            if (str2.Length > 0)
            {
                str2 = "WHERE " + str2;
            }
            string statement = string.Format("\r\nDELETE [{0}]\r\n{1}", this.TableName, str2);
            new SpExecuteSqlCommand(connectionString, statement, "", new ClassContext(null, DatabaseAction.Delete, this.Type)).ExecuteNonQuery();
        }

        public void DropDatabaseTable(DataTableTarget dataTableTarget)
        {
            string connectionString = ConnectionStringManager.Get(this.Type);
            ((dataTableTarget == DataTableTarget.Main) ? this.TableDefinition : this.VersionHistoryTableDefinition).DropTable(connectionString, new ClassContext(null, DatabaseAction.DropTable, this.Type));
        }

        private SearchFilterCollection ExtractHavingFilters(ref SearchFilterCollection searchFilters)
        {
            if (searchFilters == null)
            {
                searchFilters = new SearchFilterCollection();
                return new SearchFilterCollection();
            }
            SearchFilterCollection filters = new SearchFilterCollection();
            SearchFilterCollection filters2 = new SearchFilterCollection();
            if (!this.IsAggregated)
            {
                return new SearchFilterCollection();
            }
            if (searchFilters.LogicalOperator == LogicalOperator.Or)
            {
                filters = this.TranslateSearchFilterCollection(searchFilters);
                searchFilters = new SearchFilterCollection();
                return filters;
            }
            foreach (SearchFilter filter in searchFilters.SearchFilters)
            {
                if (filter is SearchFilterCollection)
                {
                    SearchFilter[] allAncesters = ((SearchFilterCollection) filter).GetAllAncesters();
                    bool flag = false;
                    foreach (SearchFilter filter2 in allAncesters)
                    {
                        if (this.IsAggregateFilter(filter2))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        filters.Add(filter);
                    }
                    else
                    {
                        filters2.Add(filter);
                    }
                }
                else if (this.IsAggregateFilter(filter))
                {
                    filters.Add(filter);
                }
                else
                {
                    filters2.Add(filter);
                }
            }
            searchFilters = this.TranslateSearchFilterCollection(filters2);
            return this.TranslateSearchFilterCollection(filters);
        }

        public static ClassDefinition GetClassDefinition(System.Type objectType)
        {
            string fullName = objectType.FullName;
            if (!_typeValidated.ContainsKey(fullName))
            {
                ValidateObjectType(objectType);
                _typeValidated[fullName] = true;
            }
            DataSourceAttribute dataSource = DataSourceManager.Get();
            string str2 = objectType.FullName + "+" + ConnectionStringManager.Get(objectType) + ((dataSource == null) ? "" : ("+" + dataSource.DatasourceValidationKey));
            ClassDefinition definition = (ClassDefinition) _classDefinitions[str2];
            if (definition == null)
            {
                definition = new ClassDefinition(objectType, dataSource);
                _classDefinitions[str2] = definition;
            }
            return definition;
        }

        public object GetObject(int? versionNumber, DateTime? versionDate, params object[] nameValuePairs)
        {
            string str2;
            if ((versionNumber.HasValue || versionDate.HasValue) && (!this.VersioningEnabled || !this.KeepVersionHistory))
            {
                throw new Exception(string.Format("Version History is not enabled for object of type: {0}.  To enable versioning, add the VersionAttribute to the class and set the property KeepVersionHistory to true.", this.Type));
            }
            if (this.IsAggregated)
            {
                throw new Exception("Can not 'Get' objects that have aggregated fields defined.  Use 'List' instead.");
            }
            if (this.PrimaryKeyFields.Count == 0)
            {
                throw new Exception("Can not use 'Get' on classes that do not have a primary key defined.  Use 'List' instead.");
            }
            if ((nameValuePairs.Length % 2) != 0)
            {
                throw new Exception("An incorrect number of parameters was supplied for the Get method.");
            }
            Hashtable hashtable = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            for (int i = 0; i < nameValuePairs.Length; i += 2)
            {
                string str = nameValuePairs[i].ToString();
                object obj2 = nameValuePairs[i + 1];
                hashtable[str] = obj2;
            }
            if (versionNumber.HasValue)
            {
                str2 = this._getVersionNumberObjectScript;
            }
            else if (versionDate.HasValue)
            {
                str2 = this._getVersionDateObjectScript;
            }
            else
            {
                str2 = this._getObjectScript;
            }
            string connectionString = ConnectionStringManager.Get(this.Type);
            ArrayList list = new ArrayList();
            Hashtable hashtable2 = new Hashtable();
            int num2 = 1;
            foreach (ClassFieldDefinition definition in this.PrimaryKeyFields)
            {
                if (!hashtable.ContainsKey(definition.Name))
                {
                    throw new Exception("Missing unique field: " + definition.Name + " in the name/value pairs.");
                }
                object obj3 = hashtable[definition.Name];
                hashtable2["PrimaryKey_" + num2.ToString()] = obj3;
                list.Add("@PrimaryKey_" + num2++.ToString() + " " + this.TableDefinition.Fields[definition.DatabaseFieldName].ToTypeDeclarationScript());
            }
            if (versionNumber.HasValue)
            {
                hashtable2["Version"] = versionNumber.Value;
                list.Add("@Version INT");
            }
            else if (versionDate.HasValue)
            {
                hashtable2["Version"] = versionDate.Value;
                list.Add("@Version DATETIME");
            }
            string parameters = string.Join(",", (string[]) list.ToArray(typeof(string)));
            SpExecuteSqlCommand command = new SpExecuteSqlCommand(connectionString, str2, parameters, new ClassContext(null, DatabaseAction.Get, this.Type));
            foreach (DictionaryEntry entry in hashtable2)
            {
                command.Parameters[entry.Key] = entry.Value;
            }
            DataSet set = command.ExecuteDataSet();
            int num3 = set.Tables.Count - 1;
            if (set.Tables[num3].Rows.Count > 0)
            {
                return this.RehydrateObject(set.Tables[num3].Rows[0]);
            }
            return null;
        }

        public int GetObjectCount()
        {
            return this.GetObjectCount(null);
        }

        public int GetObjectCount(SearchFilterCollection filter)
        {
            if (this.IsAggregated)
            {
                throw new Exception("Can not 'GetCount' of objects that have aggregated fields defined.  Aggregated classes support 'List' only.");
            }
            string connectionString = ConnectionStringManager.Get(this.Type);
            string str2 = this.TranslateSearchFilterCollection(filter).ToSQLWhereClause();
            if (str2.Length > 0)
            {
                str2 = "WHERE " + str2;
            }
            string str3 = "[" + this.TableName + "]";
            if (this.DataSourceType == Olympic.AutoDataLayer.ClassSupport.DataSourceType.Query)
            {
                str3 = "\r\n(\r\n" + this.Query + "\r\n) s";
            }
            string statement = string.Format("\r\nSELECT COUNT(*) \r\nFROM {0}\r\n{1}", str3, str2);
            SpExecuteSqlCommand command = new SpExecuteSqlCommand(connectionString, statement, "", new ClassContext(null, DatabaseAction.Count, this.Type));
            return Convert.ToInt32(command.ExecuteScalar());
        }

        private bool IsAggregateFilter(SearchFilter filter)
        {
            return this.TableDefinition.AggregateFields.Contains(filter.FieldName);
        }

        public Array ListObjects(SearchFilterCollection filters, OrderByClauseCollection orderBy, SearchOptions searchOptions, int pageNumber, int pageSize, out int totalRecordCount)
        {
            string format = this._listObjectScript;
            string connectionString = ConnectionStringManager.Get(this.Type);
            SearchFilterCollection filters2 = this.ExtractHavingFilters(ref filters);
            string str3 = filters.ToSQLWhereClause();
            if (str3.Length > 0)
            {
                str3 = "WHERE " + str3;
            }
            string str4 = filters2.ToSQLWhereClause();
            if (str4.Length > 0)
            {
                str4 = "HAVING " + str4;
            }
            string str5 = this.TranslateOrderByClauseCollection(orderBy).ToSQLOrderByString();
            if (str5.Length > 0)
            {
                str5 = "ORDER BY " + str5;
            }
            string str6 = ((searchOptions & SearchOptions.Distinct) == SearchOptions.Distinct) ? "DISTINCT" : "";
            format = string.Format(format, new object[] { str3, str5, pageSize, pageNumber, str6, str4 });
            SqlDataReader dataReader = new SpExecuteSqlCommand(connectionString, format, "", new ClassContext(null, DatabaseAction.List, this.Type)).ExecuteReader();
            ArrayList list = new ArrayList();
            while (dataReader.Read())
            {
                list.Add(this.RehydrateObject(dataReader));
            }
            dataReader.NextResult();
            dataReader.Read();
            totalRecordCount = Convert.ToInt32(dataReader["Count"]);
            dataReader.Close();
            return list.ToArray(this.Type);
        }

        private void OnAfterCreateTable()
        {
            if (AfterCreateTable != null)
            {
                AfterCreateTable(this.Type);
            }
        }

        private void OnBeforeCreateTable()
        {
            if (BeforeCreateTable != null)
            {
                BeforeCreateTable(this.Type);
            }
        }

        public static DataSet QueryDataSet(string query)
        {
            SpExecuteSqlCommand command = new SpExecuteSqlCommand(ConnectionStringManager.Get(typeof(DataSet)), query, "", new ClassContext(null, DatabaseAction.Query, typeof(DataSet)));
            return command.ExecuteDataSet();
        }

        public Array QueryObjects(string query, out int totalRecordCount)
        {
            totalRecordCount = 0;
            SqlDataReader dataReader = new SpExecuteSqlCommand(ConnectionStringManager.Get(this.Type), query, "", new ClassContext(null, DatabaseAction.Query, this.Type)).ExecuteReader();
            ArrayList list = new ArrayList();
            while (dataReader.Read())
            {
                list.Add(this.RehydrateObject(dataReader));
            }
            if (dataReader.NextResult() && dataReader.Read())
            {
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    if (dataReader.GetName(i).ToLower() == "count")
                    {
                        totalRecordCount = Convert.ToInt32(dataReader[i]);
                    }
                }
            }
            dataReader.Close();
            return list.ToArray(this.Type);
        }

        public object RehydrateObject(DataRow dataRow)
        {
            object objectToRehydrate = Activator.CreateInstance(this.Type);
            this.RehydrateObject(objectToRehydrate, dataRow);
            return objectToRehydrate;
        }

        public object RehydrateObject(SqlDataReader dataReader)
        {
            object objectToRehydrate = Activator.CreateInstance(this.Type);
            this.RehydrateObject(objectToRehydrate, dataReader);
            return objectToRehydrate;
        }

        public void RehydrateObject(object objectToRehydrate, DataRow dataRow)
        {
            ArrayList list = new ArrayList();
            foreach (ClassFieldDefinition definition in this.ClassFields)
            {
                object obj2;
                if (dataRow.IsNull(definition.DatabaseFieldName) || definition.ReadOnly)
                {
                    continue;
                }
                if (definition.SystemType.IsEnum)
                {
                    obj2 = Enum.Parse(definition.SystemType, Convert.ToString(dataRow[definition.DatabaseFieldName]), true);
                }
                else
                {
                    obj2 = Convert.ChangeType(dataRow[definition.DatabaseFieldName], definition.SystemType);
                }
                definition.SetValue(objectToRehydrate, obj2);
                if (definition.Unique)
                {
                    list.Add(new DictionaryEntry(definition.Name, obj2));
                }
            }
            if (this._loadedKeyCollectionField != null)
            {
                this._loadedKeyCollectionField.SetValue(objectToRehydrate, (DictionaryEntry[]) list.ToArray(typeof(DictionaryEntry)));
            }
        }

        public void RehydrateObject(object objectToRehydrate, SqlDataReader dataReader)
        {
            ArrayList list = new ArrayList();
            foreach (ClassFieldDefinition definition in this.ClassFields)
            {
                object obj2;
                try
                {
                    obj2 = dataReader[definition.DatabaseFieldName];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new Exception(string.Format("Field not found.  Expected: {0}", definition.DatabaseFieldName));
                }
                if (((obj2 != null) && !(obj2 is DBNull)) && !definition.ReadOnly)
                {
                    if (definition.SystemType.IsEnum)
                    {
                        obj2 = Enum.Parse(definition.SystemType, Convert.ToString(obj2), true);
                    }
                    else
                    {
                        obj2 = Convert.ChangeType(obj2, definition.SystemType);
                    }
                    definition.SetValue(objectToRehydrate, obj2);
                    if (definition.Unique)
                    {
                        list.Add(new DictionaryEntry(definition.Name, obj2));
                    }
                }
            }
            this._loadedKeyCollectionField.SetValue(objectToRehydrate, (DictionaryEntry[]) list.ToArray(typeof(DictionaryEntry)));
        }

        public object SaveObject(object objectToSave)
        {
            string str;
            if (this.IsAggregated)
            {
                throw new Exception("Can not 'Save' objects that have aggregated fields defined.  Aggregated classes support 'List' only.");
            }

            ClassFieldDefinitionCollection.ClassFieldDefinitionCollectionEnumerator enumerator = this.ClassFields.GetEnumerator();

            while (enumerator.MoveNext())
            {
                enumerator.Current.Validate(objectToSave);
            }
            
            DictionaryEntry[] entryArray = (DictionaryEntry[]) this._loadedKeyCollectionField.GetValue(objectToSave);
            Hashtable hashtable = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            if (entryArray != null)
            {
                foreach (DictionaryEntry entry in entryArray)
                {
                    hashtable[entry.Key] = entry.Value;
                }
            }
            bool flag = (entryArray != null) && (entryArray.Length > 0);
            if (flag)
            {
                str = this._updateObjectScript;
            }
            else
            {
                str = this._insertObjectScript;
            }
            string connectionString = ConnectionStringManager.Get(this.Type);
            ArrayList list = new ArrayList();
            Hashtable hashtable2 = new Hashtable();
            if (flag)
            {
                int num = 1;
                foreach (ClassFieldDefinition definition2 in this.PrimaryKeyFields)
                {
                    if (!hashtable.ContainsKey(definition2.Name))
                    {
                        throw new Exception("Error updating object.  Missing unique field: " + definition2.Name + " in the loaded keys.");
                    }
                    object obj2 = hashtable[definition2.Name];
                    if (definition2.AutoNumber && (Convert.ToInt32(definition2.GetValue(objectToSave)) != Convert.ToInt32(obj2)))
                    {
                        throw new Exception("Update failed.  The value in the Auto Number field: " + definition2.Name + " has been modified.");
                    }
                    hashtable2["PrimaryKey_" + num.ToString()] = obj2;
                    list.Add("@PrimaryKey_" + num++.ToString() + " " + this.TableDefinition.Fields[definition2.DatabaseFieldName].ToTypeDeclarationScript());
                }
            }
            int num2 = 1;
            foreach (ClassFieldDefinition definition3 in this.ClassFields)
            {
                object preparedValue = definition3.GetPreparedValue(objectToSave, this.TableDefinition.Fields[definition3.DatabaseFieldName].FieldType);
                if (definition3.IsModifiedByField && this.AutoResolveCurrentUser)
                {
                    preparedValue = AutoDataSupport.OnResolveCurrentUser((AutoDataSupport) objectToSave);
                }
                hashtable2["Field_" + num2.ToString()] = preparedValue;
                list.Add("@Field_" + num2++.ToString() + " " + this.TableDefinition.Fields[definition3.DatabaseFieldName].ToTypeDeclarationScript());
            }
            string parameters = string.Join(",", (string[]) list.ToArray(typeof(string)));
            SpExecuteSqlCommand command = new SpExecuteSqlCommand(connectionString, str, parameters, new ClassContext(objectToSave, DatabaseAction.Save, this.Type));
            foreach (DictionaryEntry entry2 in hashtable2)
            {
                command.Parameters[entry2.Key] = entry2.Value;
            }
            if (this.PrimaryKeyFields.Count == 0)
            {
                command.ExecuteNonQuery();
                return objectToSave;
            }
            try
            {
                DataSet set = command.ExecuteDataSet();
                if (set.Tables[0].Rows.Count > 0)
                {
                    this.RehydrateObject(objectToSave, set.Tables[0].Rows[0]);
                }
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("<StaleData>"))
                {
                    throw new StaleDataException(this.Type, (int) this._versionField.GetValue(objectToSave));
                }
                throw;
            }
            return objectToSave;
        }

        private OrderByClauseCollection TranslateOrderByClauseCollection(OrderByClauseCollection orderBy)
        {
            if (orderBy == null)
            {
                return new OrderByClauseCollection();
            }
            OrderByClauseCollection clauses = new OrderByClauseCollection();
            foreach (OrderByClause clause in orderBy)
            {
                string fieldName = "";
                if (clause.FieldName.StartsWith("[") && clause.FieldName.EndsWith("]"))
                {
                    fieldName = clause.FieldName;
                }
                else
                {
                    ClassFieldDefinition definition = this.ClassFields[clause.FieldName];
                    if (definition == null)
                    {
                        throw new Exception("Invalid field: " + clause.FieldName + " supplied for order by clause.");
                    }
                    if (definition.IsAggregate)
                    {
                        DatabaseFieldDefinition definition2 = this.TableDefinition.AggregateFields[definition.DatabaseFieldName];
                        fieldName = definition2.AggregateFunction;
                    }
                    else
                    {
                        fieldName = "[" + definition.DatabaseFieldName + "]";
                    }
                }
                clauses.Add(new OrderByClause(fieldName, clause.OrderType));
            }
            return clauses;
        }

        private SearchFilterCollection TranslateSearchFilterCollection(SearchFilterCollection searchFilters)
        {
            if (searchFilters == null)
            {
                return new SearchFilterCollection();
            }
            SearchFilterCollection filters = new SearchFilterCollection();
            filters.LogicalOperator = searchFilters.LogicalOperator;
            for (int i = 0; i < searchFilters.Count; i++)
            {
                SearchFilter filter = searchFilters[i];
                if (filter is SearchFilterCollection)
                {
                    filters.Add(this.TranslateSearchFilterCollection((SearchFilterCollection) filter));
                }
                else
                {
                    string fieldName = "";
                    if ((filter.FieldName.StartsWith("[") && filter.FieldName.EndsWith("]")) || (filter.Relationship == SearchFilterRelationship.Custom))
                    {
                        fieldName = filter.FieldName;
                    }
                    else
                    {
                        ClassFieldDefinition definition = this.ClassFields[filter.FieldName];
                        if (definition == null)
                        {
                            throw new Exception("Invalid field: " + filter.FieldName + " supplied for search filter.");
                        }
                        if (definition.IsAggregate)
                        {
                            fieldName = this.TableDefinition.AggregateFields[definition.DatabaseFieldName].AggregateFunction;
                        }
                        else
                        {
                            fieldName = definition.DatabaseFieldName;
                        }
                    }
                    SearchFilter filter2 = new SearchFilter(fieldName, filter.Relationship, filter.Criteria);
                    filters.Add(filter2);
                }
            }
            return filters;
        }

        private static void ValidateObjectType(System.Type objectType)
        {
            if (!objectType.IsAbstract)
            {
                Activator.CreateInstance(objectType);
            }
        }

        public bool Verify(DataTableTarget dataTableTarget)
        {
            string connectionString = ConnectionStringManager.Get(this.Type);
            Olympic.AutoDataLayer.BuildSupport.TableDefinition definition = (dataTableTarget == DataTableTarget.Main) ? this.TableDefinition : this.VersionHistoryTableDefinition;
            string tableName = definition.TableName;
            Olympic.AutoDataLayer.BuildSupport.TableDefinition tableDefinition = Olympic.AutoDataLayer.BuildSupport.TableDefinition.LoadFromDatabase(connectionString, tableName, new ClassContext(null, DatabaseAction.GetTableDefinition, this.Type));
            return definition.Verify(tableDefinition, dataTableTarget);
        }

        public bool IsAggregated
        {
            get
            {
                ClassFieldDefinitionCollection.ClassFieldDefinitionCollectionEnumerator enumerator = this.ClassFields.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.IsAggregate)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}

