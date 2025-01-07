using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.NetDevPack.Filter;

namespace VFi.NetDevPack.Strategies;

public interface IFilterDataTypeStrategy
{
    string ConvertFilterToText(IFilter filter);
}
