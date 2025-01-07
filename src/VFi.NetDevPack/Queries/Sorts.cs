using System.Collections.Generic;
using System.Linq;

namespace VFi.NetDevPack.Queries;

public class Sorts : List<Sort>
{
    public Sorts()
    {
    }

    public Sorts(IEnumerable<Sort> collection)
        : base(collection)
    {
    }

    public Sorts(int capacity)
        : base(capacity)
    {
    }

    public string SortExpression =>
        string.Join(',', this.Select(m => m.SortExpression));
}
