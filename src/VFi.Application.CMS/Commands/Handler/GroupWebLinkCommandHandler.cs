using FluentValidation.Results;
using MediatR;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

internal class GroupWebLinkCommandHandler : CommandHandler,
    IRequestHandler<GroupWebLinkAddCommand, ValidationResult>,
    IRequestHandler<GroupWebLinkDeleteCommand, ValidationResult>,
    IRequestHandler<GroupWebLinkEditCommand, ValidationResult>,
    IRequestHandler<EditGroupWebLinkSortCommand, ValidationResult>
{
    private readonly IGroupWebLinkRepository _groupWebLinkRepository;

    public GroupWebLinkCommandHandler(IGroupWebLinkRepository GroupWebLinkRepository)
    {
        _groupWebLinkRepository = GroupWebLinkRepository;
    }
    public void Dispose()
    {
        _groupWebLinkRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(GroupWebLinkAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_groupWebLinkRepository))
            return request.ValidationResult;
        var groupWebLink = new GroupWebLink
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Title = request.Title,
            Description = request.Description,
            Image = request.Image,
            Url = request.Url,
            Status = request.Status,
            DisplayOrder = request.DisplayOrder,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName
        };

        //add domain event
        //groupWebLink.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _groupWebLinkRepository.Add(groupWebLink);
        return await Commit(_groupWebLinkRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(GroupWebLinkDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_groupWebLinkRepository))
            return request.ValidationResult;
        var groupWebLink = new GroupWebLink
        {
            Id = request.Id
        };

        //add domain event
        //groupWebLink.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _groupWebLinkRepository.Remove(groupWebLink);
        return await Commit(_groupWebLinkRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(EditGroupWebLinkSortCommand request, CancellationToken cancellationToken)
    {
        var datas = await _groupWebLinkRepository.GetAll();

        List<GroupWebLink> dataUpdate = new List<GroupWebLink>();

        foreach (var list in request.List)
        {
            var data = datas.FirstOrDefault(x => x.Id == list.Id);

            if (data is not null)
            {
                data.DisplayOrder = list.SortOrder ?? 0;
                dataUpdate.Add(data);
            }
        }
        _groupWebLinkRepository.Update(dataUpdate);
        return await Commit(_groupWebLinkRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(GroupWebLinkEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_groupWebLinkRepository))
            return request.ValidationResult;
        var groupWebLink = await _groupWebLinkRepository.GetById(request.Id);
        groupWebLink.Code = request.Code;
        groupWebLink.Name = request.Name;
        groupWebLink.Title = request.Title;
        groupWebLink.Description = request.Description;
        groupWebLink.Image = request.Image;
        groupWebLink.Url = request.Url;
        groupWebLink.Status = request.Status;
        groupWebLink.DisplayOrder = request.DisplayOrder;
        groupWebLink.UpdatedBy = request.UpdatedBy;
        groupWebLink.UpdatedDate = request.UpdatedDate;
        groupWebLink.UpdatedByName = request.UpdatedByName;

        //add domain event
        //groupWebLink.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _groupWebLinkRepository.Update(groupWebLink);
        return await Commit(_groupWebLinkRepository.UnitOfWork);
    }
}
