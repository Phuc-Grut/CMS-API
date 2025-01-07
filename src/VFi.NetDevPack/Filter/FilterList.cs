using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Filter;

public class FilterList : IFilterList
{
    public FilterLogic Logic { get; set; }

    public IEnumerable<IFilter> Filters { get; set; }
}
