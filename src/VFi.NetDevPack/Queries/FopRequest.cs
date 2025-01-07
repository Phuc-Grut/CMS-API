using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.NetDevPack.Filter;
using VFi.NetDevPack.Order;

namespace VFi.NetDevPack.Queries;

public class FopRequest : IFopRequest
{
    public IEnumerable<IFilterList> FilterList { get; set; }

    public string OrderBy { get; set; }

    public OrderDirection Direction { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
}
