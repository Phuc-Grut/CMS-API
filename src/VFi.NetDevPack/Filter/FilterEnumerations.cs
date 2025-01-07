namespace VFi.NetDevPack.Filter;

public enum FilterOperators : byte
{
    Equal,
    NotEqual,
    GreaterThan,
    GreaterOrEqualThan,
    LessThan,
    LessOrEqualThan,
    Contains,
    NotContains,
    StartsWith,
    NotStartsWith,
    EndsWith,
    NotEndsWith
}

public enum FilterLogic : byte
{
    And,
    Or
}

public enum FilterDataTypes : byte
{
    TimeSpan,
    String,
    Int,
    Float,
    Double,
    Long,
    Decimal,
    Char,
    DateTime,
    Boolean,
    Enum,
    Guid
}

