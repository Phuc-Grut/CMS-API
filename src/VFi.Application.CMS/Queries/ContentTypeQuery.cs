using System.Data;
using Consul;
using MassTransit;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.Infra.CMS.Repository;
using VFi.NetDevPack.Queries;

namespace VFi.Application.CMS.Queries;

public class ContentTypeQueryAll : IQuery<IEnumerable<ContentTypeDto>>
{
    public ContentTypeQueryAll()
    {
    }
}

public class ContentTypeQueryListBox : IQuery<IEnumerable<ListBoxDto>>
{
    public ContentTypeQueryListBox(int? status, string? keyword)
    {
        Keyword = keyword;
        Status = status;
    }
    public int? Status { get; set; }
    public string? Keyword { get; set; }
}
public class ContentTypeQueryCheckExist : IQuery<bool>
{

    public ContentTypeQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class ContentTypeQueryById : IQuery<ContentTypeDto>
{
    public ContentTypeQueryById()
    {
    }

    public ContentTypeQueryById(Guid contentTypeId)
    {
        ContentTypeId = contentTypeId;
    }

    public Guid ContentTypeId { get; set; }
}
public class ContentTypePagingQuery : ListQuery, IQuery<PagingResponse<ContentTypeDto>>
{
    public ContentTypePagingQuery(string? keyword, int? status, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (status != null)
        {
            Filter.Add("status", status);
        }
    }

    public ContentTypePagingQuery(string? keyword, int? status, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (status != null)
        {
            Filter.Add("status", status);
        }
    }

    public string? Keyword { get; set; }
    public Dictionary<string, object> Filter
    {
        get; set;
    }
}

public class ContentTypeQueryHandler : IQueryHandler<ContentTypeQueryListBox, IEnumerable<ListBoxDto>>,
                                         IQueryHandler<ContentTypeQueryAll, IEnumerable<ContentTypeDto>>,
                                         IQueryHandler<ContentTypeQueryCheckExist, bool>,
                                         IQueryHandler<ContentTypeQueryById, ContentTypeDto>,
                                         IQueryHandler<ContentTypePagingQuery, PagingResponse<ContentTypeDto>>
{
    private readonly IContentTypeRepository _contentTypeRepository;
    public ContentTypeQueryHandler(IContentTypeRepository ContentTypeRespository)
    {
        _contentTypeRepository = ContentTypeRespository;
    }
    public async Task<bool> Handle(ContentTypeQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _contentTypeRepository.CheckExistById(request.Id);
    }

    public async Task<ContentTypeDto> Handle(ContentTypeQueryById request, CancellationToken cancellationToken)
    {
        var contentType = await _contentTypeRepository.GetById(request.ContentTypeId);
        var result = new ContentTypeDto()
        {
            Id = contentType.Id,
            Code = contentType.Code,
            Name = contentType.Name,
            Description = contentType.Description,
            DisplayOrder = contentType.DisplayOrder,
            Status = contentType.Status,
            CreatedBy = contentType.CreatedBy,
            CreatedDate = contentType.CreatedDate,
            CreatedByName = contentType.CreatedByName,
            UpdatedBy = contentType.UpdatedBy,
            UpdatedDate = contentType.UpdatedDate,
            UpdatedByName = contentType.UpdatedByName
        };
        return result;
    }

    public async Task<PagingResponse<ContentTypeDto>> Handle(ContentTypePagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<ContentTypeDto>();
        var count = await _contentTypeRepository.FilterCount(request.Keyword, request.Filter);
        var contentTypes = await _contentTypeRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
        var data = contentTypes.Select(contentType => new ContentTypeDto()
        {
            Id = contentType.Id,
            Code = contentType.Code,
            Name = contentType.Name,
            Description = contentType.Description,
            DisplayOrder = contentType.DisplayOrder,
            Status = contentType.Status,
            CreatedBy = contentType.CreatedBy,
            CreatedDate = contentType.CreatedDate,
            CreatedByName = contentType.CreatedByName,
            UpdatedBy = contentType.UpdatedBy,
            UpdatedDate = contentType.UpdatedDate,
            UpdatedByName = contentType.UpdatedByName
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<ContentTypeDto>> Handle(ContentTypeQueryAll request, CancellationToken cancellationToken)
    {
        var contentTypes = await _contentTypeRepository.GetAll();
        var result = contentTypes.Select(contentType => new ContentTypeDto()
        {
            Id = contentType.Id,
            Code = contentType.Code,
            Name = contentType.Name,
            Description = contentType.Description,
            DisplayOrder = contentType.DisplayOrder,
            Status = contentType.Status,
            CreatedBy = contentType.CreatedBy,
            CreatedDate = contentType.CreatedDate,
            UpdatedBy = contentType.UpdatedBy,
            UpdatedDate = contentType.UpdatedDate
        });
        return result;
    }

    public async Task<IEnumerable<ListBoxDto>> Handle(ContentTypeQueryListBox request, CancellationToken cancellationToken)
    {
        var contentType = await _contentTypeRepository.GetListListBox(request.Status, request.Keyword);
        var result = contentType.Select(x => new ListBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
