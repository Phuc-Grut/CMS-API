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
using VFi.Infra.CMS.Repository;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Application.CMS.Queries;

public class CategoryRootQueryByLevel : IQuery<IEnumerable<CategoryRootListViewDto>>
{

    public CategoryRootQueryByLevel(Int32 status, Int32 level, Int32 levelCount)
    {
        Level = level;
        LevelCount = levelCount;
        Status = status;
    }
    public CategoryRootQueryByLevel(Int32 status, String? parent, Int32? level, Int32? levelCount)
    {
        Level = level;
        LevelCount = levelCount;
        Status = status;
        Parent = parent;
    }
    public CategoryRootQueryByLevel(Int32? status, String? parent)
    {

        Status = status;
        Parent = parent;
    }
    public Int32? Level { get; set; }
    public Int32? LevelCount { get; set; }
    public Int32? Status { get; set; }
    public String? Parent { get; set; }
}
public class CategoryRootQueryListBox : IQuery<IEnumerable<CategoryRootListBoxDto>>
{
    public CategoryRootQueryListBox(CategoryRootQueryParams @params, string? keyword)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.ParentCategoryRootId))
        {
            Filter.Add("parentId", @params.ParentCategoryRootId);
        }
    }
    public string? Keyword { get; set; }
    public Dictionary<string, object> Filter { get; set; }
}
public class CategoryRootQueryBreadcrumb : IQuery<IEnumerable<CategoryRootListViewDto>>
{
    public CategoryRootQueryBreadcrumb(string category)
    {
        Category = category;
    }

    public string Category { get; set; }
}
public class CategoryRootQueryAll : IQuery<IEnumerable<CategoryRootDto>>
{
    public CategoryRootQueryAll()
    {
    }
}

public class CategoryRootQueryCheckExist : IQuery<bool>
{

    public CategoryRootQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class CategoryRootQueryById : IQuery<CategoryRootDto>
{
    public CategoryRootQueryById()
    {
    }

    public CategoryRootQueryById(Guid categoryId)
    {
        CategoryRootId = categoryId;
    }

    public Guid CategoryRootId { get; set; }
}
public class CategoryRootQueryBySlug : IQuery<CategoryRootDto>
{
    public CategoryRootQueryBySlug()
    {
    }

    public CategoryRootQueryBySlug(string slug)
    {
        Slug = slug;
    }

    public string Slug { get; set; }
}
public class CategoryRootQueryAllParent : IQuery<IEnumerable<CategoryRootParentDto>>
{
    public CategoryRootQueryAllParent()
    {
    }

    public CategoryRootQueryAllParent(Guid categoryId)
    {
        CategoryRootId = categoryId;
    }

    public Guid CategoryRootId { get; set; }
}
public class CategoryRootPagingQuery : ListQuery, IQuery<PagingResponse<CategoryRootDto>>
{
    public CategoryRootPagingQuery(string? keyword, CategoryRootQueryParams @params, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.ParentCategoryRootId))
        {
            Filter.Add("parentId", @params.ParentCategoryRootId);
        }
    }

    public CategoryRootPagingQuery(string? keyword, CategoryQueryParams @params, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.ParentCategoryId))
        {
            Filter.Add("parentId", @params.ParentCategoryId);
        }
    }

    public string? Keyword { get; set; }
    public Dictionary<string, object> Filter { get; set; }
}
public class CategoryRootQueryCombobox : IQuery<IEnumerable<CategoryRootComboboxDto>>
{
    public CategoryRootQueryCombobox()
    {
    }

}
public class CategoryRootQueryHandler : IQueryHandler<CategoryRootQueryListBox, IEnumerable<CategoryRootListBoxDto>>,
                                         IQueryHandler<CategoryRootQueryByLevel, IEnumerable<CategoryRootListViewDto>>,
                                         IQueryHandler<CategoryRootQueryCheckExist, bool>,
                                         IQueryHandler<CategoryRootQueryById, CategoryRootDto>, IQueryHandler<CategoryRootQueryBySlug, CategoryRootDto>,
                                         IQueryHandler<CategoryRootQueryAllParent, IEnumerable<CategoryRootParentDto>>,
                                         IQueryHandler<CategoryRootPagingQuery, PagingResponse<CategoryRootDto>>,
                                         IQueryHandler<CategoryRootQueryCombobox, IEnumerable<CategoryRootComboboxDto>>

