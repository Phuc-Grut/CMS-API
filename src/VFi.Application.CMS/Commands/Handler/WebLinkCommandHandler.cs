using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

internal class WebLinkCommandHandler : CommandHandler, IRequestHandler<WebLinkAddCommand, ValidationResult>,
    IRequestHandler<WebLinkDeleteCommand, ValidationResult>, IRequestHandler<WebLinkEditCommand, ValidationResult>,
    IRequestHandler<WebLinkSortCommand, ValidationResult>
{
    private readonly IWebLinkRepository _webLinkRepository;

    public WebLinkCommandHandler(IWebLinkRepository WebLinkRepository)
    {
        _webLinkRepository = WebLinkRepository;
    }
    public void Dispose()
    {
        _webLinkRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(WebLinkAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_webLinkRepository))
            return request.ValidationResult;
        var webLink = new WebLink
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Image = request.Image,
            Image2 = request.Image2,
            Image3 = request.Image3,
            Url = request.Url,
            ParentWebLinkId = request.ParentWebLinkId,
            GroupWebLinkId = request.GroupWebLinkId,
            Status = request.Status,
            DisplayOrder = request.DisplayOrder,
            Keywords = request.Keywords,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName
        };

        //add domain event
        //webLink.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _webLinkRepository.Add(webLink);
        return await Commit(_webLinkRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(WebLinkDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_webLinkRepository))
            return request.ValidationResult;
        var webLink = new WebLink
        {
            Id = request.Id
        };

        //add domain event
        //webLink.AddDomainEvent(new UserAddEvent(user.Id, user.LoginId, user.FullName));

        _webLinkRepository.Remove(webLink);
        return await Commit(_webLinkRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(WebLinkEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_webLinkRepository))
            return request.ValidationResult;
        var dataWebLink = await _webLinkRepository.GetById(request.Id);


        dataWebLink.Code = request.Code;
        dataWebLink.Name = request.Name;
        dataWebLink.Description = request.Description;
        dataWebLink.Url = request.Url;
        dataWebLink.Image = request.Image;
        dataWebLink.Image2 = request.Image2;
        dataWebLink.Image3 = request.Image3;
        dataWebLink.ParentWebLinkId = request.ParentWebLinkId;
        dataWebLink.GroupWebLinkId = request.GroupWebLinkId;
        dataWebLink.Status = request.Status;
        dataWebLink.DisplayOrder = request.DisplayOrder;
        dataWebLink.Keywords = request.Keywords;
        dataWebLink.UpdatedBy = request.UpdatedBy;
        dataWebLink.UpdatedDate = request.UpdatedDate;
        dataWebLink.UpdatedByName = request.UpdatedByName;
        _webLinkRepository.Update(dataWebLink);
        return await Commit(_webLinkRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(WebLinkSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _webLinkRepository.GetAll();

        List<WebLink> stores = new List<WebLink>();

        foreach (var sort in request.SortList)
        {
            WebLink store = data.FirstOrDefault(c => c.Id == sort.Id);
            if (store != null)
            {
                store.DisplayOrder = sort.SortOrder;
                stores.Add(store);
            }
        }
        _webLinkRepository.Update(stores);
        return await Commit(_webLinkRepository.UnitOfWork);
    }
}
