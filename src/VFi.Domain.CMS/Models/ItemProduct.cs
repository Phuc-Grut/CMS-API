using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VFi.NetDevPack.Domain;

namespace VFi.Domain.CMS.Models;

public class ItemProduct : Entity, IAggregateRoot
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Icon { get; set; }
    public Int64? Used { get; set; }
    public int DisplayOrder { get; set; }
    public string? Color { get; set; }
}
