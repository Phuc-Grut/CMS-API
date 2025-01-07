using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Page;

public interface IPageRequest
{
    int PageNumber { get; set; }

    int PageSize { get; set; }
}
