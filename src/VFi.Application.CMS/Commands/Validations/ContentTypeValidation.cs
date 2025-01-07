using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using VFi.Domain.CMS.Interfaces;

namespace VFi.Application.CMS.Commands.Validations;

public abstract class ContentTypeValidation<T> : AbstractValidator<T> where T : ContentTypeCommand

{
    protected readonly IContentTypeRepository _context;
    private Guid Id;
    public ContentTypeValidation(IContentTypeRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    public ContentTypeValidation(IContentTypeRepository context)
    {
        _context = context;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code alrealy exists");
    }

    private bool IsAddUnique(string? code)
    {
        return !_context.CheckExist(code, null).Result;
    }

    protected void ValidateEditCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code alrealy exists");
    }
    private bool IsEditUnique(string? code)
    {
        return !_context.CheckExist(code, Id).Result;
    }
    protected void ValidateIdExists()
    {
        RuleFor(x => x.Id).Must(IsExist).WithMessage("Id nots exists");
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
    protected void ValidateCode()
    {
        RuleFor(c => c.Code)
            .Length(0, 50).WithMessage("The code must have between 0 and 50 characters");
    }
    protected void ValidateName()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Please ensure you have entered the name")
            .Length(2, 225).WithMessage("The name must have between 2 and 225 characters");
    }
    protected void ValidateStatus()
    {
        RuleFor(c => c.Status)
            .NotNull().WithMessage("Please ensure you have selected the status");
    }
    protected void ValidateDisplayOrder()
    {
        RuleFor(c => c.DisplayOrder)
                  .NotNull().WithMessage("Please ensure you have entered the displayOrder");
    }
}
public class ContentTypeAddCommandValidation : ContentTypeValidation<ContentTypeAddCommand>
{
    public ContentTypeAddCommandValidation(IContentTypeRepository context) : base(context)
    {
        ValidateId();
        ValidateStatus();
        ValidateDisplayOrder();
        ValidateAddCodeUnique();
        ValidateCode();
        ValidateName();
    }
}
public class ContentTypeEditCommandValidation : ContentTypeValidation<ContentTypeEditCommand>
{
    public ContentTypeEditCommandValidation(IContentTypeRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateIdExists();
        ValidateStatus();
        ValidateDisplayOrder();
        ValidateEditCodeUnique();
        ValidateCode();
        ValidateName();
    }
}

public class ContentTypeDeleteCommandValidation : ContentTypeValidation<ContentTypeDeleteCommand>
{
    public ContentTypeDeleteCommandValidation(IContentTypeRepository context) : base(context)
    {
        ValidateId();
        ValidateIdExists();
    }
}
