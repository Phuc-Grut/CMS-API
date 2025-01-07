using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.CMS.DTOs;

public class GroupWebLinkDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string Image1 { get; set; }
    public string Image2 { get; set; }
    public string Url { get; set; }

    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
public class GroupWebLinkSortDto
{
    public Guid Id { get; set; }
    public int? SortOrder { get; set; }

}
