using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.CMS.Models;

public partial class SP_GET_CONTENT_BY_CONTENTTYPE
{
    public Guid Id { get; set; }
    public string? ContentType { get; set; }
    public int? TotalCountByType { get; set; }
}
