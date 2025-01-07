using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.CMS.DTOs;

public class ContentCategoryMappingDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public Guid ContentId { get; set; }
    public int DisplayOrder { get; set; }
}

public class ContentCategoryMappingQueryParams
{
    public string? CategoryId { get; set; }
    public string? ContentId { get; set; }
    public List<string>? ListCategory { get; set; }
}
