using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Filter;

public interface IFilter
{
    FilterOperators Operator { get; set; }

    FilterDataTypes DataType { get; set; }

    string Key { get; set; }

    string Value { get; set; }

    string Assembly { get; set; }

    string Fullname { get; set; }
}
