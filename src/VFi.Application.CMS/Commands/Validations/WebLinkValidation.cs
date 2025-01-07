using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;

namespace VFi.Application.CMS.Commands.Validations;

public abstract class WebLinkValidation<T> : AbstractValidator<T> where T : WebLinkCommand

{
    protected readonly IWebLinkRepository _context;
    private Guid Id;
    public WebLinkValidation(IWebLinkRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    public WebLinkValidation(IWebLinkRepository context)
    {
        _context = context;
    }

    private bool IsAddUnique(string? code)
    {
        return !_context.CheckExist(code, null).Result;
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
    protected void ValidateCode()
    {
        RuleFor(c => c.Code)
            .Length(0, 50).WithMessage("The code must have between 0 and 50 characters");
    }
    protected void ValidateName()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Please ensure you have entered the name")
            .Length(2, 400).WithMessage("The name must have between 2 and 400 characters");
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
public class WebLinkAddCommandValidation : WebLinkValidation<WebLinkAddCommand>
{
    public WebLinkAddCommandValidation(IWebLinkRepository context) : base(context)
    {
        ValidateId();
        ValidateStatus();
        ValidateDisplayOrder();
        ValidateCode();
        ValidateName();
    }
}
public class WebLinkEditCommandValidation : WebLinkValidation<WebLinkEditCommand>
{
    public WebLinkEditCommandValidation(IWebLinkRepository context, Guid id) : base(context, id)
    {
        ValidateId();
        ValidateIdExists();
        ValidateStatus();
        ValidateDisplayOrder();
        ValidateCode();
        ValidateName();
    }
}

public class WebLinkDeleteCommandValidation : WebLinkValidation<WebLinkDeleteCommand>
{
    public WebLinkDeleteCommandValidation(IWebLinkRepository context) : base(context)
    {
        ValidateId();
        ValidateIdExists();
    }
}
