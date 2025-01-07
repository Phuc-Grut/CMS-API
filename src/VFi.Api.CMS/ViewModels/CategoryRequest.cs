using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Models;

namespace VFi.Api.CMS.ViewModels;

public class AddCategoryRequest
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public string? Slug { get; set; }
    public string? ParentCategoryId { get; set; }
    public string? GroupCategoryId { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public string? Keywords { get; set; }
    public string? JsonData { get; set; }
}
public class EditCategoryRequest : AddCategoryRequest
{
    public string Id { get; set; } = null!;
}
public class ListBoxCategoryRequest : ListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }

    [FromQuery(Name = "$groupCategoryId")]
    public string? GroupCategoryId { get; set; }
    [FromQuery(Name = "$parentCategoryId")]
    public string? ParentCategoryId { get; set; }

    public CategoryQueryParams ToBaseQuery() => new CategoryQueryParams
    {
        Status = Status,
        ParentCategoryId = ParentCategoryId,
        GroupCategoryId = GroupCategoryId
    };
}
public class PagingCategoryRequest : PagingRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }

    [FromQuery(Name = "$groupCategoryId")]
    public string? GroupCategoryId { get; set; }
    [FromQuery(Name = "$parentCategoryId")]
    public string? ParentCategoryId { get; set; }
    public CategoryQueryParams ToBaseQuery() => new CategoryQueryParams
    {
        Status = Status,
        ParentCategoryId = ParentCategoryId,
        GroupCategoryId = GroupCategoryId
    };
}
public class PagingChannelContentRequest : PagingRequest
{
    [FromQuery(Name = "status")]
    public int? Status { get; set; }
    [FromQuery(Name = "channel")]
    public string? Channel { get; set; }
    [FromQuery(Name = "slug")]
    public string? Slug { get; set; }
}
