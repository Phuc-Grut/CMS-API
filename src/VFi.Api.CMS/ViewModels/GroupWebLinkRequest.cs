using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.CMS.ViewModels;

namespace VFi.Api.CMS.ViewModels;

public class AddGroupWebLinkRequest
{
    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
}
public class EditGroupWebLinkRequest : AddGroupWebLinkRequest
{
    public string Id { get; set; }
}
public class ListBoxGroupWebLinkRequest : ListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
public class PagingGroupWebLinkRequest : PagingRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
public class GroupWebLinkSort
{
    public Guid Id { get; set; }
    public int? SortOrder { get; set; }
}

public class EditGroupWebLinkSortRequest
{
    public List<GroupWebLinkSort> ListGui { get; set; }
}
