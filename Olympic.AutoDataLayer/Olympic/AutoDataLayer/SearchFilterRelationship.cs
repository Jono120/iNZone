namespace Olympic.AutoDataLayer
{
    using System;

    public enum SearchFilterRelationship
    {
        Null,
        NotNull,
        True,
        False,
        Like,
        NotLike,
        Contains,
        Equal,
        NotEqual,
        GreaterThan,
        GreaterEqual,
        LessThan,
        LessEqual,
        Between,
        In,
        NotIn,
        Custom,
        IsNumeric,
        IsNonNumeric
    }
}

