using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Queries;

public interface IFopQuery
{
    string Filter { get; set; }

    string Order { get; set; }

    int PageNumber { get; set; }

    int PageSize { get; set; }
}