{
    private readonly ICategoryRootRepository _categoryRootRepository;
    public CategoryRootQueryHandler(ICategoryRootRepository categoryRootRespository)
    {
        _categoryRootRepository = categoryRootRespository;
    }
    public async Task<bool> Handle(CategoryRootQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _categoryRootRepository.CheckExistById(request.Id);
    }
    public async Task<IEnumerable<CategoryRootListViewDto>> Handle(CategoryRootQueryByLevel request, CancellationToken cancellationToken)
    {


        var filter = new Dictionary<string, object>();
        filter.Add("status", request.Status);
        filter.Add("parent", request.Parent);
        filter.Add("level", request.Level);
        filter.Add("levelCount", request.LevelCount);

        var CategoryRoots = await _categoryRootRepository.Filter("", filter);
        var list = CategoryRoots.Select(c => new CategoryRootListViewDto()
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            Image1 = c.Image1,
            Image2 = c.Image2,
            FullName = c.FullName,
            Url = c.Url,
            Image = c.Image,
            Slug = c.Slug,
            ParentCategoryId = c.ParentCategoryId,
            ParentIds = c.ParentIds,
            DisplayOrder = c.DisplayOrder,
            JsonData = c.JsonData,
            Level = c.Level,
            Status = c.Status
        });

        return list;
    }
    public async Task<CategoryRootDto> Handle(CategoryRootQueryById request, CancellationToken cancellationToken)
    {

        var categoryRoot = await _categoryRootRepository.GetById(request.CategoryRootId);
        var parentCategory = categoryRoot.ParentCategoryId != null ? await _categoryRootRepository.GetById((Guid)categoryRoot.ParentCategoryId) : null;
        var result = new CategoryRootDto()
        {
            Id = categoryRoot.Id,
            Code = categoryRoot.Code,
            Name = categoryRoot.Name,
            FullName = categoryRoot.FullName,
            Url = categoryRoot.Url,
            Image = categoryRoot.Image,
            Image1 = categoryRoot.Image1,
            Image2 = categoryRoot.Image2,
            Slug = categoryRoot.Slug,
            ParentIds = categoryRoot.ParentIds,
            Description = categoryRoot.Description,
            ParentCategoryId = categoryRoot.ParentCategoryId,
            ParentCategoryName = parentCategory != null ? parentCategory.Name : "",
            DisplayOrder = categoryRoot.DisplayOrder,
            Status = categoryRoot.Status,
            Keywords = categoryRoot.Keywords,
            JsonData = categoryRoot.JsonData,
            CreatedBy = categoryRoot.CreatedBy,
            CreatedDate = categoryRoot.CreatedDate,
            CreatedByName = categoryRoot.CreatedByName,
            UpdatedBy = categoryRoot.UpdatedBy,
            UpdatedDate = categoryRoot.UpdatedDate,
            UpdatedByName = categoryRoot.UpdatedByName
        };
        return result;
    }
    public async Task<CategoryRootDto> Handle(CategoryRootQueryBySlug request, CancellationToken cancellationToken)
    {

        var categoryRoot = await _categoryRootRepository.GetBySlug(request.Slug);
        var parentCategory = categoryRoot.ParentCategoryId != null ? await _categoryRootRepository.GetById((Guid)categoryRoot.ParentCategoryId) : null;
        var result = new CategoryRootDto()
        {
            Id = categoryRoot.Id,
            Code = categoryRoot.Code,
            Name = categoryRoot.Name,
            FullName = categoryRoot.FullName,
            Url = categoryRoot.Url,
            Image = categoryRoot.Image,
            Image1 = categoryRoot.Image1,
            Image2 = categoryRoot.Image2,
            Slug = categoryRoot.Slug,
            ParentIds = categoryRoot.ParentIds,
            Description = categoryRoot.Description,
            ParentCategoryId = categoryRoot.ParentCategoryId,
            ParentCategoryName = parentCategory != null ? parentCategory.Name : "",
            DisplayOrder = categoryRoot.DisplayOrder,
            Status = categoryRoot.Status,
            Keywords = categoryRoot.Keywords,
            JsonData = categoryRoot.JsonData,
            CreatedBy = categoryRoot.CreatedBy,
            CreatedDate = categoryRoot.CreatedDate,
            CreatedByName = categoryRoot.CreatedByName,
            UpdatedBy = categoryRoot.UpdatedBy,
            UpdatedDate = categoryRoot.UpdatedDate,
            UpdatedByName = categoryRoot.UpdatedByName
        };
        return result;
    }
    public async Task<IEnumerable<CategoryRootParentDto>> Handle(CategoryRootQueryAllParent request, CancellationToken cancellationToken)
    {
        List<CategoryRootParentDto> List = new List<CategoryRootParentDto>();
        var listP = await RecursionParentId(List, request.CategoryRootId);
        return listP;
    }
    public async Task<IEnumerable<CategoryRootListBoxDto>> Handle(CategoryRootQueryListBox request, CancellationToken cancellationToken)
    {

        var categorys = await _categoryRootRepository.GetListListBox(request.Filter, request.Keyword);
        var result = categorys.Select(data => new CategoryRootListBoxDto()
        {
            Value = data.Id,
            Label = data.Name,
            Key = data.Code,
            ParentCategoryId = data.ParentCategoryId,
            DisplayOrder = data.DisplayOrder
        });
        return result;
    }
    private async Task<List<CategoryRootParentDto>> RecursionParentId(List<CategoryRootParentDto> List, Guid? id)
    {
        if (id != null)
        {
            var CategoryRoot = await _categoryRootRepository.GetById((Guid)id);
            List.Add(new CategoryRootParentDto { Id = CategoryRoot.Id, Name = CategoryRoot.Name });
            await RecursionParentId(List, CategoryRoot.ParentCategoryId);
        }
        return List;
    }
    public async Task<PagingResponse<CategoryRootDto>> Handle(CategoryRootPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<CategoryRootDto>();
        var count = await _categoryRootRepository.FilterCount(request.Keyword, request.Filter);
        var categoryRoots = await _categoryRootRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
        var data = categoryRoots.Select(categoryRoot => new CategoryRootDto()
        {
            Id = categoryRoot.Id,
            Code = categoryRoot.Code,
            Name = categoryRoot.Name,
            FullName = categoryRoot.FullName ?? categoryRoot.Name,
            Image = categoryRoot.Image,
            Url = categoryRoot.Url,
            Image1 = categoryRoot.Image1,
            Image2 = categoryRoot.Image2,
            Slug = categoryRoot.Slug,
            Description = categoryRoot.Description,
            ParentCategoryId = categoryRoot.ParentCategoryId,
            Status = categoryRoot.Status,
            CreatedByName = categoryRoot.CreatedByName,
            UpdatedByName = categoryRoot.UpdatedByName
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<CategoryRootComboboxDto>> Handle(CategoryRootQueryCombobox request, CancellationToken cancellationToken)
    {
        var datas = await _categoryRootRepository.GetAll();
        var datatree = datas.Select(data => new CategoryRootComboboxDto()
        {
            Value = data.Id,
            Name = data.Name,
            SortOrder = data.DisplayOrder,
            ParentCategoryRootId = data.ParentCategoryId,
        });
        var result = datatree.Count() > 0 ? BuildTreeCombobox(datatree) : datatree;
        return result;
    }
    private IEnumerable<CategoryRootComboboxDto> BuildTreeCombobox(IEnumerable<CategoryRootComboboxDto> resource)
    {
        var groups = resource.GroupBy(i => i.ParentCategoryRootId);

        var roots = groups.FirstOrDefault(x => x.Key == null)
                          .OrderBy(x => x.SortOrder)
                          .ToList();
        if (roots.Count > 0)
        {
            var dict = groups.Where(g => g.Key != null)
                             .ToDictionary(g => g.Key, g => g.OrderBy(x => x.SortOrder)
                             .ToList());

            foreach (var item in roots)
            {
                AddChildrenCombobox(item, dict);
            }
        }
        return roots;
    }
    private void AddChildrenCombobox(CategoryRootComboboxDto node, IDictionary<Guid?, List<CategoryRootComboboxDto>> source)
    {
        if (source.ContainsKey(node.Value))
        {
            node.Children = source[node.Value];
            node.Expanded = true;
            foreach (var item in node.Children)
            {
                AddChildrenCombobox(item, source);
            }
        }
        else
        {
            node.Children = new List<CategoryRootComboboxDto>();
        }
    }
}
