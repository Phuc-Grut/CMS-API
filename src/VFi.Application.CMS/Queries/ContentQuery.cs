using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;

namespace VFi.Application.CMS.Queries;

public class ContentQueryListBox : IQuery<IEnumerable<ListBoxDto>>
{
    public ContentQueryListBox(ContentQueryParams contentQueryParams, string? keyword)
    {
        Keyword = keyword;
        QueryParams = contentQueryParams;
    }
    public string? Keyword { get; set; }
    public ContentQueryParams QueryParams { get; set; }
}
public class ContentTopQuery : IQuery<IEnumerable<ContentDto>>
{
    public ContentTopQuery(string channel, string category, int top)
    {
        Channel = channel;
        Category = category;
        Top = top;
    }
    public string Keyword { get; set; }
    public string Channel { get; set; }
    public string? Category { get; set; }
    public int Top { get; set; }
    public int WithBody { get; set; } = 0;
}
public class PreNextContentQuery : IQuery<IEnumerable<ContentDto>>
{
    public PreNextContentQuery(string currentContent, int preCount, int nextCount)
    {
        CurrentContent = currentContent;
        PreCount = preCount;
        NextCount = nextCount;
    }

    public string CurrentContent { get; set; }
    public int PreCount { get; set; } = 1;
    public int NextCount { get; set; } = 1;
}

public class ContentQueryCheckExist : IQuery<bool>
{

    public ContentQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class ContentQueryById : IQuery<ContentFullDto>
{
    public ContentQueryById()
    {
    }

    public ContentQueryById(Guid contentId)
    {
        ContentId = contentId;
    }

    public Guid ContentId { get; set; }
}
public class ContentQueryByIdNumber : IQuery<ContentFullDto>
{
    public ContentQueryByIdNumber()
    {
    }

    public ContentQueryByIdNumber(int contentId)
    {
        ContentId = contentId;
    }

    public int ContentId { get; set; }
}
public class ContentQueryBySlug : IQuery<ContentFullDto>
{
    public ContentQueryBySlug()
    {
    }

    public ContentQueryBySlug(string channel, string slug)
    {
        Channel = channel;
        Slug = slug;
    }

    public string Channel { get; set; }
    public string Slug { get; set; }
}
public class ContentQueryByCategorySlug : IQuery<ContentFullDto>
{
    public ContentQueryByCategorySlug()
    {
    }

    public ContentQueryByCategorySlug(string channel, string category, string slug)
    {
        Category = category;
        Channel = channel;
        Slug = slug;
    }

    public string Channel { get; set; }
    public string Category { get; set; }
    public string Slug { get; set; }
}
public class ContentPagingFilterQuery : FopQuery, IQuery<PagedResult<List<ContentDto>>>
{
    public ContentPagingFilterQuery(string? filter, string? order, int pageNumber, int pageSize)
    {
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public ContentPagingFilterQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public ContentPagingFilterQuery(string? keyword, string? channel, string? filter, string? order, int pageNumber, int pageSize)
    {
        Keyword = keyword;
        Filter = filter;
        Order = order;
        PageNumber = pageNumber;
        PageSize = pageSize;
        Channel = channel;
    }
    public string? Keyword { get; set; }
    public string? Filter { get; set; }
    public string? Order { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? Channel { get; set; }

}

public class ContentQueryByCode : IQuery<ContentDuplicateDto>
{
    public ContentQueryByCode()
    {
    }

    public ContentQueryByCode(string code)
    {
        Code = code;
    }

    public string? Code { get; set; }
}

public class ContentPagingQuery : ListQuery, IQuery<PagingResponse<ContentDto>>
{

    public ContentPagingQuery(int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
    }
    public string? Channel { get; set; }
    public string Category { get; set; }
    public string? Keyword { get; set; }
    public bool? FullQuery { get; set; } = false;
    public Dictionary<string, object> Filter
    {
        get
        {
            var result = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(this.Keyword))
                result.Add("keyword", this.Keyword);
            if (!string.IsNullOrEmpty(Channel))
                result.Add("channel", Channel);
            Guid categoryId = Guid.Empty;
            if (Guid.TryParse(Category, out categoryId))
            {
                result.Add("categoryId", categoryId);
            }
            else
            {
                result.Add("categoryCode", Category);
            }
            return result;
        }
    }

}
public class ContentSearchQuery : IQuery<IEnumerable<ContentDto>>
{

