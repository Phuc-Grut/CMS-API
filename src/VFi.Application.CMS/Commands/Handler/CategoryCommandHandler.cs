using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

internal class CategoryCommandHandler : CommandHandler, IRequestHandler<CategoryAddCommand, ValidationResult>,
    IRequestHandler<CategoryDeleteCommand, ValidationResult>, IRequestHandler<CategoryEditCommand, ValidationResult>,
    IRequestHandler<CategorySortCommand, ValidationResult>
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryCommandHandler(ICategoryRepository CategoryRepository)
    {
        _categoryRepository = CategoryRepository;
    }
    public void Dispose()
    {
        _categoryRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(CategoryAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_categoryRepository))
            return request.ValidationResult;
        var category = new Category
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Image = request.Image,
            Url = request.Url,
            Slug = request.Slug,
            ParentCategoryId = request.ParentCategoryId,
            GroupCategoryId = request.GroupCategoryId,
            Status = request.Status,
            DisplayOrder = request.DisplayOrder,
            Keywords = request.Keywords,
            JsonData = request.JsonData,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName
        };

        //add domain event
        //category.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _categoryRepository.Add(category);
        return await Commit(_categoryRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_categoryRepository))
            return request.ValidationResult;
        var category = new Category
        {
            Id = request.Id
        };

        //add domain event
        //category.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _categoryRepository.Remove(category);
        return await Commit(_categoryRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CategoryEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_categoryRepository))
            return request.ValidationResult;
        var category = await _categoryRepository.GetById(request.Id);
        category.Code = request.Code;
        category.Name = request.Name;
        category.Description = request.Description;
        category.Url = request.Url;
        category.Slug = request.Slug;
        category.Image = request.Image;
        category.ParentCategoryId = request.ParentCategoryId;
        category.GroupCategoryId = request.GroupCategoryId;
        category.Status = request.Status;
        category.DisplayOrder = request.DisplayOrder;
        category.Keywords = request.Keywords;
        category.JsonData = request.JsonData;
        category.UpdatedBy = request.UpdatedBy;
        category.UpdatedDate = request.UpdatedDate;
        category.UpdatedByName = request.UpdatedByName;
        //add domain event
        //category.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _categoryRepository.Update(category);
        return await Commit(_categoryRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(CategorySortCommand request, CancellationToken cancellationToken)
    {
        var data = await _categoryRepository.GetAll();

        List<Category> stores = new List<Category>();

        foreach (var sort in request.SortList)
        {
            Category store = data.FirstOrDefault(c => c.Id == sort.Id);
            if (store != null)
            {
                store.DisplayOrder = sort.SortOrder;
                stores.Add(store);
            }
        }
        _categoryRepository.Update(stores);
        return await Commit(_categoryRepository.UnitOfWork);
    }
}
