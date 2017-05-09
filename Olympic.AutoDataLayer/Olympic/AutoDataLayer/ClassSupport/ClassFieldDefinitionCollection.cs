namespace Olympic.AutoDataLayer.ClassSupport
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Reflection;

    internal class ClassFieldDefinitionCollection : NameObjectCollectionBase
    {
        internal ClassFieldDefinitionCollection()
        {
        }

        public void Add(ClassFieldDefinition fieldDefinition)
        {
            base.BaseAdd(fieldDefinition.Name, fieldDefinition);
        }

        public bool Contains(string name)
        {
            return (base.BaseGet(name) != null);
        }

        public ClassFieldDefinitionCollectionEnumerator GetEnumerator()
        {
            return new ClassFieldDefinitionCollectionEnumerator(this);
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        public string ToString(string templateString, string separator, bool includeAutoNumberFields, bool primaryKeysOnly)
        {
            return this.ToString(templateString, separator, includeAutoNumberFields, primaryKeysOnly, null, null);
        }

        public string ToString(string templateString, string separator, bool includeAutoNumberFields, bool primaryKeysOnly, string versionNumberTemplate, string modifiedDateTemplate)
        {
            ArrayList list = new ArrayList();
            int num = 1;
            foreach (ClassFieldDefinition definition in this)
            {
                if (((includeAutoNumberFields || !definition.AutoNumber) && (!primaryKeysOnly || definition.Unique)) && !definition.IsAggregate)
                {
                    string str = templateString;
                    if (definition.IsVersionNumberField && (versionNumberTemplate != null))
                    {
                        str = versionNumberTemplate;
                    }
                    else if (definition.IsModifiedDateField && (versionNumberTemplate != null))
                    {
                        str = modifiedDateTemplate;
                    }
                    string str2 = str.Replace("{DatabaseFieldName}", definition.DatabaseFieldName).Replace("{Index}", num.ToString());
                    list.Add(str2);
                }
                num++;
            }
            return string.Join(separator, (string[]) list.ToArray(typeof(string)));
        }

        public ClassFieldDefinition this[string name]
        {
            get
            {
                return (ClassFieldDefinition) base.BaseGet(name);
            }
        }

        public ClassFieldDefinition this[int index]
        {
            get
            {
                return (ClassFieldDefinition) base.BaseGet(index);
            }
        }

        public class ClassFieldDefinitionCollectionEnumerator
        {
            private ClassFieldDefinitionCollection collection;
            private int nIndex;

            public ClassFieldDefinitionCollectionEnumerator(ClassFieldDefinitionCollection coll)
            {
                this.collection = coll;
                this.nIndex = -1;
            }

            public bool MoveNext()
            {
                this.nIndex++;
                return (this.nIndex < this.collection.Count);
            }

            public ClassFieldDefinition Current
            {
                get
                {
                    return (ClassFieldDefinition) this.collection.BaseGet(this.nIndex);
                }
            }
        }
    }
}

