using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Models;

namespace VFi.Api.CMS.ViewModels;

public class AddCategoryRootRequest
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public string? Slug { get; set; }
    public string? ParentCategoryId { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public int IdNumber { get; set; }
    public string? Keywords { get; set; }
    public string? JsonData { get; set; }
}
public class EditCategoryRootRequest : AddCategoryRootRequest
{
    public string Id { get; set; } = null!;
}
public class ListBoxCategoryRootRequest : ListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }

    [FromQuery(Name = "$parentCategoryRootId")]
    public string? ParentCategoryRootId { get; set; }

    public CategoryRootQueryParams ToBaseQuery() => new CategoryRootQueryParams
    {
        Status = Status,
        ParentCategoryRootId = ParentCategoryRootId,
    };
}
public class PagingCategoryRootRequest : PagingRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }

    [FromQuery(Name = "$parentCategoryRootId")]
    public string? ParentCategoryRootId { get; set; }
    public CategoryRootQueryParams ToBaseQuery() => new CategoryRootQueryParams
    {
        Status = Status,
        ParentCategoryRootId = ParentCategoryRootId,
    };
}
