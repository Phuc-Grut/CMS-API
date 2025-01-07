using System.Collections.Generic;

namespace VFi.NetDevPack.Queries;

public class Sort
{
    public const string SortDirectionAsc = "ASC";
    public const string SortDirectionDesc = "DESC";

    public readonly IList<string> SortDirections = new List<string>
    {
        "ASC",
        "DESC"
    }.AsReadOnly();

    private string _sortDirection;

    public string SortBy { get; set; }

    public string SortDirection
    {
        get => _sortDirection;
        set => _sortDirection = ValidateSortDirectionAndDefault(value);
    }

    public string SortExpression => SortBy + " " + SortDirection;

    public bool ValidateSortDirection(string sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortDirection))
            return false;
        sortDirection = sortDirection.ToUpper();
        return SortDirections.Contains(sortDirection);
    }

    public string ValidateSortDirectionAndDefault(string sortDirection)
    {
        return ValidateSortDirection(sortDirection) ? sortDirection : "ASC";
    }
}
