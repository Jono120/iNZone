namespace Olympic.AutoDataLayer.BuildSupport
{
    using System;

    internal class IndexDefinition
    {
        public bool Clustered;
        public IndexFieldDefinitionCollection Fields = new IndexFieldDefinitionCollection();
        public string Name;
        public bool Unique;
    }
}

