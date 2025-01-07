using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Queries;

namespace VFi.Application.CMS.Queries;

public class ItemQueryById : IQuery<ItemDto>
{
    public ItemQueryById()
    {
    }

    public ItemQueryById(Guid itemId)
    {
        ItemId = itemId;
    }

    public Guid ItemId { get; set; }
}
public class ItemQueryByPath : IQuery<ItemDto>
{
    public ItemQueryByPath()
    {
    }

    public ItemQueryByPath(string itemPath)
    {
        ItemPath = itemPath;
    }

    public string ItemPath { get; set; }
}
public class ItemQueryCheckExist : IQuery<bool>
{

    public ItemQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class ItemPagingQuery : ListQuery, IQuery<PagingResponse<ItemDto>>
{
    public ItemPagingQuery(string? keyword, ItemQueryParams itemQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        QueryParams = itemQueryParams;
    }

    public ItemPagingQuery(string? keyword, ItemQueryParams itemQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
    {
        Keyword = keyword;
        QueryParams = itemQueryParams;
    }

    public string? Keyword { get; set; }
    public ItemQueryParams QueryParams { get; set; }

}

public class ItemQueryAllParent : IQuery<IEnumerable<ItemParentDto>>
{
    public ItemQueryAllParent()
    {
    }

    public ItemQueryAllParent(Guid itemId)
    {
        Id = itemId;
    }

    public Guid Id { get; set; }
}
public class ItemQueryHandler : IQueryHandler<ItemQueryById, ItemDto>, IQueryHandler<ItemQueryByPath, ItemDto>,
                                IQueryHandler<ItemQueryCheckExist, bool>,
                                IQueryHandler<ItemPagingQuery, PagingResponse<ItemDto>>,
                                IQueryHandler<ItemQueryAllParent, IEnumerable<ItemParentDto>>

{
    private readonly IItemRepository _itemRepository;
    public ItemQueryHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    public async Task<ItemDto> Handle(ItemQueryById request, CancellationToken cancellationToken)

    {
        var item = await _itemRepository.GetById(request.ItemId);

        var result = new ItemDto()
        {
            Id = item.Id,
            Name = item.Name,
            Title = item.Title,
            Description = item.Description,
            Size = item.Size,
            IsFile = item.IsFile,
            ParentId = item.ParentId,
            MimeType = item.MimeType,
            HasChild = item.HasChild,
            LocalPath = item.LocalPath,
            Cdn = item.Cdn,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            Product = item.Product,
            Status = item.Status,
            Workspace = item.Workspace,
            Content = item.Content,
            Tenant = item.Tenant,
        };
        return result;
    }
    public async Task<bool> Handle(ItemQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _itemRepository.CheckExistById(request.Id);
    }
    public async Task<PagingResponse<ItemDto>> Handle(ItemPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<ItemDto>();
        var filter = new Dictionary<string, object>();
        if (request.QueryParams.Status != null)
        {
            filter.Add("status", request.QueryParams.Status);
        }
        if (!String.IsNullOrEmpty(request.QueryParams.Product))
        {
            filter.Add("product", request.QueryParams.Product);
        }
        if (request.QueryParams.IsFile != null)
        {
            filter.Add("isFile", request.QueryParams.IsFile);
        }
        if (!String.IsNullOrEmpty(request.QueryParams.ParentId))
        {
            filter.Add("parentId", request.QueryParams.ParentId);
        }
        var count = await _itemRepository.FilterCount(request.Keyword, filter);
        var items = await _itemRepository.Filter(request.Keyword, filter, request.PageSize, request.PageIndex);
        var data = items.Select(item => new ItemDto()
        {
            Id = item.Id,
            Name = item.Name,
            Title = item.Title,
            Description = item.Description,
            Size = item.Size,
            IsFile = item.IsFile,
            ParentId = item.ParentId,
            MimeType = item.MimeType,
            HasChild = item.HasChild,
            LocalPath = item.LocalPath,
            Cdn = item.Cdn,
            Product = item.Product,
            Status = item.Status,
            Workspace = item.Workspace,
            Content = item.Content,
            Tenant = item.Tenant,
            CreatedByName = item.CreatedByName,
            UpdatedByName = item.UpdatedByName,
            UpdatedDate = item.UpdatedDate,
            LastOpenDate = item.LastOpenDate
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<ItemParentDto>> Handle(ItemQueryAllParent request, CancellationToken cancellationToken)
    {
        List<ItemParentDto> List = new List<ItemParentDto>();
        var listP = await RecursionParentId(List, request.Id);
        return listP;
    }

    public async Task<ItemDto> Handle(ItemQueryByPath request, CancellationToken cancellationToken)
    {


        var item = await _itemRepository.GetByPath(request.ItemPath);

        var result = new ItemDto()
        {
            Id = item.Id,
            Name = item.Name,
            Title = item.Title,
            Description = item.Description,
            Size = item.Size,
            IsFile = item.IsFile,
            ParentId = item.ParentId,
            MimeType = item.MimeType,
            HasChild = item.HasChild,
            VirtualPath = item.VirtualPath,
            LocalPath = item.LocalPath,
            Cdn = item.Cdn,
            CreatedBy = item.CreatedBy,
            CreatedDate = item.CreatedDate,
            UpdatedBy = item.UpdatedBy,
            UpdatedDate = item.UpdatedDate,
            Product = item.Product,
            Status = item.Status,
            Workspace = item.Workspace,
            Content = item.Content,
            Tenant = item.Tenant,
        };
        return result;
    }

    private async Task<List<ItemParentDto>> RecursionParentId(List<ItemParentDto> List, Guid? id)
    {
        if (id != null)
        {
            var item = await _itemRepository.GetById((Guid)id);
            List.Add(new ItemParentDto { Id = item.Id, Title = item.Title });
            await RecursionParentId(List, item.ParentId);
        }
        return List;
    }
}
