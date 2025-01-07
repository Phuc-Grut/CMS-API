using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using VFi.Domain.CMS.Interfaces;

namespace VFi.Application.CMS.Commands.Validations;

public abstract class ContentCategoryMappingValidation<T> : AbstractValidator<T> where T : ContentCategoryMappingCommand
{
    protected readonly IContentCategoryMappingRepository _context;

    public ContentCategoryMappingValidation(IContentCategoryMappingRepository context)
    {
        _context = context;
    }
    public ContentCategoryMappingValidation()
    {
    }
    protected void ValidateIdExists()
    {
        RuleFor(x => x.Id).Must(IsExist).WithMessage("Id not exists");
    }
    private bool IsExist(Guid id)
    {
        return _context.CheckExistById(id).Result;
    }
    protected void ValidateId()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the Id");
    }
    protected void ValidateContentId()
    {
        RuleFor(c => c.ContentId)
            .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the ContentId");
    }
    protected void ValidateCategoryId()
    {
        RuleFor(c => c.CategoryId)
            .NotEqual(Guid.Empty).WithMessage("Please ensure you have entered the CategoryId");
    }
    protected void ValidateDisplayOrder()
    {
        RuleFor(c => c.DisplayOrder)
                  .NotNull().WithMessage("Please ensure you have entered the displayOrder");
    }

}
public class ContentCategoryMappingAddCommandValidation : ContentCategoryMappingValidation<ContentCategoryMappingAddCommand>
{
    public ContentCategoryMappingAddCommandValidation()
    {
        ValidateId();
        ValidateCategoryId();
        ValidateDisplayOrder();
        ValidateContentId();
    }
}
public class ContentCategoryMappingEditCommandValidation : ContentCategoryMappingValidation<ContentCategoryMappingEditCommand>
{
    public ContentCategoryMappingEditCommandValidation()
    {
        ValidateId();
        ValidateIdExists();
        ValidateCategoryId();
        ValidateDisplayOrder();
        ValidateContentId();
    }
}

public class ContentCategoryMappingDeleteCommandValidation : ContentCategoryMappingValidation<ContentCategoryMappingDeleteCommand>
{
    public ContentCategoryMappingDeleteCommandValidation(IContentCategoryMappingRepository context) : base(context)
    {
        ValidateId();
        ValidateIdExists();
    }
}
