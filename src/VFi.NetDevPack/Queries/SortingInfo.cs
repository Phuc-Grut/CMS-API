using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Queries;

public class SortingInfo
{ //
    // Summary:
    //     The data field to be used for sorting.
    public string Selector { get; set; }

    //
    // Summary:
    //     A flag indicating whether data should be sorted in a descending order.
    public bool Desc { get; set; }
}
