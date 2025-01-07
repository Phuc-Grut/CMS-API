using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using VFi.Domain.CMS.Interfaces;

namespace VFi.Application.CMS.Commands.Validations;

public abstract class UpgradeValidation<T> : AbstractValidator<T> where T : UpgradeCommand

{
    protected readonly IContentRepository _context;
    private Guid Id;
    public UpgradeValidation(IContentRepository context)
    {
        _context = context;
    }
    public UpgradeValidation(IContentRepository context, Guid id)
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
public class UpgradeAddCommandValidation : UpgradeValidation<UpgradeAddCommand>
{
    public UpgradeAddCommandValidation(IContentRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
        ValidateName();
    }
}
public class UpgradeEditCommandValidation : UpgradeValidation<UpgradeEditCommand>
{
    public UpgradeEditCommandValidation(IContentRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateIdExists();
        ValidateName();
    }
}

public class UpgradeDeleteCommandValidation : UpgradeValidation<UpgradeDeleteCommand>
{
    public UpgradeDeleteCommandValidation(IContentRepository context) : base(context)
    {
        ValidateId();
        ValidateIdExists();
    }

}
public class UpgradeDuplicateCommandValidation : UpgradeValidation<UpgradeDuplicateCommand>
{
    public UpgradeDuplicateCommandValidation(IContentRepository context) : base(context)
    {
        ValidateId();
        ValidateAddCodeUnique();
        ValidateName();
    }
}
