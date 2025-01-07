using Microsoft.AspNetCore.Mvc;
using VFi.Application.CMS.DTOs;

namespace VFi.Api.CMS.ViewModels;

public class AddItemRequest
{
    public string Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Size { get; set; }
    public bool IsFile { get; set; }
    public string? ParentId { get; set; }
    public string? MimeType { get; set; }
    public bool? HasChild { get; set; }
    public string? LocalPath { get; set; }
    public string? Cdn { get; set; }
    public string? Product { get; set; }
    public int? Status { get; set; }
    public string? Workspace { get; set; }
    public string? Content { get; set; }
    public string? Tenant { get; set; }
}
public class PagingItemRequest : PagingRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
    [FromQuery(Name = "$product")]
    public string? Product { get; set; }
    [FromQuery(Name = "$isFile")]
    public bool? IsFile { get; set; }
    [FromQuery(Name = "$parentId")]
    public string? ParentId { get; set; }
    public ItemQueryParams ToBaseQuery() => new ItemQueryParams
    {
        Status = Status,
        Product = Product,
        IsFile = IsFile,
        ParentId = ParentId
    };
}
public class AddFolderRequest
{
    public string Title { get; set; }
    public string? ParentId { get; set; }
    public string? Product { get; set; }
    public bool IsFile { get; set; }
}
public class EditFolderRequest : AddFolderRequest
{
    public string Id { get; set; } = null!;
}
