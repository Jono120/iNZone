namespace Olympic.AutoDataLayer
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ExcludeDBAttribute : Attribute
    {
    }
}

