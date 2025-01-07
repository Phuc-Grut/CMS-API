using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Filter;

public interface IFilterList
{
    FilterLogic Logic { get; set; }

    IEnumerable<IFilter> Filters { get; set; }
}
