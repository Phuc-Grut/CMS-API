using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using VFi.Domain.CMS.Interfaces;

namespace VFi.Application.CMS.Commands.Validations;

public abstract class ItemValidation<T> : AbstractValidator<T> where T : ItemCommand

{
    protected readonly IItemRepository _context;
    private Guid Id;
    public ItemValidation(IItemRepository context, Guid id)
    {
        _context = context;
        Id = id;
    }
    public ItemValidation(IItemRepository context)
    {
        _context = context;
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

}
public class ItemAddCommandValidation : ItemValidation<ItemAddCommand>
{
    public ItemAddCommandValidation(IItemRepository context) : base(context)
    {
        ValidateId();
        ValidateName();
    }
}
public class FolderAddCommandValidation : ItemValidation<FolderAddCommand>
{
    public FolderAddCommandValidation(IItemRepository context) : base(context)
    {
        ValidateId();
        ValidateName();
    }

}
public class FolderEditCommandValidation : ItemValidation<FolderEditCommand>
{
    public FolderEditCommandValidation(IItemRepository context) : base(context)
    {
        ValidateId();
        ValidateIdExists();
        ValidateName();
    }
}
public class ItemEditCommandValidation : ItemValidation<ItemEditCommand>
{
    public ItemEditCommandValidation(IItemRepository context) : base(context)
    {
        ValidateId();
        ValidateIdExists();
        ValidateName();
    }
}

public class ItemDeleteCommandValidation : ItemValidation<ItemDeleteCommand>
{
    public ItemDeleteCommandValidation(IItemRepository context) : base(context)
    {
        ValidateId();
        ValidateIdExists();
    }
}
