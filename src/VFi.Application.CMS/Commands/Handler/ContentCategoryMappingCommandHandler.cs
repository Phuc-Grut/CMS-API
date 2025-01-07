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

internal class ContentCategoryMappingCommandHandler : CommandHandler, IRequestHandler<ContentCategoryMappingAddCommand, ValidationResult>,
                                                                      IRequestHandler<ContentCategoryMappingDeleteCommand, ValidationResult>,
                                                                      IRequestHandler<ContentCategoryMappingEditCommand, ValidationResult>

{
    private readonly IContentCategoryMappingRepository _contentCategoryMappingRepository;

    public ContentCategoryMappingCommandHandler(IContentCategoryMappingRepository contentCategoryMappingRepository)
    {
        _contentCategoryMappingRepository = contentCategoryMappingRepository;
    }
    public void Dispose()
    {
        _contentCategoryMappingRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(ContentCategoryMappingAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var contentCategoryMapping = new ContentCategoryMapping
        {
            Id = request.Id,
            ContentId = request.ContentId,
            CategoryId = request.CategoryId,
            DisplayOrder = request.DisplayOrder
        };
        _contentCategoryMappingRepository.Add(contentCategoryMapping);
        return await Commit(_contentCategoryMappingRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContentCategoryMappingDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentCategoryMappingRepository))
            return request.ValidationResult;
        var contentCategoryMapping = new ContentCategoryMapping
        {
            Id = request.Id
        };
        _contentCategoryMappingRepository.Remove(contentCategoryMapping);
        return await Commit(_contentCategoryMappingRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContentCategoryMappingEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return request.ValidationResult;
        var contentCategoryMapping = new ContentCategoryMapping
        {
            Id = request.Id,
            ContentId = request.ContentId,
            CategoryId = request.CategoryId,
            DisplayOrder = request.DisplayOrder
        };
        _contentCategoryMappingRepository.Update(contentCategoryMapping);
        return await Commit(_contentCategoryMappingRepository.UnitOfWork);
    }
}
