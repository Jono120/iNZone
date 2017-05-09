namespace Olympic.AutoDataLayer
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Text;

    public class OrderByClauseCollection : CollectionBase
    {
        public int Add(OrderByClause value)
        {
            return base.List.Add(value);
        }

        public bool Contains(OrderByClause value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(OrderByClause value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, OrderByClause value)
        {
            base.List.Insert(index, value);
        }

        protected override void OnInsert(int index, object value)
        {
            if (value.GetType() != typeof(OrderByClause))
            {
                throw new ArgumentException("value must be of type OrderByClause.", "value");
            }
        }

        protected override void OnRemove(int index, object value)
        {
            if (value.GetType() != typeof(OrderByClause))
            {
                throw new ArgumentException("value must be of type OrderByClause.", "value");
            }
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (newValue.GetType() != typeof(OrderByClause))
            {
                throw new ArgumentException("newValue must be of type OrderByClause.", "newValue");
            }
        }

        protected override void OnValidate(object value)
        {
            if (value.GetType() != typeof(OrderByClause))
            {
                throw new ArgumentException("value must be of type OrderByClause.");
            }
        }

        public void Remove(OrderByClause value)
        {
            base.List.Remove(value);
        }

        public OrderByClause[] ToArray()
        {
            return (OrderByClause[]) base.InnerList.ToArray(typeof(OrderByClause));
        }

        public string ToSQLOrderByString()
        {
            if (base.List.Count == 0)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            foreach (OrderByClause clause in base.List)
            {
                builder.Append(clause.ToSQLOrderByString());
                builder.Append(",");
            }
            return builder.ToString().TrimEnd(new char[] { ',' });
        }

        public OrderByClause this[int index]
        {
            get
            {
                return (OrderByClause) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

