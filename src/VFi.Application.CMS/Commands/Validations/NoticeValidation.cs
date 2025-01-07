using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using VFi.Domain.CMS.Interfaces;

namespace VFi.Application.CMS.Commands.Validations;

public abstract class NoticeValidation<T> : AbstractValidator<T> where T : NoticeCommand

{
    protected readonly IContentRepository _context;
    private Guid Id;
    public NoticeValidation(IContentRepository context)
    {
        _context = context;
    }
    public NoticeValidation(IContentRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    protected void ValidateAddCodeUnique()
    {
        RuleFor(x => x.Code).Must(IsAddUnique).WithMessage("Code Already Exists");
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
        RuleFor(x => x.Code).Must(IsEditUnique).WithMessage("Code Already Exists");
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
public class NoticeAddCommandValidation : NoticeValidation<NoticeAddCommand>
{
    public NoticeAddCommandValidation(IContentRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
        ValidateName();
    }
}
public class NoticeEditCommandValidation : NoticeValidation<NoticeEditCommand>
{
    public NoticeEditCommandValidation(IContentRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateIdExists();
        ValidateName();
    }
}

public class NoticeDeleteCommandValidation : NoticeValidation<NoticeDeleteCommand>
{
    public NoticeDeleteCommandValidation(IContentRepository context) : base(context)
    {
        ValidateId();
        ValidateIdExists();
    }

}
public class NoticeDuplicateCommandValidation : NoticeValidation<NoticeDuplicateCommand>
{
    public NoticeDuplicateCommandValidation(IContentRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
        ValidateName();
    }
}
