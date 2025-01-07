using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Models;

namespace VFi.Api.CMS.ViewModels;

public class AddWebLinkRequest
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Image2 { get; set; }
    public string? Image3 { get; set; }
    public string? Url { get; set; }
    public string? ParentWebLinkId { get; set; }
    public string? GroupWebLinkId { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public string? Keywords { get; set; }
}
public class EditWebLinkRequest : AddWebLinkRequest
{
    public string Id { get; set; } = null!;
}
public class ListBoxWebLinkRequest : ListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }

    [FromQuery(Name = "$groupWebLinkId")]
    public string? GroupWebLinkId { get; set; }
    [FromQuery(Name = "$parentWebLinkId")]
    public string? ParentWebLinkId { get; set; }

    public WebLinkQueryParams ToBaseQuery() => new WebLinkQueryParams
    {
        Status = Status,
        ParentWebLinkId = ParentWebLinkId,
        GroupWebLinkId = GroupWebLinkId
    };
}
public class PagingWebLinkRequest : PagingRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }

    [FromQuery(Name = "$groupWebLinkId")]
    public string? GroupWebLinkId { get; set; }
    [FromQuery(Name = "$parentWebLinkId")]
    public string? ParentWebLinkId { get; set; }
    public WebLinkQueryParams ToBaseQuery() => new WebLinkQueryParams
    {
        Status = Status,
        ParentWebLinkId = ParentWebLinkId,
        GroupWebLinkId = GroupWebLinkId
    };
}
public class SearchWebLinkRequest : PagingRequest
{
    public string? Group { get; set; }
    public string? Keyword { get; set; }

}
