using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class CategoryRootCommandHandler : CommandHandler, IRequestHandler<CategoryRootAddCommand, ValidationResult>,
                                                            IRequestHandler<CategoryRootDeleteCommand, ValidationResult>,
                                                            IRequestHandler<CategoryRootEditCommand, ValidationResult>,
                                                            IRequestHandler<CategoryRootSortCommand, ValidationResult>
{
    private readonly ICategoryRootRepository _categoryRootRepository;

    public CategoryRootCommandHandler(ICategoryRootRepository categoryRootRepository)
    {
        _categoryRootRepository = categoryRootRepository;
    }
    public void Dispose()
    {
        _categoryRootRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(CategoryRootAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_categoryRootRepository))
            return request.ValidationResult;
        var cateRoot = new CategoryRoot
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Url = request.Url,
            Slug = request.Slug,
            Image = request.Image,
            ParentCategoryId = request.ParentCategoryId,
            Status = request.Status,
            DisplayOrder = request.DisplayOrder,
            IdNumber = request.IdNumber,
            Keywords = request.Keywords,
            JsonData = request.JsonData,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName
        };

        //add domain event
        //category.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _categoryRootRepository.Add(cateRoot);
        return await Commit(_categoryRootRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CategoryRootDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_categoryRootRepository))
            return request.ValidationResult;
        var cateRoot = new CategoryRoot
        {
            Id = request.Id
        };

        //add domain event
        //category.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _categoryRootRepository.Remove(cateRoot);
        return await Commit(_categoryRootRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CategoryRootEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_categoryRootRepository))
            return request.ValidationResult;
        var category = await _categoryRootRepository.GetById(request.Id);
        category.Code = request.Code;
        category.Name = request.Name;
        category.Description = request.Description;
        category.Url = request.Url;
        category.Slug = request.Slug;
        category.Image = request.Image;
        category.ParentCategoryId = request.ParentCategoryId;
        category.Status = request.Status;
        category.DisplayOrder = request.DisplayOrder;
        category.Keywords = request.Keywords;
        category.JsonData = request.JsonData;
        category.UpdatedBy = request.UpdatedBy;
        category.UpdatedDate = request.UpdatedDate;
        category.UpdatedByName = request.UpdatedByName;

        //add domain event
        //category.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _categoryRootRepository.Update(category);
        return await Commit(_categoryRootRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CategoryRootSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _categoryRootRepository.GetAll();

        List<CategoryRoot> stores = new List<CategoryRoot>();

        foreach (var sort in request.SortList)
        {
            CategoryRoot store = data.FirstOrDefault(c => c.Id == sort.Id);
            if (store != null)
            {
                store.DisplayOrder = sort.SortOrder;
                stores.Add(store);
            }
        }
        _categoryRootRepository.Update(stores);
        return await Commit(_categoryRootRepository.UnitOfWork);
    }
}
