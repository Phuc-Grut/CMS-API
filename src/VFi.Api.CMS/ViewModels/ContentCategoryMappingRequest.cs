using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.CMS.DTOs;

namespace VFi.Api.CMS.ViewModels;

public class AddContentCategoryMappingRequest
{
    public string CategoryId { get; set; } = null!;
    public string ContentId { get; set; } = null!;
    public int DisplayOrder { get; set; }
}
public class EditContentCategoryMappingRequest : AddContentCategoryMappingRequest
{
    public string Id { get; set; } = null!;
}

public class PagingContentCategoryMappingRequest : PagingRequest
{
    [FromQuery(Name = "$categoryId")]
    public string? CategoryId { get; set; }
    [FromQuery(Name = "$ContentId")]
    public string? ContentId { get; set; }
    public ContentCategoryMappingQueryParams ToBaseQuery() => new ContentCategoryMappingQueryParams
    {
        CategoryId = CategoryId,
        ContentId = ContentId
    };
}
public class ListBoxContentRequest : ListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }

    [FromQuery(Name = "$contentTypeId")]
    public string? ContentTypeId { get; set; }

    public ContentQueryParams ToBaseQuery() => new ContentQueryParams
    {
        ContentTypeId = ContentTypeId,
        Status = Status,
    };
}
