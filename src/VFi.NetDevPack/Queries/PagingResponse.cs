using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Queries;

public class PagingResponse<TEntity> : BaseResponse
{
    public PagingResponse()
    {

    }
    public PagingResponse(bool status, IList<DetailError> detailErrors) : base(status, detailErrors)
    {
    }
    public IEnumerable<TEntity> Items { get; set; }

    public PagingResponse(IEnumerable<TEntity> items)
    {
        Items = items;
    }

    public long Total { get; set; }
    public long Count { get; set; }
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
}
