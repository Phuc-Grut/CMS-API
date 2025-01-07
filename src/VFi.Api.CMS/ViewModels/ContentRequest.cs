using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.CMS.DTOs;

namespace VFi.Api.CMS.ViewModels;

public class AddContentRequest
{
    public Guid? ContentTypeId { get; set; }
    public string? ContentType { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; }
    public string? LinkInfo { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public Guid? CategoryRootId { get; set; }
    public string? CategoryRoot { get; set; }
    public string? IdGroupCategories { get; set; }
    public List<ContentCategoriesDto>? Categories { get; set; }
    public string? IdCategories { get; set; }
    public string? Image { get; set; }
    public bool Deleted { get; set; }
    public int Status { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? Tags { get; set; }
    //public string? ModuleCode { get; set; }
    public int? IsAuto { get; set; }
    public string? Slug { get; set; }
}
public class EditContentRequest : AddContentRequest
{
    public string Id { get; set; } = null!;
}
public class DuplicateContentRequest
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; }
    public int Status { get; set; }
    public int IsAuto { get; set; }
    public string? ModuleCode { get; set; }

}
