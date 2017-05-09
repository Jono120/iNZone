namespace Olympic.AutoDataLayer.BuildSupport
{
    using System;
    using System.Collections.Specialized;
    using System.Reflection;

    internal class DatabaseFieldDefinitionCollection : NameObjectCollectionBase
    {
        internal DatabaseFieldDefinitionCollection()
        {
        }

        public void Add(DatabaseFieldDefinition fieldDefinition)
        {
            base.BaseAdd(fieldDefinition.FieldName, fieldDefinition);
        }

        public bool Contains(string fieldName)
        {
            return (base.BaseGet(fieldName) != null);
        }

        public DatabaseFieldDefinitionCollectionEnumerator GetEnumerator()
        {
            return new DatabaseFieldDefinitionCollectionEnumerator(this);
        }

        public void Remove(string fieldName)
        {
            base.BaseRemove(fieldName);
        }

        public DatabaseFieldDefinition this[string fieldName]
        {
            get
            {
                return (DatabaseFieldDefinition) base.BaseGet(fieldName);
            }
        }

        public DatabaseFieldDefinition this[int index]
        {
            get
            {
                return (DatabaseFieldDefinition) base.BaseGet(index);
            }
        }

        public class DatabaseFieldDefinitionCollectionEnumerator
        {
            private DatabaseFieldDefinitionCollection collection;
            private int nIndex;

            public DatabaseFieldDefinitionCollectionEnumerator(DatabaseFieldDefinitionCollection coll)
            {
                this.collection = coll;
                this.nIndex = -1;
            }

            public bool MoveNext()
            {
                this.nIndex++;
                return (this.nIndex < this.collection.Count);
            }

            public DatabaseFieldDefinition Current
            {
                get
                {
                    return (DatabaseFieldDefinition) this.collection.BaseGet(this.nIndex);
                }
            }
        }
    }
}

