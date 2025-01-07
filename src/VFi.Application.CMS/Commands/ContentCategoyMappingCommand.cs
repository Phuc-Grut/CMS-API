using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using VFi.Application.CMS.Commands.Validations;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class ContentCategoryMappingCommand : Command
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public Guid ContentId { get; set; }
    public int DisplayOrder { get; set; }
}
public class ContentCategoryMappingAddCommand : ContentCategoryMappingCommand
{
    public ContentCategoryMappingAddCommand(
        Guid id,
        Guid categoryId,
        Guid contentId,
        int displayOrder)
    {
        Id = id;
        CategoryId = categoryId;
        ContentId = contentId;
        DisplayOrder = displayOrder;
    }
    public bool IsValid()
    {
        ValidationResult = new ContentCategoryMappingAddCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ContentCategoryMappingEditCommand : ContentCategoryMappingCommand
{
    public ContentCategoryMappingEditCommand(
        Guid id,
        Guid categoryId,
        Guid contentId,
        int displayOrder)
    {
        Id = id;
        CategoryId = categoryId;
        ContentId = contentId;
        DisplayOrder = displayOrder;
    }
    public bool IsValid()
    {
        ValidationResult = new ContentCategoryMappingEditCommandValidation().Validate(this);
        return ValidationResult.IsValid;
    }
}

public class ContentCategoryMappingDeleteCommand : ContentCategoryMappingCommand
{
    public ContentCategoryMappingDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IContentCategoryMappingRepository _context)
    {
        ValidationResult = new ContentCategoryMappingDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