    public ContentSearchQuery()
    {
    }
    public string Channel { get; set; }
    public string Keyword { get; set; }
    public int? Status { get; set; }
    public int Top { get; set; }
}
public class ContentRelatedQuery : IQuery<IEnumerable<ContentDto>>
{
    public ContentRelatedQuery()
    {
    }

    public string Channel { get; set; }
    public Guid ContentId { get; set; }
    public string Category { get; set; }
    public int Max { get; set; } = 10;
}

public class ContentDisplayTopQuery : IQuery<IEnumerable<ContentDto>>
{

    public ContentDisplayTopQuery()
    {
    }
    public string Channel { get; set; }
    public string Category { get; set; }
    public int? Status { get; set; }
    public int Top { get; set; } = 10;
}
public class ContentDisplayTop1Query : IQuery<IEnumerable<ContentDto>>
{

    public ContentDisplayTop1Query()
    {
    }
    public string Channel { get; set; }
    public string Category { get; set; }
    public int? Status { get; set; }
    public int Top { get; set; } = 10;
}
public class ContentDisplayTop2Query : IQuery<IEnumerable<ContentDto>>
{

    public ContentDisplayTop2Query()
    {
    }
    public string Channel { get; set; }
    public string Category { get; set; }
    public int? Status { get; set; }
    public int Top { get; set; } = 10;
}

public class ContentQueryHandler : IQueryHandler<ContentQueryListBox, IEnumerable<ListBoxDto>>,
                                        IQueryHandler<ContentTopQuery, IEnumerable<ContentDto>>,
                                        IQueryHandler<ContentSearchQuery, IEnumerable<ContentDto>>,
                                        IQueryHandler<ContentDisplayTopQuery, IEnumerable<ContentDto>>,
                                        IQueryHandler<ContentDisplayTop1Query, IEnumerable<ContentDto>>,
                                        IQueryHandler<ContentDisplayTop2Query, IEnumerable<ContentDto>>,
                                        IQueryHandler<PreNextContentQuery, IEnumerable<ContentDto>>,
                                        IQueryHandler<ContentRelatedQuery, IEnumerable<ContentDto>>,
                                        IQueryHandler<ContentQueryCheckExist, bool>,
                                        IQueryHandler<ContentQueryById, ContentFullDto>,
                                        IQueryHandler<ContentQueryByIdNumber, ContentFullDto>,
                                        IQueryHandler<ContentQueryBySlug, ContentFullDto>,
                                        IQueryHandler<ContentQueryByCategorySlug, ContentFullDto>,
                                        IQueryHandler<ContentPagingFilterQuery, PagedResult<List<ContentDto>>>,
                                        IQueryHandler<ContentQueryByCode, ContentDuplicateDto>,
                                        IQueryHandler<ContentPagingQuery, PagingResponse<ContentDto>>

{
    private readonly IContentRepository _contentRepository;
    private readonly IContentCategoryMappingRepository _contentCategoryMappingRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IContentGroupCategoryMappingRepository _contentGroupCategoryMappingRepository;
    public ContentQueryHandler(
        IContentRepository contentRespository,
        IContentCategoryMappingRepository contentCategoryMappingRepository,
        ICategoryRepository categoryRepository,
        IContentGroupCategoryMappingRepository contentGroupCategoryMappingRepository
        )
    {
        _contentRepository = contentRespository;
        _contentCategoryMappingRepository = contentCategoryMappingRepository;
        _categoryRepository = categoryRepository;
        _contentGroupCategoryMappingRepository = contentGroupCategoryMappingRepository;
    }

    public async Task<IEnumerable<ListBoxDto>> Handle(ContentQueryListBox request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        if (request.QueryParams.Status != null)
        {
            filter.Add("status", request.QueryParams.Status);
        }
        if (!String.IsNullOrEmpty(request.QueryParams.ContentTypeId))
        {
            filter.Add("contentType", request.QueryParams.ContentTypeId);
        }
        if (!String.IsNullOrEmpty(request.QueryParams.CategoryRootId))
        {
            filter.Add("categoryRoot", request.QueryParams.CategoryRootId);
        }

        var products = await _contentRepository.GetListListBox(filter, request.Keyword);
        var result = products.Select(x => new ListBoxDto()
        {
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }

    public async Task<bool> Handle(ContentQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _contentRepository.CheckExistById(request.Id);

    }

    public async Task<ContentFullDto> Handle(ContentQueryById request, CancellationToken cancellationToken)
    {
        var content = await _contentRepository.GetById(request.ContentId);
        var filter = new Dictionary<string, object>();
        filter.Add("contentId", request.ContentId);
        var listGroupCategory = await _contentGroupCategoryMappingRepository.GetListListBox(filter);
        var listCategoryMapping = await _contentCategoryMappingRepository.GetListListBox(filter);
        var listCategory = await _categoryRepository.GetByIds(listCategoryMapping);
        var result = new ContentFullDto()
        {
            Id = content.Id,
            Code = content.Code,
            ContentTypeId = content.ContentTypeId,
            ContentType = content.ContentType,
            Name = content.Name,
            Title = content.Title,
            LinkInfo = content.LinkInfo,
            ShortDescription = content.ShortDescription,
            FullDescription = content.FullDescription,
            CategoryRootId = content.CategoryRootId,
            CategoryRoot = content.CategoryRoot,
            IdGroupCategories = content.IdGroupCategories,
            GroupCategories = content.GroupCategories,
            Categories = content.Categories,
            IdCategories = content.IdCategories,
            Image = content.Image,
            Deleted = content.Deleted,
            Status = content.Status,
            CreatedBy = content.CreatedBy,
            UpdatedBy = content.UpdatedBy,
            CreatedDate = content.CreatedDate,
            UpdatedDate = content.UpdatedDate,
            CreatedByName = content.CreatedByName,
            UpdatedByName = content.UpdatedByName,
            Tags = content.Tags,
            IdNumber = content.IdNumber,
            Slug = content.Slug,
            ListGroupCategory = listGroupCategory.Select(x => new ContentMappingDto()
            {
                Value = x.GroupCategoryId,
            }
             ).ToList(),
            ListCategory = listCategory.Select(x => new MappingContentCategoriesDto()
            {
                Value = x.Id,
                Label = x.FullName,
                DisplayOrder = x.DisplayOrder,
                GroupCategoryId = x.GroupCategoryId
            }
            ).OrderBy(x => x.DisplayOrder).ToList(),

        };
        return result;
    }

    public async Task<ContentFullDto> Handle(ContentQueryByIdNumber request, CancellationToken cancellationToken)
    {
        var content = await _contentRepository.GetByIdNumber(request.ContentId);
        var filter = new Dictionary<string, object>();
        filter.Add("contentId", request.ContentId);
        // var listGroupCategory = await _contentGroupCategoryMappingRepository.GetListListBox(filter);
        //var listCategoryMapping = await _contentCategoryMappingRepository.GetListListBox(filter);
        //  var listCategory = await _categoryRepository.GetByIds(listCategoryMapping);
        var result = new ContentFullDto()
        {
            Id = content.Id,
            Code = content.Code,
            ContentTypeId = content.ContentTypeId,
            ContentType = content.ContentType,
            Name = content.Name,
            Title = content.Title,
            LinkInfo = content.LinkInfo,
            ShortDescription = content.ShortDescription,
            FullDescription = content.FullDescription,
            CategoryRootId = content.CategoryRootId,
            CategoryRoot = content.CategoryRoot,
            IdGroupCategories = content.IdGroupCategories,
            GroupCategories = content.GroupCategories,
            Categories = content.Categories,
            IdCategories = content.IdCategories,
            Image = content.Image,
            Deleted = content.Deleted,
            Status = content.Status,
            CreatedBy = content.CreatedBy,
            UpdatedBy = content.UpdatedBy,
            CreatedDate = content.CreatedDate,
            UpdatedDate = content.UpdatedDate,
            CreatedByName = content.CreatedByName,
            UpdatedByName = content.UpdatedByName,
            Tags = content.Tags,
            IdNumber = content.IdNumber,
            Slug = content.Slug

        };
        return result;
    }
    public async Task<ContentFullDto> Handle(ContentQueryBySlug request, CancellationToken cancellationToken)
    {
        var content = await _contentRepository.GetBySlug(request.Channel, request.Slug, 1);
        var filter = new Dictionary<string, object>();
        filter.Add("contentId", content.Id);
        var listGroupCategory = await _contentGroupCategoryMappingRepository.GetListListBox(filter);
        var listCategoryMapping = await _contentCategoryMappingRepository.GetListListBox(filter);
        var listCategory = await _categoryRepository.GetByIds(listCategoryMapping);
        var result = new ContentFullDto()
        {
            Id = content.Id,
            Code = content.Code,
            ContentTypeId = content.ContentTypeId,
            ContentType = content.ContentType,
            Name = content.Name,
            Title = content.Title,
            LinkInfo = content.LinkInfo,
            ShortDescription = content.ShortDescription,
            FullDescription = content.FullDescription,
            CategoryRootId = content.CategoryRootId,
            CategoryRoot = content.CategoryRoot,
            IdGroupCategories = content.IdGroupCategories,
            GroupCategories = content.GroupCategories,
            Categories = content.Categories,
            IdCategories = content.IdCategories,
            Image = content.Image,
            Deleted = content.Deleted,
            Status = content.Status,
            CreatedBy = content.CreatedBy,
            UpdatedBy = content.UpdatedBy,
            CreatedDate = content.CreatedDate,
            UpdatedDate = content.UpdatedDate,
            CreatedByName = content.CreatedByName,
            UpdatedByName = content.UpdatedByName,
            Tags = content.Tags,
            IdNumber = content.IdNumber,
            Slug = content.Slug,
            ListGroupCategory = listGroupCategory.Select(x => new ContentMappingDto()
            {
                Value = x.GroupCategoryId,
            }
             ).ToList(),
            ListCategory = listCategory.Select(x => new MappingContentCategoriesDto()
            {
                Value = x.Id,
                Label = x.FullName,
                DisplayOrder = x.DisplayOrder,
                GroupCategoryId = x.GroupCategoryId
            }
            ).OrderBy(x => x.DisplayOrder).ToList(),

        };
        return result;
    }
    public async Task<ContentFullDto> Handle(ContentQueryByCategorySlug request, CancellationToken cancellationToken)
    {
        var content = await _contentRepository.GetBySlug(request.Channel, request.Slug);
        var filter = new Dictionary<string, object>();
        filter.Add("contentId", content.Id);
        var listGroupCategory = await _contentGroupCategoryMappingRepository.GetListListBox(filter);
        var listCategoryMapping = await _contentCategoryMappingRepository.GetListListBox(filter);
        var listCategory = await _categoryRepository.GetByIds(listCategoryMapping);
        var result = new ContentFullDto()
        {
            Id = content.Id,
            Code = content.Code,
            ContentTypeId = content.ContentTypeId,
            ContentType = content.ContentType,
            Name = content.Name,
            Title = content.Title,
            LinkInfo = content.LinkInfo,
            ShortDescription = content.ShortDescription,
            FullDescription = content.FullDescription,
            CategoryRootId = content.CategoryRootId,
            CategoryRoot = content.CategoryRoot,
            IdGroupCategories = content.IdGroupCategories,
            GroupCategories = content.GroupCategories,
            Categories = content.Categories,
            IdCategories = content.IdCategories,
            Image = content.Image,
            Deleted = content.Deleted,
            Status = content.Status,
            CreatedBy = content.CreatedBy,
            UpdatedBy = content.UpdatedBy,
            CreatedDate = content.CreatedDate,
            UpdatedDate = content.UpdatedDate,
            CreatedByName = content.CreatedByName,
            UpdatedByName = content.UpdatedByName,
            Tags = content.Tags,
            IdNumber = content.IdNumber,
            Slug = content.Slug,
            ListGroupCategory = listGroupCategory.Select(x => new ContentMappingDto()
            {
                Value = x.GroupCategoryId,
            }
             ).ToList(),
            ListCategory = listCategory.Select(x => new MappingContentCategoriesDto()
            {
                Value = x.Id,
                Label = x.FullName,
                DisplayOrder = x.DisplayOrder,
                GroupCategoryId = x.GroupCategoryId
            }
            ).OrderBy(x => x.DisplayOrder).ToList(),

        };
        return result;
    }
    public async Task<PagedResult<List<ContentDto>>> Handle(ContentPagingFilterQuery request, CancellationToken cancellationToken)
    {
        var response = new PagedResult<List<ContentDto>>();

        var fopRequest = FopExpressionBuilder<Content>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

        var (datas, count) = await _contentRepository.Filter(request.Keyword, request.Channel, fopRequest);
        var result = datas.Select(content =>
        {
            return new ContentDto()
            {
                Id = content.Id,
                ContentTypeId = content.ContentTypeId,
                ContentType = content.ContentType,
                Code = content.Code,
                Name = content.Name,
                Title = content.Title,
                LinkInfo = content.LinkInfo,
                ShortDescription = content.ShortDescription,
                CategoryRootId = content.CategoryRootId,
                CategoryRoot = content.CategoryRoot,
                Categories = content.Categories,
                IdCategories = content.IdCategories,
                IdGroupCategories = content.IdGroupCategories,
                GroupCategories = content.GroupCategories,
                Image = content.Image,
                Status = content.Status,
                Tags = content.Tags,
                IdNumber = content.IdNumber,
                Slug = content.Slug,
                CreatedBy = content.CreatedBy,
                CreatedByName = content.CreatedByName,
                CreatedDate = content.CreatedDate,
                UpdatedDate = content.UpdatedDate
            };
        }
        ).ToList();
        response.Items = result;
        response.TotalCount = count;
        response.PageNumber = request.PageNumber;
        response.PageSize = request.PageSize;
        return response;
    }

    public async Task<ContentDuplicateDto> Handle(ContentQueryByCode request, CancellationToken cancellationToken)
    {
        var contentDuplicate = await _contentRepository.GetByCode(request.Code);

        var result = new ContentDuplicateDto()
        {
            Id = contentDuplicate.Id,
        };
        return result;
    }
    public async Task<PagingResponse<ContentDto>> Handle(ContentPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<ContentDto>();

        var count = await _contentRepository.FilterCount(request.Keyword, request.Filter);
        var items = await _contentRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
        if (count > 1)
        {
            var data = items.Select(content => new ContentDto()
            {
                Id = content.Id,
                ContentTypeId = content.ContentTypeId,
                ContentType = content.ContentType,
                Code = content.Code,
                Name = content.Name,
                Title = content.Title,
                LinkInfo = content.LinkInfo,
                ShortDescription = content.ShortDescription,
                FullDescription = request.FullQuery.Value ? content.FullDescription : "",
                CategoryRootId = content.CategoryRootId,
                CategoryRoot = content.CategoryRoot,
                Categories = content.Categories,
                IdCategories = content.IdCategories,
                IdGroupCategories = content.IdGroupCategories,
                GroupCategories = content.GroupCategories,
                Image = content.Image,
                Status = content.Status,
                Tags = content.Tags,
                IdNumber = content.IdNumber,
                Slug = content.Slug,
                CreatedBy = content.CreatedBy,
                CreatedByName = content.CreatedByName,
                CreatedDate = content.CreatedDate,
                UpdatedDate = content.UpdatedDate
            });
            response.Items = data;
            response.Total = count;
            response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }
        else
        {
            var data = items.Select(content => new ContentDto()
            {
                Id = content.Id,
                ContentTypeId = content.ContentTypeId,
                ContentType = content.ContentType,
                Code = content.Code,
                Name = content.Name,
                Title = content.Title,
                LinkInfo = content.LinkInfo,
                ShortDescription = content.ShortDescription,
                FullDescription = content.FullDescription,
                CategoryRootId = content.CategoryRootId,
                CategoryRoot = content.CategoryRoot,
                Categories = content.Categories,
                IdGroupCategories = content.IdGroupCategories,
                IdCategories = content.IdCategories,
                GroupCategories = content.GroupCategories,
                Image = content.Image,
                Status = content.Status,
                Tags = content.Tags,
                IdNumber = content.IdNumber,
                Slug = content.Slug,
                CreatedBy = content.CreatedBy,
                CreatedByName = content.CreatedByName,
                CreatedDate = content.CreatedDate,
                UpdatedDate = content.UpdatedDate
            });
            response.Items = data;
            response.Total = count;
            response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

    }

    public async Task<IEnumerable<ContentDto>> Handle(ContentTopQuery request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("channel", request.Channel);
        if (!string.IsNullOrWhiteSpace(request.Category))
        {
            var categoryId = Guid.Empty;
            if (Guid.TryParse(request.Category, out categoryId))
            {
                filter.Add("categoryId", categoryId);
            }
            else
            {
                filter.Add("categoryCode", request.Category);
            }
        }

        var items = await _contentRepository.Filter(request.Keyword, filter, request.Top, 1,1);
        return items.Select(content => new ContentDto()
        {
            Id = content.Id,
            ContentTypeId = content.ContentTypeId,
            ContentType = content.ContentType,
            Code = content.Code,
            Name = content.Name,
            Title = content.Title,
            LinkInfo = content.LinkInfo,
            ShortDescription = content.ShortDescription,
            FullDescription = request.WithBody == 1 ? content.FullDescription : "",
            CategoryRootId = content.CategoryRootId,
            CategoryRoot = content.CategoryRoot,
            Categories = content.Categories,
            IdCategories = content.IdCategories,
            IdGroupCategories = content.IdGroupCategories,
            GroupCategories = content.GroupCategories,
            Image = content.Image,
            Status = content.Status,
            Tags = content.Tags,
            IdNumber = content.IdNumber,
            Slug = content.Slug,
            CreatedDate = content.CreatedDate,
            UpdatedDate = content.UpdatedDate
        });
        ;
        ;

    }
    public async Task<IEnumerable<ContentDto>> Handle(ContentSearchQuery request, CancellationToken cancellationToken)
    {
        var filter = new Dictionary<string, object>();
        filter.Add("channel", request.Channel);
        var items = await _contentRepository.Filter(request.Keyword, filter, request.Top, 1);


        //var listIdCategories = items.Select(x=>x.IdCategories).ToList();
        //var listId = new List<Guid>();
        //foreach (var item in listIdCategories)
        //{
        //    listId.AddRange(item.Split(',').Select(x=>Guid.Parse(x)));
        //}
        //listId = listId.Distinct().ToList();
        //var listCate = _categoryRepository.GetCategoriesByListId(listId);

        return items.Select(content => new ContentDto()
        {
            Id = content.Id,
            ContentTypeId = content.ContentTypeId,
            ContentType = content.ContentType,
            Code = content.Code,
            Name = content.Name,
            Title = content.Title,
            LinkInfo = content.LinkInfo,
            ShortDescription = content.ShortDescription,
            CategoryRootId = content.CategoryRootId,
            CategoryRoot = content.CategoryRoot,
            Categories = content.Categories,
            IdCategories = content.IdCategories,
            IdGroupCategories = content.IdGroupCategories,
            GroupCategories = content.GroupCategories,
            Image = content.Image,
            Status = content.Status,
            Tags = content.Tags,
            IdNumber = content.IdNumber,
            Slug = content.Slug,
            CreatedDate = content.CreatedDate,
            UpdatedDate = content.UpdatedDate
        });

    }
    public Task<IEnumerable<ContentDto>> Handle(PreNextContentQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ContentDto>> Handle(ContentRelatedQuery request, CancellationToken cancellationToken)
    {
        var items = await _contentRepository.Related(request.Channel, request.Category, request.ContentId, request.Max);
        return items.Select(content => new ContentDto()
        {
            Id = content.Id,
            ContentTypeId = content.ContentTypeId,
            ContentType = content.ContentType,
            Code = content.Code,
            Name = content.Name,
            Title = content.Title,
            LinkInfo = content.LinkInfo,
            ShortDescription = content.ShortDescription,
            CategoryRootId = content.CategoryRootId,
            CategoryRoot = content.CategoryRoot,
            Categories = content.Categories,
            IdCategories = content.IdCategories,
            IdGroupCategories = content.IdGroupCategories,
            GroupCategories = content.GroupCategories,
            Image = content.Image,
            Status = content.Status,
            Tags = content.Tags,
            IdNumber = content.IdNumber,
            Slug = content.Slug,
            CreatedDate = content.CreatedDate,
            UpdatedDate = content.UpdatedDate
        });
        ;
    }

    public async Task<IEnumerable<ContentDto>> Handle(ContentDisplayTopQuery request, CancellationToken cancellationToken)
    {

        var items = await _contentRepository.DisplayTop(request.Channel, request.Category, request.Status, request.Top);
        return items.Select(content => new ContentDto()
        {
            Id = content.Id,
            ContentTypeId = content.ContentTypeId,
            ContentType = content.ContentType,
            Code = content.Code,
            Name = content.Name,
            Title = content.Title,
            LinkInfo = content.LinkInfo,
            ShortDescription = content.ShortDescription,
            //FullDescription = request.WithBody == 1 ? content.FullDescription : "",
            CategoryRootId = content.CategoryRootId,
            CategoryRoot = content.CategoryRoot,
            Categories = content.Categories,
            IdCategories = content.IdCategories,
            IdGroupCategories = content.IdGroupCategories,
            GroupCategories = content.GroupCategories,
            Image = content.Image,
            Status = content.Status,
            Tags = content.Tags,
            IdNumber = content.IdNumber,
            Slug = content.Slug,
            CreatedDate = content.CreatedDate,
            UpdatedDate = content.UpdatedDate
        });

    }
    public async Task<IEnumerable<ContentDto>> Handle(ContentDisplayTop1Query request, CancellationToken cancellationToken)
    {

        var items = await _contentRepository.DisplayTop1(request.Channel, request.Category, request.Status, request.Top);
        return items.Select(content => new ContentDto()
        {
            Id = content.Id,
            ContentTypeId = content.ContentTypeId,
            ContentType = content.ContentType,
            Code = content.Code,
            Name = content.Name,
            Title = content.Title,
            LinkInfo = content.LinkInfo,
            ShortDescription = content.ShortDescription,
            //FullDescription = request.WithBody == 1 ? content.FullDescription : "",
            CategoryRootId = content.CategoryRootId,
            CategoryRoot = content.CategoryRoot,
            Categories = content.Categories,
            IdCategories = content.IdCategories,
            IdGroupCategories = content.IdGroupCategories,
            GroupCategories = content.GroupCategories,
            Image = content.Image,
            Status = content.Status,
            Tags = content.Tags,
            IdNumber = content.IdNumber,
            Slug = content.Slug,
            CreatedDate = content.CreatedDate,
            UpdatedDate = content.UpdatedDate
        });

    }
    public async Task<IEnumerable<ContentDto>> Handle(ContentDisplayTop2Query request, CancellationToken cancellationToken)
    {

        var items = await _contentRepository.DisplayTop2(request.Channel, request.Category, request.Status, request.Top);
        return items.Select(content => new ContentDto()
        {
            Id = content.Id,
            ContentTypeId = content.ContentTypeId,
            ContentType = content.ContentType,
            Code = content.Code,
            Name = content.Name,
            Title = content.Title,
            LinkInfo = content.LinkInfo,
            ShortDescription = content.ShortDescription,
            //FullDescription = request.WithBody == 1 ? content.FullDescription : "",
            CategoryRootId = content.CategoryRootId,
            CategoryRoot = content.CategoryRoot,
            Categories = content.Categories,
            IdCategories = content.IdCategories,
            IdGroupCategories = content.IdGroupCategories,
            GroupCategories = content.GroupCategories,
            Image = content.Image,
            Status = content.Status,
            Tags = content.Tags,
            IdNumber = content.IdNumber,
            Slug = content.Slug,
            CreatedDate = content.CreatedDate,
            UpdatedDate = content.UpdatedDate
        });

    }
}
