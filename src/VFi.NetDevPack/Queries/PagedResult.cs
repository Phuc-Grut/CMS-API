using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Queries;

public class PagedResult<T>
{


    public PagedResult()
    {

    }
    public PagedResult(T data, int totalCount, int pageNumber, int pageSize)
    {
        Items = data;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }


    public T Items { get; set; }

    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalPages
    {
        get
        {
            return ((TotalCount - 1) / PageSize) + 1;
        }
    }
}
