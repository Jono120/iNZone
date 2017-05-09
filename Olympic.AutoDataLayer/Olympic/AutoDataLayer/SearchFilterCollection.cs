namespace Olympic.AutoDataLayer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    public class SearchFilterCollection : SearchFilter
    {
        private ArrayList _searchFilters;
        public Olympic.AutoDataLayer.LogicalOperator LogicalOperator;

        public SearchFilterCollection()
        {
            this._searchFilters = new ArrayList();
        }

        public SearchFilterCollection(params SearchFilter[] filters)
        {
            this._searchFilters = new ArrayList();
            foreach (SearchFilter filter in filters)
            {
                this.Add(filter);
            }
        }

        public int Add(SearchFilter value)
        {
            this._searchFilters.Add(value);
            return this._searchFilters.Count;
        }

        public int Add(string fieldName, SearchFilterRelationship relationship)
        {
            this._searchFilters.Add(new SearchFilter(fieldName, relationship, new object[0]));
            return this._searchFilters.Count;
        }

        public int Add(string fieldName, object criteriaValue)
        {
            this._searchFilters.Add(new SearchFilter(fieldName, criteriaValue));
            return this._searchFilters.Count;
        }

        public int Add(string fieldName, SearchFilterRelationship relationship, params object[] criteriaValues)
        {
            this._searchFilters.Add(new SearchFilter(fieldName, relationship, criteriaValues));
            return this._searchFilters.Count;
        }

        public int Add(string fieldName, SearchFilterRelationship relationship, object criteriaValue)
        {
            this._searchFilters.Add(new SearchFilter(fieldName, relationship, new object[] { criteriaValue }));
            return this._searchFilters.Count;
        }

        public void AddRange(SearchFilter[] filters)
        {
            foreach (SearchFilter filter in filters)
            {
                this._searchFilters.Add(filter);
            }
        }

        public bool Contains(SearchFilter value)
        {
            bool flag = false;
            if (this._searchFilters != null)
            {
                foreach (SearchFilter filter in this._searchFilters)
                {
                    if (filter.GetType() == typeof(SearchFilterCollection))
                    {
                        SearchFilterCollection filters = new SearchFilterCollection();
                        flag = ((SearchFilterCollection) filter).Contains(value);
                    }
                    else if (filter == value)
                    {
                        return true;
                    }
                }
            }
            return flag;
        }

        public SearchFilter[] Contains(string FieldName)
        {
            ArrayList list = new ArrayList();
            if (this._searchFilters != null)
            {
                foreach (SearchFilter filter in this._searchFilters)
                {
                    if (filter.GetType() == typeof(SearchFilterCollection))
                    {
                        SearchFilterCollection filters = new SearchFilterCollection();
                        SearchFilter[] filterArray = ((SearchFilterCollection) filter).Contains(FieldName);
                        if (filterArray != null)
                        {
                            foreach (SearchFilter filter2 in filterArray)
                            {
                                list.Add(filter2);
                            }
                        }
                        continue;
                    }
                    if (filter.FieldName.ToLower().Equals(FieldName.ToLower()))
                    {
                        list.Add(filter);
                    }
                }
            }
            if (list.Count == 0)
            {
                return null;
            }
            return (SearchFilter[]) list.ToArray(typeof(SearchFilter));
        }

        internal bool ContainsOr()
        {
            if (this.LogicalOperator == Olympic.AutoDataLayer.LogicalOperator.Or)
            {
                return true;
            }
            foreach (SearchFilter filter in this.SearchFilters)
            {
                if ((filter is SearchFilterCollection) && ((SearchFilterCollection) filter).ContainsOr())
                {
                    return true;
                }
            }
            return false;
        }

        internal SearchFilter[] GetAllAncesters()
        {
            List<SearchFilter> list = new List<SearchFilter>();
            foreach (SearchFilter filter in this.SearchFilters)
            {
                if (filter is SearchFilterCollection)
                {
                    list.AddRange(((SearchFilterCollection) filter).GetAllAncesters());
                }
                else
                {
                    list.Add(filter);
                }
            }
            return list.ToArray();
        }

        public int IndexOf(SearchFilter value)
        {
            return this._searchFilters.IndexOf(value);
        }

        public void Insert(int index, SearchFilter value)
        {
            this._searchFilters.Insert(index, value);
        }

        public void Remove(SearchFilter value)
        {
            if (this._searchFilters != null)
            {
                foreach (SearchFilter filter in this._searchFilters)
                {
                    if (filter.GetType() == typeof(SearchFilterCollection))
                    {
                        SearchFilterCollection filters = new SearchFilterCollection();
                        ((SearchFilterCollection) filter).Remove(value);
                    }
                    else if (filter == value)
                    {
                        this._searchFilters.Remove(value);
                        break;
                    }
                }
            }
        }

        public void RemoveAt(int Index)
        {
            this._searchFilters.RemoveAt(Index);
        }

        public override string ToSQLWhereClause()
        {
            if ((this._searchFilters == null) || (this._searchFilters.Count == 0))
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("(");
            int num = 0;
            foreach (SearchFilter filter in this._searchFilters)
            {
                builder.Append(filter.ToSQLWhereClause());
                if (++num != this._searchFilters.Count)
                {
                    builder.Append(" " + this.LogicalOperator.ToString() + "\n");
                }
            }
            builder.Append(")");
            return builder.ToString();
        }

        public int Count
        {
            get
            {
                return this._searchFilters.Count;
            }
        }

        public SearchFilter this[int index]
        {
            get
            {
                return (SearchFilter) this._searchFilters[index];
            }
            set
            {
                this._searchFilters[index] = value;
            }
        }

        [XmlArray, XmlArrayItem(typeof(SearchFilter))]
        public SearchFilter[] SearchFilters
        {
            get
            {
                return (SearchFilter[]) this._searchFilters.ToArray(typeof(SearchFilter));
            }
            set
            {
                this._searchFilters = ArrayList.Adapter(value);
            }
        }
    }
}

