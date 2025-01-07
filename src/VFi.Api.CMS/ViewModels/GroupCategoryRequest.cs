using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.CMS.ViewModels;

namespace VFi.Api.CMS.ViewModels;

public class AddGroupCategoryRequest
{
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Logo { get; set; }
    public string? Logo2 { get; set; }
    public string? Favicon { get; set; }
    public string? Url { get; set; }
    public string? Tags { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Facebook { get; set; }
    public string? Youtube { get; set; }
    public string? Zalo { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
}
public class EditGroupCategoryRequest : AddGroupCategoryRequest
{
    public string Id { get; set; }
}
public class ListBoxGroupCategoryRequest : ListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
public class PagingGroupCategoryRequest : PagingRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
public class GroupCategorySort
{
    public Guid Id { get; set; }
    public int? SortOrder { get; set; }
}

public class EditGroupCategorySortRequest
{
    public List<GroupCategorySort> ListGui { get; set; }
}
