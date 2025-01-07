

using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.CMS.Models;

namespace VFi.Application.CMS.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string Image2 { get; set; }
    public string Image1 { get; set; }
    public string Url { get; set; }
    public string Slug { get; set; }
    public string ParentCategoryName { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string Keywords { get; set; }
    public string JsonData { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public Guid? GroupCategoryId { get; set; }

    public string ParentIds { get; set; }
    public int? Level { get; set; }
    public string GroupCategoryCode { get; set; }
    public string GroupCategoryName { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }

    public List<CategoryDto> Children { get; set; }
}
public class CategoryListBoxDto
{
    public Guid Value { get; set; }
    public string Label { get; set; }
    public string? Key { get; set; }
    public bool Expanded { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public List<CategoryListBoxDto> Children { get; set; }
}
public class CategoryComboboxDto
{
    public Guid Value { get; set; }
    public string Label { get; set; }
    public string? Key { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? ParentCategoryId { get; set; }
}
public class CategoryQueryParams
{
    public int? Status { get; set; }
    public string? GroupCategoryId { get; set; }
    public string? ParentCategoryId { get; set; }
}
public class CategoryParentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
public class CategoryListViewDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? FullName { get; set; }
    public string Image { get; set; }
    public string Image2 { get; set; }
    public string Image1 { get; set; }
    public string Url { get; set; }
    public string Slug { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? ParentIds { get; set; }
    public Guid? GroupCategoryId { get; set; }
    public string? JsonData { get; set; }
    public int? Level { get; set; }
    public int DisplayOrder { get; set; }
    public int? Status { get; set; }

}
