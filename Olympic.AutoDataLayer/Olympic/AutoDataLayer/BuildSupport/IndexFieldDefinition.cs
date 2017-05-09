namespace Olympic.AutoDataLayer.BuildSupport
{
    using Olympic.AutoDataLayer;
    using System;

    internal class IndexFieldDefinition
    {
        public string Name;
        public OrderType SortOrder;

        public string ToIndexFieldListString()
        {
            string str = (this.SortOrder == OrderType.Descending) ? " DESC" : "";
            return ("[" + this.Name + "]" + str);
        }
    }
}

