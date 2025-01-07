using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Queries;

namespace VFi.Application.CMS.Queries;


public class GroupCategoryQueryAll : IQuery<IEnumerable<GroupCategoryDto>>
{
    public GroupCategoryQueryAll()
    {
    }
}

public class GroupCategoryQueryListBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public GroupCategoryQueryListBox(int? status, string? keyword)
    {
        Status = status;
        Keyword = keyword;
    }
    public int? Status { get; set; }
    public string? Keyword { get; set; }
}
public class GroupCategoryQueryCheckExist : IQuery<bool>
{

    public GroupCategoryQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class GroupCategoryQueryById : IQuery<GroupCategoryDto>
{
    public GroupCategoryQueryById()
    {
    }

    public GroupCategoryQueryById(Guid groupCategoryId)
    {
        GroupCategoryId = groupCategoryId;
    }

    public Guid GroupCategoryId { get; set; }
}
public class GroupCategoryQueryByCode : IQuery<GroupCategoryDto>
{
    public GroupCategoryQueryByCode()
    {
    }

    public GroupCategoryQueryByCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class GroupCategoryPagingQuery : ListQuery, IQuery<PagingResponse<GroupCategoryDto>>
{
    public GroupCategoryPagingQuery(string? keyword, int? status, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (status != null)
        {
            Filter.Add("status", status);
        }
    }

    public GroupCategoryPagingQuery(string? keyword, int? status, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
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
public class GroupCategoryQueryByListId : IQuery<IEnumerable<GroupCategoryDto>>
{
    public GroupCategoryQueryByListId()
    {
    }
    public List<Guid> ListId { get; set; }
}
public class GroupCategoryQueryHandler : IQueryHandler<GroupCategoryQueryListBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<GroupCategoryQueryAll, IEnumerable<GroupCategoryDto>>,
                                        IQueryHandler<GroupCategoryQueryByListId, IEnumerable<GroupCategoryDto>>,
                                         IQueryHandler<GroupCategoryQueryCheckExist, bool>,
                                         IQueryHandler<GroupCategoryQueryById, GroupCategoryDto>,
                                         IQueryHandler<GroupCategoryQueryByCode, GroupCategoryDto>,
                                         IQueryHandler<GroupCategoryPagingQuery, PagingResponse<GroupCategoryDto>>
{
    private readonly IGroupCategoryRepository _groupCategoryRepository;
    public GroupCategoryQueryHandler(IGroupCategoryRepository groupCategoryRespository)
    {
        _groupCategoryRepository = groupCategoryRespository;
    }
    public async Task<bool> Handle(GroupCategoryQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _groupCategoryRepository.CheckExistById(request.Id);

    }

    public async Task<GroupCategoryDto> Handle(GroupCategoryQueryById request, CancellationToken cancellationToken)
    {
        var groupCategory = await _groupCategoryRepository.GetById(request.GroupCategoryId);
        var result = new GroupCategoryDto()
        {
            Id = groupCategory.Id,
            Code = groupCategory.Code,
            Name = groupCategory.Name,
            Title = groupCategory.Title,
            Description = groupCategory.Description,
            Image = groupCategory.Image,
            Logo = groupCategory.Logo,
            Logo2 = groupCategory.Logo2,
            Favicon = groupCategory.Favicon,
            Url = groupCategory.Url,
            Tags = groupCategory.Tags,
            Email = groupCategory.Email,
            Phone = groupCategory.Phone,
            Address = groupCategory.Address,
            Facebook = groupCategory.Facebook,
            Youtube = groupCategory.Youtube,
            Zalo = groupCategory.Zalo,
            Slug = groupCategory.Slug,
            DisplayOrder = groupCategory.DisplayOrder,
            Status = groupCategory.Status,
            CreatedBy = groupCategory.CreatedBy,
            CreatedDate = groupCategory.CreatedDate,
            UpdatedBy = groupCategory.UpdatedBy,
            UpdatedDate = groupCategory.UpdatedDate
        };
        return result;
    }
    public async Task<GroupCategoryDto> Handle(GroupCategoryQueryByCode request, CancellationToken cancellationToken)
    {
        var groupCategory = await _groupCategoryRepository.GetByCode(request.Code);
        var result = new GroupCategoryDto()
        {
            Id = groupCategory.Id,
            Code = groupCategory.Code,
            Name = groupCategory.Name,
            Title = groupCategory.Title,
            Description = groupCategory.Description,
            Image = groupCategory.Image,
            Logo = groupCategory.Logo,
            Logo2 = groupCategory.Logo2,
            Favicon = groupCategory.Favicon,
            Url = groupCategory.Url,
            Tags = groupCategory.Tags,
            Email = groupCategory.Email,
            Phone = groupCategory.Phone,
            Address = groupCategory.Address,
            Facebook = groupCategory.Facebook,
            Youtube = groupCategory.Youtube,
            Zalo = groupCategory.Zalo,
            Slug = groupCategory.Slug,
            DisplayOrder = groupCategory.DisplayOrder,
            Status = groupCategory.Status,
            CreatedBy = groupCategory.CreatedBy,
            CreatedDate = groupCategory.CreatedDate,
            UpdatedBy = groupCategory.UpdatedBy,
            UpdatedDate = groupCategory.UpdatedDate,
            CreatedByName = groupCategory.CreatedByName,
            UpdatedByName = groupCategory.UpdatedByName
        };
        return result;
    }
    public async Task<PagingResponse<GroupCategoryDto>> Handle(GroupCategoryPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<GroupCategoryDto>();
        var count = await _groupCategoryRepository.FilterCount(request.Keyword, request.Filter);
        var groupCategorys = await _groupCategoryRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
        var data = groupCategorys.Select(groupCategory => new GroupCategoryDto()
        {
            Id = groupCategory.Id,
            Code = groupCategory.Code,
            Name = groupCategory.Name,
            Title = groupCategory.Title,
            Description = groupCategory.Description,
            Image = groupCategory.Image,
            Logo = groupCategory.Logo,
            Logo2 = groupCategory.Logo2,
            Favicon = groupCategory.Favicon,
            Url = groupCategory.Url,
            Tags = groupCategory.Tags,
            Email = groupCategory.Email,
            Phone = groupCategory.Phone,
            Address = groupCategory.Address,
            Facebook = groupCategory.Facebook,
            Youtube = groupCategory.Youtube,
            Zalo = groupCategory.Zalo,
            Slug = groupCategory.Slug,
            DisplayOrder = groupCategory.DisplayOrder,
            Status = groupCategory.Status,
            CreatedBy = groupCategory.CreatedBy,
            CreatedDate = groupCategory.CreatedDate,
            UpdatedBy = groupCategory.UpdatedBy,
            UpdatedDate = groupCategory.UpdatedDate
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<GroupCategoryDto>> Handle(GroupCategoryQueryAll request, CancellationToken cancellationToken)
    {
        var groupCategorys = await _groupCategoryRepository.GetAll();
        var result = groupCategorys.Select(groupCategory => new GroupCategoryDto()
        {
            Id = groupCategory.Id,
            Code = groupCategory.Code,
            Name = groupCategory.Name,
            Title = groupCategory.Title,
            Description = groupCategory.Description,
            Image = groupCategory.Image,
            Logo = groupCategory.Logo,
            Logo2 = groupCategory.Logo2,
            Favicon = groupCategory.Favicon,
            Url = groupCategory.Url,
            Tags = groupCategory.Tags,
            Email = groupCategory.Email,
            Phone = groupCategory.Phone,
            Address = groupCategory.Address,
            Facebook = groupCategory.Facebook,
            Youtube = groupCategory.Youtube,
            Zalo = groupCategory.Zalo,
            Slug = groupCategory.Slug,
            DisplayOrder = groupCategory.DisplayOrder,
            Status = groupCategory.Status,
            CreatedBy = groupCategory.CreatedBy,
            CreatedDate = groupCategory.CreatedDate,
            UpdatedBy = groupCategory.UpdatedBy,
            UpdatedDate = groupCategory.UpdatedDate
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(GroupCategoryQueryListBox request, CancellationToken cancellationToken)
    {

        var groupCategorys = await _groupCategoryRepository.GetListListBox(request.Status, request.Keyword);
        var result = groupCategorys.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }

    public async Task<IEnumerable<GroupCategoryDto>> Handle(GroupCategoryQueryByListId request, CancellationToken cancellationToken)
    {
        var groupCategorys = await _groupCategoryRepository.GetByListId(request.ListId);
        var result = groupCategorys.Select(groupCategory => new GroupCategoryDto()
        {
            Id = groupCategory.Id,
            Code = groupCategory.Code,
            Name = groupCategory.Name,
            Title = groupCategory.Title,
            Description = groupCategory.Description,
            Image = groupCategory.Image,
            Logo = groupCategory.Logo,
            Logo2 = groupCategory.Logo2,
            Favicon = groupCategory.Favicon,
            Url = groupCategory.Url,
            Tags = groupCategory.Tags,
            Email = groupCategory.Email,
            Phone = groupCategory.Phone,
            Address = groupCategory.Address,
            Facebook = groupCategory.Facebook,
            Youtube = groupCategory.Youtube,
            Zalo = groupCategory.Zalo,
            Slug = groupCategory.Slug,
            DisplayOrder = groupCategory.DisplayOrder,
            Status = groupCategory.Status,
            CreatedBy = groupCategory.CreatedBy,
            CreatedDate = groupCategory.CreatedDate,
            UpdatedBy = groupCategory.UpdatedBy,
            UpdatedDate = groupCategory.UpdatedDate
        });
        return result;
    }
}
