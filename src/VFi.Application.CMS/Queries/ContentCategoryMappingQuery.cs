using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Queries;

namespace VFi.Application.CMS.Queries;

public class ContentCategoryMappingQueryAll : IQuery<IEnumerable<ContentCategoryMappingDto>>
{
    public ContentCategoryMappingQueryAll()
    {
    }
}
public class ContentCategoryMappingQueryListBox : IQuery<IEnumerable<ListBoxDto>>
{
    public ContentCategoryMappingQueryListBox(ContentCategoryMappingQueryParams ContentQueryParams)
    {
        QueryParams = ContentQueryParams;
    }
    public ContentCategoryMappingQueryParams QueryParams { get; set; }
}
public class ContentCategoryMappingQueryById : IQuery<ContentCategoryMappingDto>
{
    public ContentCategoryMappingQueryById()
    {
    }

    public ContentCategoryMappingQueryById(Guid contentCategoryMappingId)
    {
        ContentCategoryMappingId = contentCategoryMappingId;
    }

    public Guid ContentCategoryMappingId { get; set; }
}
public class ContentCategoryMappingQueryCheckExist : IQuery<bool>
{

    public ContentCategoryMappingQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class ContentCategoryMappingPagingQuery : ListQuery, IQuery<PagingResponse<ContentCategoryMappingDto>>
{
    public ContentCategoryMappingPagingQuery(ContentCategoryMappingQueryParams contentCategoryMappingQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        QueryParams = contentCategoryMappingQueryParams;
    }

    public ContentCategoryMappingPagingQuery(string? keyword, ContentCategoryMappingQueryParams contentCategoryMappingQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
    {
        QueryParams = contentCategoryMappingQueryParams;
    }

    public ContentCategoryMappingQueryParams QueryParams { get; set; }
}

public class ContentCategoryMappingQueryHandler :
                                         IQueryHandler<ContentCategoryMappingQueryCheckExist, bool>,
                                         IQueryHandler<ContentCategoryMappingQueryById, ContentCategoryMappingDto>,
                                         IQueryHandler<ContentCategoryMappingPagingQuery, PagingResponse<ContentCategoryMappingDto>>,
                                         IQueryHandler<ContentCategoryMappingQueryAll, IEnumerable<ContentCategoryMappingDto>>
{
    private readonly IContentCategoryMappingRepository _contentCategoryMappingRepository;
    private readonly IContentRepository _contentRepository;
    public ContentCategoryMappingQueryHandler(IContentCategoryMappingRepository contentCategoryMappingRespository, IContentRepository contentRepository)
    {
        _contentCategoryMappingRepository = contentCategoryMappingRespository;
        _contentRepository = contentRepository;
    }
    public async Task<bool> Handle(ContentCategoryMappingQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _contentCategoryMappingRepository.CheckExistById(request.Id);
    }
    public async Task<ContentCategoryMappingDto> Handle(ContentCategoryMappingQueryById request, CancellationToken cancellationToken)
    {
        var contentCategoryMapping = await _contentCategoryMappingRepository.GetById(request.ContentCategoryMappingId);
        var result = new ContentCategoryMappingDto()
        {
            Id = contentCategoryMapping.Id,
            CategoryId = contentCategoryMapping.CategoryId,
            ContentId = contentCategoryMapping.ContentId,
            DisplayOrder = contentCategoryMapping.DisplayOrder
        };
        return result;
    }

    public async Task<PagingResponse<ContentCategoryMappingDto>> Handle(ContentCategoryMappingPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<ContentCategoryMappingDto>();
        var filter = new Dictionary<string, object>();
        if (request.QueryParams.ContentId != null)
        {
            filter.Add("ContentId", request.QueryParams.ContentId);
        }
        if (request.QueryParams.CategoryId != null)
        {
            filter.Add("categoryId", request.QueryParams.CategoryId);
        }
        if (request.QueryParams.ListCategory != null)
        {
            filter.Add("listCategory", request.QueryParams.ListCategory);
        }
        var count = await _contentCategoryMappingRepository.FilterCount(filter);
        var contentCategoryMappings = await _contentCategoryMappingRepository.Filter(filter, request.PageSize, request.PageIndex);
        var data = contentCategoryMappings.Select(contentCategoryMapping => new ContentCategoryMappingDto()
        {
            Id = contentCategoryMapping.Id,
            CategoryId = contentCategoryMapping.CategoryId,
            ContentId = contentCategoryMapping.ContentId,
            DisplayOrder = contentCategoryMapping.DisplayOrder
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<ContentCategoryMappingDto>> Handle(ContentCategoryMappingQueryAll request, CancellationToken cancellationToken)
    {
        var productCategoryMappings = await _contentCategoryMappingRepository.GetAll();
        var result = productCategoryMappings.Select(productCategoryMapping => new ContentCategoryMappingDto()
        {
            Id = productCategoryMapping.Id,
            CategoryId = productCategoryMapping.CategoryId,
            ContentId = productCategoryMapping.ContentId,
            DisplayOrder = productCategoryMapping.DisplayOrder
        });
        return result;
    }

    public async Task<IEnumerable<ListBoxDto>> Handle(ContentCategoryMappingQueryListBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.QueryParams.ContentId != null)
        {
            filter.Add("ContentId", request.QueryParams.ContentId);
        }
        if (request.QueryParams.CategoryId != null)
        {
            filter.Add("categoryId", request.QueryParams.CategoryId);
        }
        if (request.QueryParams.ListCategory != null)
        {
            filter.Add("listCategory", request.QueryParams.ListCategory);
        }
        var ContentCategoryMappings = await _contentCategoryMappingRepository.GetListListBox(filter);
        var Contents = await _contentRepository.GetAll();

        var result = Contents.Where(x => ContentCategoryMappings.Any(y => y.ContentId == x.Id)).Select(x => new ListBoxDto()
        {
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
