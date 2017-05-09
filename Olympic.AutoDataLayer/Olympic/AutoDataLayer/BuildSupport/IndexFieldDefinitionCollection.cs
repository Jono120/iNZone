namespace Olympic.AutoDataLayer.BuildSupport
{
    using System;
    using System.Collections.Specialized;
    using System.Reflection;

    internal class IndexFieldDefinitionCollection : NameObjectCollectionBase
    {
        internal IndexFieldDefinitionCollection()
        {
        }

        public void Add(IndexFieldDefinition fieldDefinition)
        {
            base.BaseAdd(fieldDefinition.Name, fieldDefinition);
        }

        public bool Contains(string fieldName)
        {
            return (base.BaseGet(fieldName) != null);
        }

        public IndexFieldDefinitionCollectionEnumerator GetEnumerator()
        {
            return new IndexFieldDefinitionCollectionEnumerator(this);
        }

        public void Remove(string fieldName)
        {
            base.BaseRemove(fieldName);
        }

        public IndexFieldDefinition this[string fieldName]
        {
            get
            {
                return (IndexFieldDefinition) base.BaseGet(fieldName);
            }
        }

        public IndexFieldDefinition this[int index]
        {
            get
            {
                return (IndexFieldDefinition) base.BaseGet(index);
            }
        }

        public class IndexFieldDefinitionCollectionEnumerator
        {
            private IndexFieldDefinitionCollection collection;
            private int nIndex;

            public IndexFieldDefinitionCollectionEnumerator(IndexFieldDefinitionCollection coll)
            {
                this.collection = coll;
                this.nIndex = -1;
            }

            public bool MoveNext()
            {
                this.nIndex++;
                return (this.nIndex < this.collection.Count);
            }

            public IndexFieldDefinition Current
            {
                get
                {
                    return (IndexFieldDefinition) this.collection.BaseGet(this.nIndex);
                }
            }
        }
    }
}

