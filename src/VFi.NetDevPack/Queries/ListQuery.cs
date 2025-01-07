
using System.Linq;

namespace VFi.NetDevPack.Queries;

public abstract class ListQuery
{
    protected ListQuery()
    {

    }
    protected ListQuery(int pageSize, int pageIndex)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
    protected ListQuery(int pageSize, int pageIndex, Sorts sorts)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Sorts = sorts;
    }

    protected ListQuery(int pageIndex, int pageSize, string sorts, string sortsDerection = "desc")
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        if (string.IsNullOrEmpty(sortsDerection))
            sortsDerection = "desc";
        Sorts = new Sorts
        {
            new Sort
            {
                SortBy = sorts,
                SortDirection = sortsDerection
            }
        };
    }

    protected ListQuery(int pageSize, int pageIndex, SortingInfo[] sorts)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        if (sorts != null)
        {
            Sorts = new Sorts(sorts.Select(sort => new Sort
            {
                SortBy = sort.Selector,
                SortDirection = sort.Desc ? "DESC" : "ASC"
            }));
        }
        else
        {
            Sorts = null;
        }
    }

    public Sorts Sorts { get; set; }
    public int PageIndex { get; }
    public int PageSize { get; }
}
