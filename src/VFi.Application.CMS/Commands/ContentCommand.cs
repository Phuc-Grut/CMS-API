using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Consul;
using VFi.Application.CMS.Commands.Validations;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class ContentCommand : Command
{
    public Guid Id { get; set; }
    public Guid? ContentTypeId { get; set; }
    public string? ContentType { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
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
    public string? Tags { get; set; }
    public string? Slug { get; set; }

}
public class ContentAddCommand : ContentCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedByName { get; set; }

    public ContentAddCommand
        (
         Guid Id,
         Guid? ContenTypeId,
         string? ContentType,
         string? Code,
         string Name,
         string? LinkInfo,
         string? ShortDescription,
         string? FullDescription,
         Guid? CategoryRootId,
         string? CategoryRoot,
         string? IdGroupCategories,
         List<ContentCategoriesDto>? Categories,
         string? IdCategories,
         string? Image,
         bool Deleted,
         int Status,
         Guid? CreatedBy,
         DateTime? CreatedDate,
         string? CreatedByName,
         string? Tags
        )
    {
        this.Id = Id;
        this.ContentTypeId = ContenTypeId;
        this.ContentType = ContentType;
        this.Code = Code;
        this.Name = Name;
        this.LinkInfo = LinkInfo;
        this.ShortDescription = ShortDescription;
        this.FullDescription = FullDescription;
        this.CategoryRootId = CategoryRootId;
        this.CategoryRoot = CategoryRoot;
        this.IdGroupCategories = IdGroupCategories;
        this.Categories = Categories;
        this.IdCategories = IdCategories;
        this.Image = Image;
        this.Deleted = Deleted;
        this.Status = Status;
        this.CreatedBy = CreatedBy;
        this.CreatedDate = CreatedDate;
        this.CreatedByName = CreatedByName;
        this.Tags = Tags;

    }
    public bool IsValid(IContentRepository _context)
    {
        ValidationResult = new ContentAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ContentEditCommand : ContentCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
    public ContentEditCommand
        (
         Guid Id,
         Guid? ContenTypeId,
         string? ContentType,
         string? Code,
         string Name,
         string? LinkInfo,
         string? ShortDescription,
         string? FullDescription,
         Guid? CategoryRootId,
         string? CategoryRoot,
         string? IdGroupCategories,
         List<ContentCategoriesDto>? Categories,
         string? IdCategories,
         string? Image,
         bool Deleted,
         int Status,
         Guid? UpdatedBy,
         DateTime? UpdatedDate,
         string? UpdatedByName,
         string? Tags
        )
    {
        this.Id = Id;
        this.ContentTypeId = ContenTypeId;
        this.ContentType = ContentType;
        this.Code = Code;
        this.Name = Name;
        this.LinkInfo = LinkInfo;
        this.ShortDescription = ShortDescription;
        this.FullDescription = FullDescription;
        this.CategoryRootId = CategoryRootId;
        this.CategoryRoot = CategoryRoot;
        this.IdGroupCategories = IdGroupCategories;
        this.Categories = Categories;
        this.IdCategories = IdCategories;
        this.Image = Image;
        this.Deleted = Deleted;
        this.Status = Status;
        this.UpdatedBy = UpdatedBy;
        this.UpdatedDate = UpdatedDate;
        this.UpdatedByName = UpdatedByName;
        this.Tags = Tags;
    }
    public bool IsValid(IContentRepository _context)
    {
        ValidationResult = new ContentEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ContentDeleteCommand : ContentCommand
{
    public ContentDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IContentRepository _context)
    {
        ValidationResult = new ContentDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ContentDuplicateCommand : ContentCommand
{
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public ContentDuplicateCommand(
        Guid id,
        string? code,
        string name,
        int status,
        Guid? createdBy,
        string? createdByName
    )
    {
        Id = id;
        Code = code;
        Name = name;
        Status = status;
        CreatedBy = createdBy;
        CreatedByName = createdByName;

    }
    public bool IsValid(IContentRepository _context)

    {
        ValidationResult = new ContentDuplicateCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
