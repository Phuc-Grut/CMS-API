using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.CMS.Models;

public partial class SP_GET_TOP_NEW_CONTENT
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Image { get; set; }
    public string? shortDescription { get; set; }
}
