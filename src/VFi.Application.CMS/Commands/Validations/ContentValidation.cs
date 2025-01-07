using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using VFi.Domain.CMS.Interfaces;

namespace VFi.Application.CMS.Commands.Validations;

public abstract class ContentValidation<T> : AbstractValidator<T> where T : ContentCommand

{
    protected readonly IContentRepository _context;
    private Guid Id;
    public ContentValidation(IContentRepository context)
    {
        _context = context;
    }
    public ContentValidation(IContentRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("ContentCode AlreadyExists");
    }
    private bool IsAddUnique(string code)
    {
        var model = _context.GetByCode(code).Result;

        if (model == null)
        {
            return true;
        }

        return false;
    }
    private bool IsEditUnique(string code)
    {
        var model = _context.GetByCode(code).Result;

        if (model == null || model.Id == Id)
        {
            return true;
        }

        return false;
    }
    protected void ValidateEditCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("ContentCode AlreadyExists");
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
            .NotEqual(Guid.Empty).WithMessage("Id IsRequired");
    }
    protected void ValidateName()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name IsRequired")
            .Length(2, 400).WithMessage("The name must have between 2 and 400 characters");
    }

}
public class ContentAddCommandValidation : ContentValidation<ContentAddCommand>
{
    public ContentAddCommandValidation(IContentRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
        ValidateName();
    }
}
public class ContentEditCommandValidation : ContentValidation<ContentEditCommand>
{
    public ContentEditCommandValidation(IContentRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateIdExists();
        ValidateName();
    }
}

public class ContentDeleteCommandValidation : ContentValidation<ContentDeleteCommand>
{
    public ContentDeleteCommandValidation(IContentRepository context) : base(context)
    {
        ValidateId();
        ValidateIdExists();
    }

}
public class ContentDuplicateCommandValidation : ContentValidation<ContentDuplicateCommand>
{
    public ContentDuplicateCommandValidation(IContentRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
        ValidateName();
    }
}
