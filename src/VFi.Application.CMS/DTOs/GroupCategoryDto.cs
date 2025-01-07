using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.CMS.DTOs;

public class GroupCategoryDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string Logo { get; set; }
    public string Logo2 { get; set; }
    public string Favicon { get; set; }
    public string Url { get; set; }
    public string Tags { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Facebook { get; set; }
    public string Youtube { get; set; }
    public string Zalo { get; set; }
    public string Slug { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
}
public class GroupCategorySortDto
{
    public Guid Id { get; set; }
    public int? SortOrder { get; set; }

}
