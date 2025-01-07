using VFi.NetDevPack.Exceptions;
using VFi.NetDevPack.Filter;

namespace VFi.NetDevPack.Strategies;

public class TimeSpanDataTypeStrategy : IFilterDataTypeStrategy
{
    public string ConvertFilterToText(IFilter filter)
    {
        switch (filter.Operator)
        {
            case FilterOperators.Equal:
                return filter.Key + " == TimeSpan.Parse(\"" + filter.Value + "\")";
            case FilterOperators.NotEqual:
                return filter.Key + " != TimeSpan.Parse(\"" + filter.Value + "\")";
            case FilterOperators.GreaterThan:
                return filter.Key + " > TimeSpan.Parse(\"" + filter.Value + "\")";
            case FilterOperators.GreaterOrEqualThan:
                return filter.Key + " >= TimeSpan.Parse(\"" + filter.Value + "\")";
            case FilterOperators.LessThan:
                return filter.Key + " < TimeSpan.Parse(\"" + filter.Value + "\")";
            case FilterOperators.LessOrEqualThan:
                return filter.Key + " <= TimeSpan.Parse(\"" + filter.Value + "\")";
            case FilterOperators.Contains:
            case FilterOperators.NotContains:
            case FilterOperators.StartsWith:
            case FilterOperators.NotStartsWith:
            case FilterOperators.EndsWith:
            case FilterOperators.NotEndsWith:
            default:
                throw new TimeSpanTypeNotSupportedException($"TimeSpan filter does not support {filter.Operator}");

        }
    }
}

