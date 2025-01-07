using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Filter;

public class Filter : IFilter
{
    public FilterOperators Operator { get; set; }

    public FilterDataTypes DataType { get; set; }

    public string Key { get; set; }

    public string Value { get; set; }

    public string Assembly { get; set; }

    public string Fullname { get; set; }
}
