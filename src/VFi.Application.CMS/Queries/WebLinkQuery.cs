using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;
using Consul;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Queries;

namespace VFi.Application.CMS.Queries;

public class WebLinkQueryByLevel : IQuery<IEnumerable<WebLinkListViewDto>>
{

    public WebLinkQueryByLevel(Int32 status, string groupCode, Int32 level, Int32 levelCount)
    {
        Level = level;
        LevelCount = levelCount;
        GroupCode = groupCode;
        Status = status;
    }
    public WebLinkQueryByLevel(Int32 status, string groupCode, String? parent, Int32? level, Int32? levelCount)
    {
        Level = level;
        LevelCount = levelCount;
        GroupCode = groupCode;
        Status = status;
        Parent = parent;
    }
    public WebLinkQueryByLevel(Int32 status, string groupCode, String? parent, Int32? level, Int32? levelCount, Int32? top)
    {
        Level = level;
        LevelCount = levelCount;
        GroupCode = groupCode;
        Status = status;
        Parent = parent;
        Top = top;
    }
    public WebLinkQueryByLevel(Int32? status, string groupCode, String? parent)
    {

        GroupCode = groupCode;
        Status = status;
        Parent = parent;
    }
    public String GroupCode { get; set; }
    public Int32? Level { get; set; }
    public Int32? LevelCount { get; set; }
    public Int32? Status { get; set; }
    public Int32? Top { get; set; }
    public String? Parent { get; set; }
}

public class WebLinkQueryByGroup : IQuery<IEnumerable<WebLinkListViewDto>>
{
    public WebLinkQueryByGroup(List<string> groupCode, int? status)
    {
        GroupCode = groupCode;
        Status = status;
    }

    public List<string> GroupCode { get; set; }
    public Int32? Status { get; set; }
}

public class WebLinkQueryBreadcrumb : IQuery<IEnumerable<WebLinkListViewDto>>
{
    public WebLinkQueryBreadcrumb(string group, string webLink)
    {
        Group = group;
        WebLink = webLink;
    }

    public string Group { get; set; }
    public string WebLink { get; set; }
}
public class WebLinkQueryAll : IQuery<IEnumerable<WebLinkDto>>
{
    public WebLinkQueryAll()
    {
    }
}

public class WebLinkQueryListBox : IQuery<IEnumerable<WebLinkListBoxDto>>
{
    public WebLinkQueryListBox(WebLinkQueryParams @params, string? keyword)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.GroupWebLinkId))
        {
            Filter.Add("groupId", @params.GroupWebLinkId);
        }
        if (!String.IsNullOrEmpty(@params.ParentWebLinkId))
        {
            Filter.Add("parentId", @params.ParentWebLinkId);
        }
    }
    public string? Keyword { get; set; }
    public Dictionary<string, object> Filter { get; set; }
}
public class WebLinkQueryCombobox : IQuery<IEnumerable<WebLinkComboboxDto>>
{
    public WebLinkQueryCombobox(WebLinkQueryParams @params, string? keyword)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.GroupWebLinkId))
        {
            Filter.Add("groupId", @params.GroupWebLinkId);
        }
        if (!String.IsNullOrEmpty(@params.ParentWebLinkId))
        {
            Filter.Add("parentId", @params.ParentWebLinkId);
        }
        else
        {
            Filter.Add("parentId", "null");
        }
    }
    public string? Keyword { get; set; }
    public Dictionary<string, object> Filter { get; set; }
}
public class WebLinkQueryCheckExist : IQuery<bool>
{

    public WebLinkQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class WebLinkQueryById : IQuery<WebLinkDto>
{
    public WebLinkQueryById()
    {
    }

    public WebLinkQueryById(Guid webLinkId)
    {
        WebLinkId = webLinkId;
    }

    public Guid WebLinkId { get; set; }
}
public class WebLinkQueryAllParent : IQuery<IEnumerable<WebLinkParentDto>>
{
    public WebLinkQueryAllParent()
    {
    }

    public WebLinkQueryAllParent(Guid webLinkId)
    {
        WebLinkId = webLinkId;
    }

    public Guid WebLinkId { get; set; }
}
public class WebLinkPagingQuery : ListQuery, IQuery<PagingResponse<WebLinkDto>>
{
    public WebLinkPagingQuery(string? keyword, WebLinkQueryParams @params, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.GroupWebLinkId))
        {
            Filter.Add("groupId", @params.GroupWebLinkId);
        }
        if (!String.IsNullOrEmpty(@params.ParentWebLinkId))
        {
            Filter.Add("parentId", @params.ParentWebLinkId);
        }
    }

    public WebLinkPagingQuery(string? keyword, WebLinkQueryParams @params, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.GroupWebLinkId))
        {
            Filter.Add("groupId", @params.GroupWebLinkId);
        }
        if (!String.IsNullOrEmpty(@params.ParentWebLinkId))
        {
            Filter.Add("parentId", @params.ParentWebLinkId);
        }
    }
    public string? Keyword { get; set; }
    public Dictionary<string, object> Filter { get; set; }
}
public class WebLinkSearchQuery : ListQuery, IQuery<PagingResponse<WebLinkDto>>
{
    public WebLinkSearchQuery(int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
    }

