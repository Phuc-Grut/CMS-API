using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Queries;

namespace VFi.Application.CMS.Queries;


public class GroupWebLinkQueryAll : IQuery<IEnumerable<GroupWebLinkDto>>
{
    public GroupWebLinkQueryAll()
    {
    }
}

public class GroupWebLinkQueryListBox : IQuery<IEnumerable<ComboBoxDto>>
{
    public GroupWebLinkQueryListBox(int? status, string? keyword)
    {
        Status = status;
        Keyword = keyword;
    }
    public int? Status { get; set; }
    public string? Keyword { get; set; }
}
public class GroupWebLinkQueryCheckExist : IQuery<bool>
{

    public GroupWebLinkQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class GroupWebLinkQueryById : IQuery<GroupWebLinkDto>
{
    public GroupWebLinkQueryById()
    {
    }

    public GroupWebLinkQueryById(Guid groupWebLinkId)
    {
        GroupWebLinkId = groupWebLinkId;
    }

    public Guid GroupWebLinkId { get; set; }
}
public class GroupWebLinkQueryByCode : IQuery<GroupWebLinkDto>
{
    public GroupWebLinkQueryByCode()
    {
    }

    public GroupWebLinkQueryByCode(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}
public class GroupWebLinkPagingQuery : ListQuery, IQuery<PagingResponse<GroupWebLinkDto>>
{
    public GroupWebLinkPagingQuery(string? keyword, int? status, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (status != null)
        {
            Filter.Add("status", status);
        }
    }

    public GroupWebLinkPagingQuery(string? keyword, int? status, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
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

public class GroupWebLinkQueryHandler : IQueryHandler<GroupWebLinkQueryListBox, IEnumerable<ComboBoxDto>>,
                                         IQueryHandler<GroupWebLinkQueryAll, IEnumerable<GroupWebLinkDto>>,
                                         IQueryHandler<GroupWebLinkQueryCheckExist, bool>,
                                         IQueryHandler<GroupWebLinkQueryById, GroupWebLinkDto>,
                                         IQueryHandler<GroupWebLinkQueryByCode, GroupWebLinkDto>,
                                         IQueryHandler<GroupWebLinkPagingQuery, PagingResponse<GroupWebLinkDto>>
{
    private readonly IGroupWebLinkRepository _groupWebLinkRepository;
    public GroupWebLinkQueryHandler(IGroupWebLinkRepository groupWebLinkRespository)
    {
        _groupWebLinkRepository = groupWebLinkRespository;
    }
    public async Task<bool> Handle(GroupWebLinkQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _groupWebLinkRepository.CheckExistById(request.Id);

    }

    public async Task<GroupWebLinkDto> Handle(GroupWebLinkQueryById request, CancellationToken cancellationToken)
    {
        var groupWebLink = await _groupWebLinkRepository.GetById(request.GroupWebLinkId);
        var result = new GroupWebLinkDto()
        {
            Id = groupWebLink.Id,
            Code = groupWebLink.Code,
            Name = groupWebLink.Name,
            Title = groupWebLink.Title,
            Description = groupWebLink.Description,
            Image = groupWebLink.Image,
            Image1 = groupWebLink.Image1,
            Image2 = groupWebLink.Image2,
            Url = groupWebLink.Url,
            DisplayOrder = groupWebLink.DisplayOrder,
            Status = groupWebLink.Status,
            CreatedBy = groupWebLink.CreatedBy,
            CreatedDate = groupWebLink.CreatedDate,
            UpdatedBy = groupWebLink.UpdatedBy,
            UpdatedDate = groupWebLink.UpdatedDate
        };
        return result;
    }
    public async Task<GroupWebLinkDto> Handle(GroupWebLinkQueryByCode request, CancellationToken cancellationToken)
    {
        var groupWebLink = await _groupWebLinkRepository.GetByCode(request.Code);
        var result = new GroupWebLinkDto()
        {
            Id = groupWebLink.Id,
            Code = groupWebLink.Code,
            Name = groupWebLink.Name,
            Title = groupWebLink.Title,
            Description = groupWebLink.Description,
            Image = groupWebLink.Image,
            Image1 = groupWebLink.Image1,
            Url = groupWebLink.Url,
            DisplayOrder = groupWebLink.DisplayOrder,
            Status = groupWebLink.Status,
            CreatedBy = groupWebLink.CreatedBy,
            CreatedDate = groupWebLink.CreatedDate,
            UpdatedBy = groupWebLink.UpdatedBy,
            UpdatedDate = groupWebLink.UpdatedDate,
            CreatedByName = groupWebLink.CreatedByName,
            UpdatedByName = groupWebLink.UpdatedByName
        };
        return result;
    }
    public async Task<PagingResponse<GroupWebLinkDto>> Handle(GroupWebLinkPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<GroupWebLinkDto>();
        var count = await _groupWebLinkRepository.FilterCount(request.Keyword, request.Filter);
        var groupWebLinks = await _groupWebLinkRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
        var data = groupWebLinks.Select(groupWebLink => new GroupWebLinkDto()
        {
            Id = groupWebLink.Id,
            Code = groupWebLink.Code,
            Name = groupWebLink.Name,
            Title = groupWebLink.Title,
            Description = groupWebLink.Description,
            Image = groupWebLink.Image,
            Image1 = groupWebLink.Image1,
            Image2 = groupWebLink.Image2,
            Url = groupWebLink.Url,
            DisplayOrder = groupWebLink.DisplayOrder,
            Status = groupWebLink.Status,
            CreatedBy = groupWebLink.CreatedBy,
            CreatedDate = groupWebLink.CreatedDate,
            UpdatedBy = groupWebLink.UpdatedBy,
            UpdatedDate = groupWebLink.UpdatedDate
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<GroupWebLinkDto>> Handle(GroupWebLinkQueryAll request, CancellationToken cancellationToken)
    {
        var groupWebLinks = await _groupWebLinkRepository.GetAll();
        var result = groupWebLinks.Select(groupWebLink => new GroupWebLinkDto()
        {
            Id = groupWebLink.Id,
            Code = groupWebLink.Code,
            Name = groupWebLink.Name,
            Title = groupWebLink.Title,
            Description = groupWebLink.Description,
            Image = groupWebLink.Image,
            Image1 = groupWebLink.Image1,
            Image2 = groupWebLink.Image2,
            Url = groupWebLink.Url,
            DisplayOrder = groupWebLink.DisplayOrder,
            Status = groupWebLink.Status,
            CreatedBy = groupWebLink.CreatedBy,
            CreatedDate = groupWebLink.CreatedDate,
            UpdatedBy = groupWebLink.UpdatedBy,
            UpdatedDate = groupWebLink.UpdatedDate
        });
        return result;
    }

    public async Task<IEnumerable<ComboBoxDto>> Handle(GroupWebLinkQueryListBox request, CancellationToken cancellationToken)
    {

        var groupWebLinks = await _groupWebLinkRepository.GetListListBox(request.Status, request.Keyword);
        var result = groupWebLinks.Select(x => new ComboBoxDto()
        {
            Key = x.Code,
            Value = x.Id,
            Label = x.Name
        });
        return result;
    }
}
