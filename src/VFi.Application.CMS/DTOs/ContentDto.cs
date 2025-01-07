using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.CMS.DTOs;

public class ContentDto
{
    public Guid Id { get; set; }
    public Guid? ContentTypeId { get; set; }
    public string? ContentType { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? LinkInfo { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public Guid? CategoryRootId { get; set; }
    public string? CategoryRoot { get; set; }
    public string? IdGroupCategories { get; set; }
    public string? GroupCategories { get; set; }
    public string? Categories { get; set; }
    public string? IdCategories { get; set; }
    public string? Image { get; set; }
    public bool? Deleted { get; set; }
    public int? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public string? Tags { get; set; }
    public string Slug { get; set; }
    public int IdNumber { get; set; }

}
public class ContentFullDto : ContentDto
{
    public string? IdCategories { get; set; }
    public List<MappingContentCategoriesDto>? ListCategory { get; set; }
    public List<ContentMappingDto>? ListGroupCategory { get; set; }

}
public class ContentCategoriesDto
{
    public Guid CategoryId { get; set; }
    public Guid? GroupCategoryId { get; set; }
}
public class ContentMappingDto
{
    public Guid? Value { get; set; }
    public int? DisplayOrder { get; set; }
}
public class MappingContentCategoriesDto : ContentMappingDto
{
    public Guid? GroupCategoryId { get; set; }
    public string? Label { get; set; }
}
public class ContentQueryParams
{
    public string? ContentTypeId { get; set; }
    public string? CategoryRootId { get; set; }
    public int? Status { get; set; }
    public string? ListContent { get; set; }
}
public class ContentsResult
{
    public Guid? Id { get; set; }
    public bool IsValid { get; set; }
}
public class ContentDuplicateDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; }
    public int? Status { get; set; }
}
public class ContentListViewDto
{
    public Guid Id { get; set; }
    public Guid? ContentTypeId { get; set; }
    public string? ContentType { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public string? LinkInfo { get; set; }
    public string? ShortDescription { get; set; }
    public Guid? CategoryRootId { get; set; }
    public string? CategoryRoot { get; set; }
    public string? IdGroupCategories { get; set; }
    public string? GroupCategories { get; set; }
    public string? Categories { get; set; }
    public string? Image { get; set; }
    public int? Status { get; set; }
    public string? Tags { get; set; }
    public string? Slug { get; set; }
    public int IdNumber { get; set; }

}
