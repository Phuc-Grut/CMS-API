using FluentValidation.Results;
using MediatR;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands.Handler;

internal class GuideCommandHandler : CommandHandler, IRequestHandler<GuideAddCommand, ValidationResult>,
                                                        IRequestHandler<GuideDeleteCommand, ValidationResult>,
                                                        IRequestHandler<GuideEditCommand, ValidationResult>,
                                                        IRequestHandler<GuideDuplicateCommand, ValidationResult>
{
    private readonly IContentRepository _contentRepository;
    private readonly IContentTypeRepository _contentTypeRepository;
    private readonly IGroupCategoryRepository _groupCategoryRepository;
    private readonly IContentCategoryMappingRepository _contentCategoryMappingRepository;
    private readonly IContentGroupCategoryMappingRepository _contentGroupCategoryMappingRepository;
    private readonly IDAMContextProcedures _damContextProcedures;

    public GuideCommandHandler
        (
        IContentRepository contentRepository, IContentTypeRepository contentTypeRepository, IGroupCategoryRepository groupCategoryRepository,
        IContentCategoryMappingRepository contentCategoryMappingRepository,
        IContentGroupCategoryMappingRepository contentGroupCategoryMappingRepository,
        IDAMContextProcedures damContextProcedures
        )
    {
        _contentRepository = contentRepository;
        _contentTypeRepository = contentTypeRepository;
        _groupCategoryRepository = groupCategoryRepository;
        _contentCategoryMappingRepository = contentCategoryMappingRepository;
        _contentGroupCategoryMappingRepository = contentGroupCategoryMappingRepository;
        _damContextProcedures = damContextProcedures;
    }
    public void Dispose()
    {
        _contentRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(GuideAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentRepository))
            return request.ValidationResult;
        var contentType = await _contentTypeRepository.GetByCode("Guide");
        var content = new Content
        {
            Id = request.Id,
            ContentTypeId = contentType.Id,
            ContentType = request.Name,
            Code = request.Code,
            Name = request.Name,
            Title = request.Title,
            LinkInfo = "",
            ShortDescription = request.ShortDescription,
            FullDescription = request.FullDescription,
            Image = request.Image,
            Deleted = false,
            Status = request.Status,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
            CreatedByName = request.CreatedByName,
            Tags = request.Tags,
            Slug = request.Slug
        };

        _contentRepository.Add(content);

        var channel = await _groupCategoryRepository.GetByCode("Guide");

        var contentGroupCategories = new List<ContentGroupCategoryMapping>();
        contentGroupCategories.Add(new ContentGroupCategoryMapping()
        {
            Id = Guid.NewGuid(),
            ContentId = request.Id,
            GroupCategoryId = channel.Id
        });
        if (contentGroupCategories.Count > 0)
        {
            _contentGroupCategoryMappingRepository.Add(contentGroupCategories);
        }

        var contentCategories = new List<ContentCategoryMapping>();
        if (request.ListIdCategories != null)
        {
            int i = 0;
            foreach (var item in request.ListIdCategories)
            {
                contentCategories.Add(new ContentCategoryMapping()
                {
                    Id = Guid.NewGuid(),
                    ContentId = request.Id,
                    CategoryId = Guid.Parse(item),
                    GroupCategoryId = channel.Id,
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

    public async Task<ValidationResult> Handle(GuideDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentRepository))
            return request.ValidationResult;

        var content = await _contentRepository.GetById(request.Id);
        content.Deleted = true;
        content.DeletedDate = DateTime.Now;
        _contentRepository.Update(content);
        return await Commit(_contentRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(GuideEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_contentRepository))
            return request.ValidationResult;

        var dataCate = await _contentCategoryMappingRepository.Filter(request.Id);
        // var dataGroup = await _contentGroupCategoryMappingRepository.Filter(request.Id);

        _contentCategoryMappingRepository.Remove(dataCate);
        //  _contentGroupCategoryMappingRepository.Remove(dataGroup);

        var content = await _contentRepository.GetById(request.Id);
        content.Code = request.Code;
        content.Name = request.Name;
        content.Title = request.Title;
        content.ShortDescription = request.ShortDescription;
        content.FullDescription = request.FullDescription;
        content.Image = request.Image;
        content.Status = request.Status;
        content.UpdatedBy = request.UpdatedBy;
        content.UpdatedByName = request.UpdatedByName;
        content.UpdatedDate = request.UpdatedDate;
        content.Tags = request.Tags;
        content.Slug = request.Slug;
        _contentRepository.Update(content);

        //List<ContentGroupCategoryMapping> contentGroupCategories = new List<ContentGroupCategoryMapping>();
        //if (request.IdGroupCategories != null && request.IdGroupCategories != "")
        //{
        //    var groupCategories = request.IdGroupCategories.Split(',').ToList();
        //    foreach (string item in groupCategories)
        //    {
        //        contentGroupCategories.Add(new ContentGroupCategoryMapping()
        //        {
        //            Id = Guid.NewGuid(),
        //            ContentId = request.Id,
        //            GroupCategoryId = new Guid(item)
        //        });
        //    }
        //    if (contentGroupCategories.Count > 0)
        //    {
        //        _contentGroupCategoryMappingRepository.Add(contentGroupCategories);
        //    }
        //}
        var channel = await _groupCategoryRepository.GetByCode("Guide");
        List<ContentCategoryMapping> contentCategories = new List<ContentCategoryMapping>();
        if (request.ListIdCategories != null)
        {
            int i = 0;
            foreach (var item in request.ListIdCategories)
            {
                contentCategories.Add(new ContentCategoryMapping()
                {
                    Id = Guid.NewGuid(),
                    ContentId = request.Id,
                    CategoryId = Guid.Parse(item),
                    GroupCategoryId = channel.Id,
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

    public async Task<ValidationResult> Handle(GuideDuplicateCommand request, CancellationToken cancellationToken)
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
