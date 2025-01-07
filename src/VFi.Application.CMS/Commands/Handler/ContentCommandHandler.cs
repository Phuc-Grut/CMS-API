using FluentValidation.Results;
using MediatR;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands.Handler;

internal class ContentCommandHandler : CommandHandler, IRequestHandler<ContentAddCommand, ValidationResult>,
                                                        IRequestHandler<ContentDeleteCommand, ValidationResult>,
                                                        IRequestHandler<ContentEditCommand, ValidationResult>,
                                                        IRequestHandler<ContentDuplicateCommand, ValidationResult>
{
    private readonly IContentRepository _contentRepository;
    private readonly IContentCategoryMappingRepository _contentCategoryMappingRepository;
    private readonly IContentGroupCategoryMappingRepository _contentGroupCategoryMappingRepository;
    private readonly IDAMContextProcedures _damContextProcedures;

    public ContentCommandHandler
        (
        IContentRepository contentRepository,
        IContentCategoryMappingRepository contentCategoryMappingRepository,
        IContentGroupCategoryMappingRepository contentGroupCategoryMappingRepository,
        IDAMContextProcedures damContextProcedures
        )
    {
        _contentRepository = contentRepository;
        _contentCategoryMappingRepository = contentCategoryMappingRepository;
        _contentGroupCategoryMappingRepository = contentGroupCategoryMappingRepository;
        _damContextProcedures = damContextProcedures;
    }
    public void Dispose()
    {
        _contentRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(ContentAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentRepository))
            return request.ValidationResult;

        var content = new Content
        {
            Id = request.Id,
            ContentTypeId = request.ContentTypeId,
            ContentType = request.ContentType,
            Code = request.Code,
            Name = request.Name,
            LinkInfo = request.LinkInfo,
            ShortDescription = request.ShortDescription,
            FullDescription = request.FullDescription,
            CategoryRootId = request.CategoryRootId,
            CategoryRoot = request.CategoryRoot,
            Image = request.Image,
            Deleted = request.Deleted,
            Status = request.Status,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName,
            Tags = request.Tags,
            Slug = request.Slug
        };

        _contentRepository.Add(content);

        List<ContentGroupCategoryMapping> contentGroupCategories = new List<ContentGroupCategoryMapping>();
        if (request.IdGroupCategories != null && request.IdGroupCategories != "")
        {
            var groupCategories = request.IdGroupCategories.Split(',').ToList();
            foreach (string item in groupCategories)
            {
                contentGroupCategories.Add(new ContentGroupCategoryMapping()
                {
                    Id = Guid.NewGuid(),
                    ContentId = request.Id,
                    GroupCategoryId = new Guid(item)
                });
            }
            if (contentGroupCategories.Count > 0)
            {
                _contentGroupCategoryMappingRepository.Add(contentGroupCategories);
            }
        }
        List<ContentCategoryMapping> contentCategories = new List<ContentCategoryMapping>();
        if (request.Categories != null)
        {
            int i = 0;
            foreach (ContentCategoriesDto item in request.Categories)
            {
                contentCategories.Add(new ContentCategoryMapping()
                {
                    Id = Guid.NewGuid(),
                    ContentId = request.Id,
                    CategoryId = item.CategoryId,
                    GroupCategoryId = item.GroupCategoryId,
                    DisplayOrder = i
                });
                i++;
            }
            if (contentCategories.Count > 0)
            {
                _contentCategoryMappingRepository.Add(contentCategories);
            }
        }
        return await Commit(_contentRepository.UnitOfWork);

    }

    public async Task<ValidationResult> Handle(ContentDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentRepository))
            return request.ValidationResult;

        var content = await _contentRepository.GetById(request.Id);
        content.Deleted = true;
        content.DeletedDate = DateTime.Now;
        _contentRepository.Update(content);
        return await Commit(_contentRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContentEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentRepository))
            return request.ValidationResult;

        var dataCate = await _contentCategoryMappingRepository.Filter(request.Id);
        var dataGroup = await _contentGroupCategoryMappingRepository.Filter(request.Id);

        _contentCategoryMappingRepository.Remove(dataCate);
        _contentGroupCategoryMappingRepository.Remove(dataGroup);

        var content = await _contentRepository.GetById(request.Id);
        content.ContentTypeId = request.ContentTypeId;
        content.Code = request.Code;
        content.Name = request.Name;
        content.LinkInfo = request.LinkInfo;
        content.ShortDescription = request.ShortDescription;
        content.FullDescription = request.FullDescription;
        content.CategoryRootId = request.CategoryRootId;
        content.CategoryRoot = request.CategoryRoot;
        content.Image = request.Image;
        content.Deleted = request.Deleted;
        content.Status = request.Status;
        content.UpdatedBy = request.UpdatedBy;
        content.UpdatedByName = request.UpdatedByName;
        content.UpdatedDate = request.UpdatedDate;
        content.Tags = request.Tags;
        content.Slug = request.Slug;
        _contentRepository.Update(content);

        List<ContentGroupCategoryMapping> contentGroupCategories = new List<ContentGroupCategoryMapping>();
        if (request.IdGroupCategories != null && request.IdGroupCategories != "")
        {
            var groupCategories = request.IdGroupCategories.Split(',').ToList();
            foreach (string item in groupCategories)
            {
                contentGroupCategories.Add(new ContentGroupCategoryMapping()
                {
                    Id = Guid.NewGuid(),
                    ContentId = request.Id,
                    GroupCategoryId = new Guid(item)
                });
            }
            if (contentGroupCategories.Count > 0)
            {
                _contentGroupCategoryMappingRepository.Add(contentGroupCategories);
            }
        }
        List<ContentCategoryMapping> contentCategories = new List<ContentCategoryMapping>();
        if (request.Categories != null)
        {
            int i = 0;
            foreach (ContentCategoriesDto item in request.Categories)
            {
                contentCategories.Add(new ContentCategoryMapping()
                {
                    Id = Guid.NewGuid(),
                    ContentId = request.Id,
                    CategoryId = item.CategoryId,
                    GroupCategoryId = item.GroupCategoryId,
                    DisplayOrder = i
                });
                i++;
            }
            if (contentCategories.Count > 0)
            {
                _contentCategoryMappingRepository.Add(contentCategories);
            }
        }
        return await Commit(_contentRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ContentDuplicateCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentRepository))
            return request.ValidationResult;

        int rs = await _damContextProcedures.SP_COPY_CONTENTAsync(request.Id, request.Code, request.Name, request.Status, request.CreatedBy, request.CreatedByName);
        if (rs == 0)
        {
            return new ValidationResult(new List<ValidationFailure>()
            { new ValidationFailure("Duplicate", "Duplicate unsuccessful") });
        }
        else
        {
            return new ValidationResult();

        }
    }
}
