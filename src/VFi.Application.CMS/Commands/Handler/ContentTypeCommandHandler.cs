using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands.Handler;

internal class ContentTypeCommandHandler : CommandHandler, IRequestHandler<ContentTypeAddCommand, ValidationResult>,
                                                           IRequestHandler<ContentTypeDeleteCommand, ValidationResult>,
                                                           IRequestHandler<ContentTypeEditCommand, ValidationResult>,
                                                           IRequestHandler<ContentTypeSortCommand, ValidationResult>
{
    private readonly IContentTypeRepository _contentTypeRepository;
    public ContentTypeCommandHandler(IContentTypeRepository contentTypeRepository)
    {
        _contentTypeRepository = contentTypeRepository;
    }
    public void Dispose()
    {
        _contentTypeRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(ContentTypeAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentTypeRepository))
            return request.ValidationResult;
        var contentType = new ContentType
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            DisplayOrder = request.DisplayOrder,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName
        };


        _contentTypeRepository.Add(contentType);
        return await Commit(_contentTypeRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContentTypeDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentTypeRepository))
            return request.ValidationResult;
        var contentType = new ContentType
        {
            Id = request.Id
        };


        _contentTypeRepository.Remove(contentType);
        return await Commit(_contentTypeRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContentTypeEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentTypeRepository))
            return request.ValidationResult;
        var contentType = await _contentTypeRepository.GetById(request.Id);

        contentType.Id = request.Id;
        contentType.Code = request.Code;
        contentType.Name = request.Name;
        contentType.Description = request.Description;
        contentType.Status = request.Status;
        contentType.DisplayOrder = request.DisplayOrder;
        contentType.UpdatedBy = request.UpdatedBy;
        contentType.UpdatedDate = request.UpdatedDate;
        contentType.UpdatedByName = request.UpdatedByName;


        _contentTypeRepository.Update(contentType);
        return await Commit(_contentTypeRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContentTypeSortCommand request, CancellationToken cancellationToken)
    {
        var data = await _contentTypeRepository.GetAll();

        List<ContentType> list = new List<ContentType>();

        foreach (var sort in request.SortList)
        {
            Domain.CMS.Models.ContentType obj = data.FirstOrDefault(c => c.Id == sort.Id);
            if (obj != null)
            {
                obj.DisplayOrder = sort.SortOrder;
                list.Add(obj);
            }
        }
        _contentTypeRepository.Update(list);
        return await Commit(_contentTypeRepository.UnitOfWork);
    }
}
