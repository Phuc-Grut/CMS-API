using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.NetDevPack.Filter;
using VFi.NetDevPack.Order;
using VFi.NetDevPack.Page;

namespace VFi.NetDevPack.Queries;

public interface IFopRequest : IFilterRequest, IOrderRequest, IPageRequest
{

}
