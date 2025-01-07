using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.CMS.Models;

public partial class SP_GET_TOP_CATEGORY
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int? ContentCount { get; set; }
}
