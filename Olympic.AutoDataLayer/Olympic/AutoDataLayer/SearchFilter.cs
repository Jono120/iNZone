namespace Olympic.AutoDataLayer
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Text.RegularExpressions;

    [Serializable]
    public class SearchFilter
    {
        private object[] _criteria;
        private string _fieldName;
        private SearchFilterRelationship _relationship;
        private static string DEFAULT_CONTAINS_OPERATOR = "OR";

        public SearchFilter()
        {
        }

        public SearchFilter(string fieldName, object criteriaValue) : this(fieldName, SearchFilterRelationship.Equal, new object[] { criteriaValue })
        {
        }

        public SearchFilter(string fieldName, SearchFilterRelationship relationship, params object[] criteriaValues)
        {
            if (fieldName == null)
            {
                throw new ArgumentNullException("fieldName", "fieldName must not be null.");
            }
            if (!this.IsValidSQLFieldName(fieldName))
            {
                throw new ArgumentException("fieldName is not a valid SQL field name.", "fieldName");
            }
            ArrayList list = new ArrayList();
            foreach (object obj2 in criteriaValues)
            {
                if (obj2 is Array)
                {
                    Array array = (Array) obj2;
                    foreach (object obj3 in array)
                    {
                        list.Add(obj3);
                    }
                }
                else
                {
                    list.Add(obj2);
                }
            }
            criteriaValues = list.ToArray();
            if (criteriaValues.Length == 0)
            {
                if ((((relationship != SearchFilterRelationship.Null) && (relationship != SearchFilterRelationship.NotNull)) && ((relationship != SearchFilterRelationship.True) && (relationship != SearchFilterRelationship.False))) && (((relationship != SearchFilterRelationship.Custom) && (relationship != SearchFilterRelationship.IsNumeric)) && (relationship != SearchFilterRelationship.IsNonNumeric)))
                {
                    throw new ArgumentOutOfRangeException("relationship", "Only relationships Null, NotNull, True, False, Customer, IsNumeric and IsNotNumeric are appropriate if not specifying a criteriaValue.");
                }
            }
            else if (criteriaValues.Length > 1)
            {
                if (relationship != SearchFilterRelationship.Between)
                {
                    if (((relationship != SearchFilterRelationship.In) && (relationship != SearchFilterRelationship.NotIn)) && (relationship != SearchFilterRelationship.Custom))
                    {
                        throw new ArgumentOutOfRangeException("relationship", "Only SearchFilterRelationship.Between, SearchFilterRelationship.In and SearchFilterRelationship.Custom are appropriate if specifying more than one criteria value.");
                    }
                }
                else if (criteriaValues.Length != 2)
                {
                    throw new ArgumentOutOfRangeException("relationship", "Incorrect number of criteriaValues specified for SearchFilterRelationship.Between relationship.");
                }
            }
            else
            {
                if (relationship == SearchFilterRelationship.Between)
                {
                    throw new ArgumentOutOfRangeException("relationship", "SearchFilterRelationship.Between is not appropriate if only specifying 1 criteriaValue.");
                }
                if ((((relationship == SearchFilterRelationship.Null) || (relationship == SearchFilterRelationship.NotNull)) || ((relationship == SearchFilterRelationship.True) || (relationship == SearchFilterRelationship.False))) || ((relationship == SearchFilterRelationship.IsNumeric) || (relationship == SearchFilterRelationship.IsNonNumeric)))
                {
                    throw new ArgumentOutOfRangeException("relationship", "SearchFilterRelationship." + relationship.ToString() + " is not appropriate if specifying a criteriaValue.");
                }
            }
            this._fieldName = fieldName;
            this._relationship = relationship;
            this._criteria = criteriaValues;
        }

        private string FormatContainsQuery(object criteriaValue)
        {
            string input = criteriaValue.ToString();
            MatchCollection matchs = new Regex("(\"[^\"]+\"|and not|and|or|&)").Matches(input);
            for (int i = matchs.Count - 1; i > -1; i--)
            {
                Match match = matchs[i];
                string str2 = match.Value.ToLower();
                if (str2 != null)
                {
                    if (!(str2 == "and") && !(str2 == "&"))
                    {
                        if (str2 == "and not")
                        {
                            goto Label_00A9;
                        }
                        if (str2 == "or")
                        {
                            goto Label_00D3;
                        }
                    }
                    else
                    {
                        input = input.Remove(match.Index, match.Length).Insert(match.Index, "+");
                    }
                }
                continue;
            Label_00A9:
                input = input.Remove(match.Index, match.Length).Insert(match.Index, "-");
                continue;
            Label_00D3:
                input = input.Remove(match.Index, match.Length).Insert(match.Index, "|");
            }
            input = this.FormatContainsSearchCondition(input);
            return string.Format("'{0}'", input.Replace("'", ""));
        }

        private string FormatContainsSearchCondition(string sqlCONTAINSSearchCondition)
        {
            MatchCollection matchs = new Regex("(\\+|-|\\||\"[^\"]+\"|\\([^\\)]+\\)|[\\w]+)").Matches(sqlCONTAINSSearchCondition);
            string str = "";
            bool flag = true;
            for (int i = 0; i < matchs.Count; i++)
            {
                string str2 = matchs[i].Value;
                if (flag)
                {
                    if (str2.StartsWith("("))
                    {
                        str = str + "(" + this.FormatContainsSearchCondition(str2.Substring(1, str2.Length - 2)) + ") ";
                    }
                    else
                    {
                        str = str + str2 + " ";
                    }
                    flag = false;
                    continue;
                }
                string str3 = str2;
                if (str3 == null)
                {
                    goto Label_00DC;
                }
                if (!(str3 == "+"))
                {
                    if (str3 == "|")
                    {
                        goto Label_00C0;
                    }
                    if (str3 == "-")
                    {
                        goto Label_00CE;
                    }
                    goto Label_00DC;
                }
                str = str + "AND ";
                goto Label_00F3;
            Label_00C0:
                str = str + "OR ";
                goto Label_00F3;
            Label_00CE:
                str = str + "AND NOT ";
                goto Label_00F3;
            Label_00DC:
                str = str + DEFAULT_CONTAINS_OPERATOR + " ";
                i--;
            Label_00F3:
                flag = true;
            }
            return str.TrimEnd(new char[0]);
        }

        public string GetFormattedCriteria()
        {
            if (this._relationship == SearchFilterRelationship.Contains)
            {
                return this.FormatContainsQuery(this._criteria[0]);
            }
            return "";
        }

        private bool IsValidSQLFieldName(string fieldName)
        {
            return true;
        }

        private string MakeSafeSQLString(object criteriaValue)
        {
            string str = criteriaValue.ToString();
            if (criteriaValue is DateTime)
            {
                DateTime time = (DateTime) criteriaValue;
                return string.Format("'{0}'", time.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            }
            if (!(criteriaValue is bool))
            {
                return string.Format("'{0}'", str.Replace("'", "''"));
            }
            if (!((bool) criteriaValue))
            {
                return "0";
            }
            return "1";
        }

        public virtual string ToSQLWhereClause()
        {
            SearchFilterRelationship @null = this._relationship;
            if ((this._relationship == SearchFilterRelationship.Equal) && (this.Criteria[0] == null))
            {
                @null = SearchFilterRelationship.Null;
            }
            if ((this._relationship == SearchFilterRelationship.NotEqual) && (this.Criteria[0] == null))
            {
                @null = SearchFilterRelationship.NotNull;
            }
            switch (@null)
            {
                case SearchFilterRelationship.Null:
                    return string.Format("{0} IS NULL", this._fieldName);

                case SearchFilterRelationship.NotNull:
                    return string.Format("{0} IS NOT NULL", this._fieldName);

                case SearchFilterRelationship.True:
                    return string.Format("{0} = 1", this._fieldName);

                case SearchFilterRelationship.False:
                    return string.Format("{0} = 0", this._fieldName);

                case SearchFilterRelationship.Like:
                    return string.Format("{0} LIKE {1}", this._fieldName, this.MakeSafeSQLString(this._criteria[0]));

                case SearchFilterRelationship.NotLike:
                    return string.Format("{0} NOT LIKE {1}", this._fieldName, this.MakeSafeSQLString(this._criteria[0]));

                case SearchFilterRelationship.Contains:
                    return string.Format("CONTAINS({0}, {1})", this._fieldName, this.FormatContainsQuery(this._criteria[0]));

                case SearchFilterRelationship.Equal:
                    return string.Format("{0} = {1}", this._fieldName, this.MakeSafeSQLString(this._criteria[0]));

                case SearchFilterRelationship.NotEqual:
                    return string.Format("{0} <> {1}", this._fieldName, this.MakeSafeSQLString(this._criteria[0]));

                case SearchFilterRelationship.GreaterThan:
                    return string.Format("{0} > {1}", this._fieldName, this.MakeSafeSQLString(this._criteria[0]));

                case SearchFilterRelationship.GreaterEqual:
                    return string.Format("{0} >= {1}", this._fieldName, this.MakeSafeSQLString(this._criteria[0]));

                case SearchFilterRelationship.LessThan:
                    return string.Format("{0} < {1}", this._fieldName, this.MakeSafeSQLString(this._criteria[0]));

                case SearchFilterRelationship.LessEqual:
                    return string.Format("{0} <= {1}", this._fieldName, this.MakeSafeSQLString(this._criteria[0]));

                case SearchFilterRelationship.Between:
                    return string.Format("{0} BETWEEN {1} AND {2}", this._fieldName, this.MakeSafeSQLString(this._criteria[0]), this.MakeSafeSQLString(this._criteria[1]));

                case SearchFilterRelationship.In:
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (object obj2 in this._criteria)
                    {
                        builder.Append(this.MakeSafeSQLString(obj2) + ",");
                    }
                    builder.Remove(builder.Length - 1, 1);
                    return string.Format("{0} IN ({1})", this._fieldName, builder);
                }
                case SearchFilterRelationship.NotIn:
                {
                    StringBuilder builder2 = new StringBuilder();
                    foreach (object obj3 in this._criteria)
                    {
                        builder2.Append(this.MakeSafeSQLString(obj3) + ",");
                    }
                    builder2.Remove(builder2.Length - 1, 1);
                    return string.Format("{0} NOT IN ({1})", this._fieldName, builder2);
                }
                case SearchFilterRelationship.Custom:
                {
                    string[] strArray = new string[this._criteria.Length];
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        strArray[i] = this.MakeSafeSQLString(this._criteria[i]);
                    }
                    return string.Format(this._fieldName, (object[]) strArray);
                }
                case SearchFilterRelationship.IsNumeric:
                    return string.Format("ISNUMERIC({0}) = 1", this._fieldName);

                case SearchFilterRelationship.IsNonNumeric:
                    return string.Format("ISNUMERIC({0}) = 0", this._fieldName);
            }
            throw new ArgumentOutOfRangeException("Undefined relationship for SearchFilter.");
        }

        public object[] Criteria
        {
            get
            {
                return this._criteria;
            }
            set
            {
                this._criteria = value;
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

        public SearchFilterRelationship Relationship
        {
            get
            {
                return this._relationship;
            }
            set
            {
                this._relationship = value;
            }
        }
    }
}

