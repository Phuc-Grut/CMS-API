using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Order;

public interface IOrderRequest
{
    string OrderBy { get; }

    OrderDirection Direction { get; }
}
