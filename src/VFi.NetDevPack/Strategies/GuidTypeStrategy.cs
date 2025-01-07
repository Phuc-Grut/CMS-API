
using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Filter;

namespace VFi.NetDevPack.Strategies;

public class GuidTypeStrategy : IFilterDataTypeStrategy
{
    public string ConvertFilterToText(IFilter filter)
    {
        switch (filter.Operator)
        {
            case FilterOperators.Equal:
                return filter.Key + " ==  new Guid(\"" + filter.Value + "\")";
            case FilterOperators.NotEqual:
                return filter.Key + " != new Guid(\"" + filter.Value + "\")";
            case FilterOperators.Contains:
            case FilterOperators.NotContains:
            case FilterOperators.StartsWith:
            case FilterOperators.NotStartsWith:
            case FilterOperators.EndsWith:
            case FilterOperators.NotEndsWith:
            case FilterOperators.GreaterThan:
            case FilterOperators.GreaterOrEqualThan:
            case FilterOperators.LessThan:
            case FilterOperators.LessOrEqualThan:
            default:
                throw new GuidDataTypeNotSupportedException($"Guid filter does not support {filter.Operator}");
        }
    }
}
