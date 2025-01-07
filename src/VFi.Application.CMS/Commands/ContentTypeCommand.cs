using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Consul;
using MassTransit.Internals.GraphValidation;
using VFi.Application.CMS.Commands.Validations;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class ContentTypeCommand : Command
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
}
public class ContentTypeAddCommand : ContentTypeCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public ContentTypeAddCommand(
        Guid id,
        string? code,
        string name,
        string? description,
        int status,
        int displayOrder,
        Guid createdBy,
        DateTime createdDate,
        string? createdName)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        DisplayOrder = displayOrder;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        CreatedByName = createdName;
    }
    public bool IsValid(IContentTypeRepository _context)
    {
        ValidationResult = new ContentTypeAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ContentTypeEditCommand : ContentTypeCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }

    public ContentTypeEditCommand(
       Guid id,
        string? code,
        string name,
        string? description,
        int status,
        int displayOrder,
        Guid? updatedBy,
        DateTime? updatedDate,
        string? updatedName)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        DisplayOrder = displayOrder;
        UpdatedBy = updatedBy;
        UpdatedDate = updatedDate;
        UpdatedByName = updatedName;
    }
    public bool IsValid(IContentTypeRepository _context)
    {
        ValidationResult = new ContentTypeEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ContentTypeSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public ContentTypeSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}

public class ContentTypeDeleteCommand : ContentTypeCommand
{
    public ContentTypeDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IContentTypeRepository _context)
    {
        ValidationResult = new ContentTypeDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
