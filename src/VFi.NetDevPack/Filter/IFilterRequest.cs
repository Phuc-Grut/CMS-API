using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Filter;

public interface IFilterRequest
{
    IEnumerable<IFilterList> FilterList { get; }
}