    public int? Status { get; set; }
    public string Group { get; set; }
    public string? Keyword { get; set; }
}
public class WebLinkQueryHandler : IQueryHandler<WebLinkQueryListBox, IEnumerable<WebLinkListBoxDto>>, IQueryHandler<WebLinkQueryBreadcrumb, IEnumerable<WebLinkListViewDto>>,
    IQueryHandler<WebLinkQueryByLevel, IEnumerable<WebLinkListViewDto>>,
     IQueryHandler<WebLinkQueryByGroup, IEnumerable<WebLinkListViewDto>>,
    IQueryHandler<WebLinkQueryCombobox, IEnumerable<WebLinkComboboxDto>>,
                                         IQueryHandler<WebLinkQueryCheckExist, bool>,
                                         IQueryHandler<WebLinkQueryById, WebLinkDto>,
                                         IQueryHandler<WebLinkQueryAllParent, IEnumerable<WebLinkParentDto>>,
                                         IQueryHandler<WebLinkPagingQuery, PagingResponse<WebLinkDto>>,
                                         IQueryHandler<WebLinkSearchQuery, PagingResponse<WebLinkDto>>
{
    private readonly IWebLinkRepository _webLinkRepository;
    public WebLinkQueryHandler(IWebLinkRepository webLinkRespository)
    {
        _webLinkRepository = webLinkRespository;
    }
    public async Task<bool> Handle(WebLinkQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _webLinkRepository.CheckExistById(request.Id);
    }
    public async Task<IEnumerable<WebLinkListViewDto>> Handle(WebLinkQueryByLevel request, CancellationToken cancellationToken)
    {


        var filter = new Dictionary<string, object>();
        filter.Add("status", request.Status);
        Guid group = Guid.Empty;
        if (!Guid.TryParse(request.GroupCode, out group))
        {
            filter.Add("group", request.GroupCode);
        }
        else
        {
            filter.Add("groupid", request.GroupCode);
        }
        // filter.Add("group", request.GroupCode);  
        if (request.Level.HasValue)
            filter.Add("level", request.Level);
        if (request.LevelCount.HasValue)
            filter.Add("levelCount", request.LevelCount);
        if (request.Top.HasValue)
            filter.Add("top", request.Top);
        var webLinks = await _webLinkRepository.Filter("", filter);
        var list = webLinks.Select(c => new WebLinkListViewDto()
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            FullName = c.FullName,
            Description = c.Description,
            Url = c.Url,
            Image = c.Image,
            Image2 = c.Image2,
            Image3 = c.Image3,
            ParentWebLinkId = c.ParentWebLinkId,
            ParentIds = c.ParentIds,
            GroupWebLinkId = c.GroupWebLinkId,
            GroupWebLinkCode = c.GroupWebLinkCode,
            GroupWebLinkName = c.GroupWebLinkName,
            DisplayOrder = c.DisplayOrder,
            Level = c.Level,
            Status = c.Status
        });

        return list;
    }
    public async Task<WebLinkDto> Handle(WebLinkQueryById request, CancellationToken cancellationToken)
    {
        var webLink = await _webLinkRepository.GetById(request.WebLinkId);

        var parentWebLink = webLink.ParentWebLinkId != null ? await _webLinkRepository.GetById((Guid)webLink.ParentWebLinkId) : null;
        var result = new WebLinkDto()
        {
            Id = webLink.Id,
            Code = webLink.Code,
            Name = webLink.Name,
            FullName = webLink.FullName,
            Url = webLink.Url,
            Image = webLink.Image,
            Image2 = webLink.Image2,
            Image3 = webLink.Image3,
            ParentIds = webLink.ParentIds,
            Level = webLink.Level,
            GroupWebLinkCode = webLink.GroupWebLinkCode,
            GroupWebLinkName = webLink.GroupWebLinkName,
            Description = webLink.Description,
            GroupWebLinkId = webLink.GroupWebLinkId,
            ParentWebLinkId = webLink.ParentWebLinkId,
            ParentWebLinkName = parentWebLink != null ? parentWebLink.Name : "",
            DisplayOrder = webLink.DisplayOrder,
            Status = webLink.Status,
            Keywords = webLink.Keywords,
            CreatedBy = webLink.CreatedBy,
            CreatedDate = webLink.CreatedDate,
            CreatedByName = webLink.CreatedByName,
            UpdatedBy = webLink.UpdatedBy,
            UpdatedDate = webLink.UpdatedDate,
            UpdatedByName = webLink.UpdatedByName
        };
        return result;
    }
    public async Task<IEnumerable<WebLinkParentDto>> Handle(WebLinkQueryAllParent request, CancellationToken cancellationToken)
    {
        List<WebLinkParentDto> List = new List<WebLinkParentDto>();
        var listP = await RecursionParentId(List, request.WebLinkId);
        return listP;
    }
    private async Task<List<WebLinkParentDto>> RecursionParentId(List<WebLinkParentDto> List, Guid? id)
    {
        if (id != null)
        {
            var webLink = await _webLinkRepository.GetById((Guid)id);
            List.Add(new WebLinkParentDto { Id = webLink.Id, Name = webLink.Name });
            await RecursionParentId(List, webLink.ParentWebLinkId);
        }
        return List;
    }
    public async Task<PagingResponse<WebLinkDto>> Handle(WebLinkPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<WebLinkDto>();
        var count = await _webLinkRepository.FilterCount(request.Keyword, request.Filter);
        var webLinks = await _webLinkRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
        var data = webLinks.Select(webLink => new WebLinkDto()
        {
            Id = webLink.Id,
            Code = webLink.Code,
            Name = webLink.Name,
            Url = webLink.Url,
            Image = webLink.Image,
            Image2 = webLink.Image2,
            Image3 = webLink.Image3,
            Description = webLink.Description,
            ParentWebLinkId = webLink.ParentWebLinkId,
            GroupWebLinkCode = webLink.GroupWebLinkCode,
            GroupWebLinkName = webLink.GroupWebLinkName,
            Status = webLink.Status,
            CreatedByName = webLink.CreatedByName,
            UpdatedByName = webLink.UpdatedByName,
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<WebLinkListBoxDto>> Handle(WebLinkQueryListBox request, CancellationToken cancellationToken)
    {

        var webLinks = await _webLinkRepository.GetListListBox(request.Filter, request.Keyword);
        var result = webLinks.Select(data => new WebLinkListBoxDto()
        {
            Value = data.Id,
            Label = data.Name,
            Key = data.Code,
            ParentWebLinkId = data.ParentWebLinkId,
            DisplayOrder = data.DisplayOrder
        });
        return result;
    }
    public async Task<IEnumerable<WebLinkComboboxDto>> Handle(WebLinkQueryCombobox request, CancellationToken cancellationToken)
    {

        var webLinks = await _webLinkRepository.GetCombobox(request.Filter, request.Keyword);
        var result = webLinks.Select(data => new WebLinkComboboxDto()
        {
            Value = data.Id,
            Label = data.FullName,
            Key = data.Code,
            ParentWebLinkId = data.ParentWebLinkId,
        });
        return result;
    }

    public async Task<IEnumerable<WebLinkListViewDto>> Handle(WebLinkQueryBreadcrumb request, CancellationToken cancellationToken)
    {
        var webLinks = await _webLinkRepository.GetBreadcrumb(request.Group, request.WebLink);
        var list = webLinks.Select(c => new WebLinkListViewDto()
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            FullName = c.FullName,
            Url = c.Url,
            Image = c.Image,
            Image2 = c.Image2,
            Image3 = c.Image3,
            ParentWebLinkId = c.ParentWebLinkId,
            ParentIds = c.ParentIds,
            GroupWebLinkId = c.GroupWebLinkId,
            GroupWebLinkCode = c.GroupWebLinkCode,
            GroupWebLinkName = c.GroupWebLinkName,
            DisplayOrder = c.DisplayOrder,
            Level = c.Level,
            Status = c.Status
        });

        return list;
    }

    public async Task<IEnumerable<WebLinkListViewDto>> Handle(WebLinkQueryByGroup request, CancellationToken cancellationToken)
    {
        var webLinks = await _webLinkRepository.GetByGroup(request.GroupCode, request.Status);
        var list = webLinks.Select(c => new WebLinkListViewDto()
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            FullName = c.FullName,
            Url = c.Url,
            Image = c.Image,
            Image2 = c.Image2,
            Image3 = c.Image3,
            ParentWebLinkId = c.ParentWebLinkId,
            ParentIds = c.ParentIds,
            GroupWebLinkId = c.GroupWebLinkId,
            GroupWebLinkCode = c.GroupWebLinkCode,
            GroupWebLinkName = c.GroupWebLinkName,
            DisplayOrder = c.DisplayOrder,
            Level = c.Level,
            Status = c.Status
        });

        return list;
    }

    public async Task<PagingResponse<WebLinkDto>> Handle(WebLinkSearchQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<WebLinkDto>();
        var filter = new Dictionary<string, object>();
        filter.Add("group", request.Group);
        if (request.Status.HasValue)
            filter.Add("status", request.Status);
        var count = await _webLinkRepository.FilterCount(request.Keyword, filter);
        var webLinks = await _webLinkRepository.Filter(request.Keyword, filter, request.PageSize, request.PageIndex);
        var data = webLinks.Select(webLink => new WebLinkDto()
        {
            Id = webLink.Id,
            Code = webLink.Code,
            Name = webLink.Name,
            Url = webLink.Url,
            Image = webLink.Image,
            Image2 = webLink.Image2,
            Image3 = webLink.Image3,
            Description = webLink.Description,
            ParentWebLinkId = webLink.ParentWebLinkId,
            GroupWebLinkCode = webLink.GroupWebLinkCode,
            GroupWebLinkName = webLink.GroupWebLinkName,
            Status = webLink.Status,
            CreatedByName = webLink.CreatedByName,
            UpdatedByName = webLink.UpdatedByName,
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
