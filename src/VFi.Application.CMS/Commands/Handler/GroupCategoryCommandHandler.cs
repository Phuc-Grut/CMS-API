using FluentValidation.Results;
using MediatR;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

internal class GroupCategoryCommandHandler : CommandHandler,
    IRequestHandler<GroupCategoryAddCommand, ValidationResult>,
    IRequestHandler<GroupCategoryDeleteCommand, ValidationResult>,
    IRequestHandler<GroupCategoryEditCommand, ValidationResult>,
    IRequestHandler<EditGroupCategorySortCommand, ValidationResult>
{
    private readonly IGroupCategoryRepository _groupCategoryRepository;

    public GroupCategoryCommandHandler(IGroupCategoryRepository GroupCategoryRepository)
    {
        _groupCategoryRepository = GroupCategoryRepository;
    }
    public void Dispose()
    {
        _groupCategoryRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(GroupCategoryAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_groupCategoryRepository))
            return request.ValidationResult;
        var groupCategory = new GroupCategory
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Title = request.Title,
            Description = request.Description,
            Image = request.Image,
            Logo = request.Logo,
            Logo2 = request.Logo2,
            Favicon = request.Favicon,
            Url = request.Url,
            Tags = request.Tags,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            Facebook = request.Facebook,
            Youtube = request.Youtube,
            Zalo = request.Zalo,
            Status = request.Status,
            DisplayOrder = request.DisplayOrder,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName
        };

        //add domain event
        //groupCategory.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _groupCategoryRepository.Add(groupCategory);
        return await Commit(_groupCategoryRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(GroupCategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_groupCategoryRepository))
            return request.ValidationResult;
        var groupCategory = new GroupCategory
        {
            Id = request.Id
        };

        //add domain event
        //groupCategory.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _groupCategoryRepository.Remove(groupCategory);
        return await Commit(_groupCategoryRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(EditGroupCategorySortCommand request, CancellationToken cancellationToken)
    {
        var datas = await _groupCategoryRepository.GetAll();

        List<GroupCategory> dataUpdate = new List<GroupCategory>();

        foreach (var list in request.List)
        {
            var data = datas.FirstOrDefault(x => x.Id == list.Id);

            if (data is not null)
            {
                data.DisplayOrder = list.SortOrder ?? 0;
                dataUpdate.Add(data);
            }
        }
        _groupCategoryRepository.Update(dataUpdate);
        return await Commit(_groupCategoryRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(GroupCategoryEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_groupCategoryRepository))
            return request.ValidationResult;
        var groupCategory = await _groupCategoryRepository.GetById(request.Id);
        groupCategory.Code = request.Code;
        groupCategory.Name = request.Name;
        groupCategory.Title = request.Title;
        groupCategory.Description = request.Description;
        groupCategory.Image = request.Image;
        groupCategory.Logo = request.Logo;
        groupCategory.Logo2 = request.Logo2;
        groupCategory.Favicon = request.Favicon;
        groupCategory.Url = request.Url;
        groupCategory.Tags = request.Tags;
        groupCategory.Email = request.Email;
        groupCategory.Phone = request.Phone;
        groupCategory.Address = request.Address;
        groupCategory.Facebook = request.Facebook;
        groupCategory.Youtube = request.Youtube;
        groupCategory.Zalo = request.Zalo;
        groupCategory.Status = request.Status;
        groupCategory.DisplayOrder = request.DisplayOrder;
        groupCategory.UpdatedBy = request.UpdatedBy;
        groupCategory.UpdatedDate = request.UpdatedDate;
        groupCategory.UpdatedByName = request.UpdatedByName;

        //add domain event
        //groupCategory.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _groupCategoryRepository.Update(groupCategory);
        return await Commit(_groupCategoryRepository.UnitOfWork);
    }
}
