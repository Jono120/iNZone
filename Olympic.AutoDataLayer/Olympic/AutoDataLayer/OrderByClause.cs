namespace Olympic.AutoDataLayer
{
    using System;
    using System.Text;

    public class OrderByClause
    {
        public string FieldName;
        public Olympic.AutoDataLayer.OrderType OrderType;

        public OrderByClause(string fieldName) : this(fieldName, Olympic.AutoDataLayer.OrderType.Ascending)
        {
        }

        public OrderByClause(string fieldName, Olympic.AutoDataLayer.OrderType orderType)
        {
            this.FieldName = fieldName;
            this.OrderType = orderType;
        }

        public static implicit operator OrderByClauseCollection(OrderByClause orderByClause)
        {
            OrderByClauseCollection clauses = new OrderByClauseCollection();
            clauses.Add(orderByClause);
            return clauses;
        }

        public string ToSQLOrderByString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.FieldName);
            builder.Append(" ");
            switch (this.OrderType)
            {
                case Olympic.AutoDataLayer.OrderType.Ascending:
                    builder.Append("ASC");
                    break;

                case Olympic.AutoDataLayer.OrderType.Descending:
                    builder.Append("DESC");
                    break;
            }
            return builder.ToString();
        }
    }
}

