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

public class CategoryQueryByLevel : IQuery<IEnumerable<CategoryListViewDto>>
{

    public CategoryQueryByLevel(Int32 status, string groupCode, Int32 level, Int32 levelCount)
    {
        Level = level;
        LevelCount = levelCount;
        GroupCode = groupCode;
        Status = status;
    }
    public CategoryQueryByLevel(Int32 status, string groupCode, String? parent, Int32? level, Int32? levelCount)
    {
        Level = level;
        LevelCount = levelCount;
        GroupCode = groupCode;
        Status = status;
        Parent = parent;
    }
    public CategoryQueryByLevel(Int32? status, string groupCode, String? parent)
    {

        GroupCode = groupCode;
        Status = status;
        Parent = parent;
    }
    public String GroupCode { get; set; }
    public Int32? Level { get; set; }
    public Int32? LevelCount { get; set; }
    public Int32? Status { get; set; }
    public String? Parent { get; set; }
}
public class CategoryQueryBreadcrumb : IQuery<IEnumerable<CategoryListViewDto>>
{
    public CategoryQueryBreadcrumb(string group, string category)
    {
        Group = group;
        Category = category;
    }

    public string Group { get; set; }
    public string Category { get; set; }
}
public class CategoryQueryAll : IQuery<IEnumerable<CategoryDto>>
{
    public CategoryQueryAll()
    {
    }
}
public class CategoryQueryByListId : IQuery<IEnumerable<CategoryListViewDto>>
{
    public CategoryQueryByListId()
    {
    }
    public List<Guid> ListId { get; set; }
}
public class CategoryQueryListBox : IQuery<IEnumerable<CategoryListBoxDto>>
{
    public CategoryQueryListBox(CategoryQueryParams @params, string? keyword)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.GroupCategoryId))
        {
            Filter.Add("groupId", @params.GroupCategoryId);
        }
        if (!String.IsNullOrEmpty(@params.ParentCategoryId))
        {
            Filter.Add("parentId", @params.ParentCategoryId);
        }
    }
    public string? Keyword { get; set; }
    public Dictionary<string, object> Filter { get; set; }
}
public class CategoryQueryCombobox : IQuery<IEnumerable<CategoryComboboxDto>>
{
    public CategoryQueryCombobox(CategoryQueryParams @params, string? keyword)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.GroupCategoryId))
        {
            Filter.Add("groupId", @params.GroupCategoryId);
        }
        if (!String.IsNullOrEmpty(@params.ParentCategoryId))
        {
            Filter.Add("parentId", @params.ParentCategoryId);
        }
        else
        {
            Filter.Add("parentId", "null");
        }
    }
    public string? Keyword { get; set; }
    public Dictionary<string, object> Filter { get; set; }
}
public class CategoryByChannelQueryCbx : IQuery<IEnumerable<CategoryComboboxDto>>
{
    public CategoryByChannelQueryCbx()
    { }
    public string Channel { get; set; }
    public string? Keyword { get; set; }
    public int? Status { get; set; }
}
public class CategoryQueryCheckExist : IQuery<bool>
{

    public CategoryQueryCheckExist(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; set; }
}
public class CategoryQueryById : IQuery<CategoryDto>
{
    public CategoryQueryById()
    {
    }

    public CategoryQueryById(Guid categoryId)
    {
        CategoryId = categoryId;
    }

    public Guid CategoryId { get; set; }
}
public class CategoryQueryBySlug : IQuery<CategoryDto>
{
    public CategoryQueryBySlug()
    {
    }

    public CategoryQueryBySlug(string channel, string slug)
    {
        Channel = channel;
        Slug = slug;
    }

    public string Channel { get; set; }
    public string Slug { get; set; }

}
public class CategoryQueryAllParent : IQuery<IEnumerable<CategoryParentDto>>
{
    public CategoryQueryAllParent()
    {
    }

    public CategoryQueryAllParent(Guid categoryId)
    {
        CategoryId = categoryId;
    }

    public Guid CategoryId { get; set; }
}
public class CategoryPagingQuery : ListQuery, IQuery<PagingResponse<CategoryDto>>
{
    public CategoryPagingQuery(string? keyword, CategoryQueryParams @params, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.GroupCategoryId))
        {
            Filter.Add("groupId", @params.GroupCategoryId);
        }
        if (!String.IsNullOrEmpty(@params.ParentCategoryId))
        {
            Filter.Add("parentId", @params.ParentCategoryId);
        }
    }

    public CategoryPagingQuery(string? keyword, CategoryQueryParams @params, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
    {
        Keyword = keyword;
        Filter = new Dictionary<string, object>();
        Filter = new Dictionary<string, object>();
        if (@params.Status != null)
        {
            Filter.Add("status", @params.Status);
        }
        if (!String.IsNullOrEmpty(@params.GroupCategoryId))
        {
            Filter.Add("groupId", @params.GroupCategoryId);
        }
        if (!String.IsNullOrEmpty(@params.ParentCategoryId))
        {
            Filter.Add("parentId", @params.ParentCategoryId);
        }
    }

    public string? Keyword { get; set; }
    public Dictionary<string, object> Filter { get; set; }
}

public class CategoryQueryHandler : IQueryHandler<CategoryQueryListBox,
                                    IEnumerable<CategoryListBoxDto>>,
                                    IQueryHandler<CategoryQueryBreadcrumb, IEnumerable<CategoryListViewDto>>,
                                    IQueryHandler<CategoryQueryByLevel, IEnumerable<CategoryListViewDto>>,
                                    IQueryHandler<CategoryQueryCombobox, IEnumerable<CategoryComboboxDto>>,
                                    IQueryHandler<CategoryByChannelQueryCbx, IEnumerable<CategoryComboboxDto>>,
                                    IQueryHandler<CategoryQueryCheckExist, bool>,
                                    IQueryHandler<CategoryQueryById, CategoryDto>,
                                    IQueryHandler<CategoryQueryBySlug, CategoryDto>,
                                    IQueryHandler<CategoryQueryAllParent, IEnumerable<CategoryParentDto>>,
                                    IQueryHandler<CategoryPagingQuery, PagingResponse<CategoryDto>>,
                                    IQueryHandler<CategoryQueryByListId, IEnumerable<CategoryListViewDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryQueryHandler(ICategoryRepository categoryRespository)
    {
        _categoryRepository = categoryRespository;
    }
    public async Task<bool> Handle(CategoryQueryCheckExist request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.CheckExistById(request.Id);
    }
    public async Task<IEnumerable<CategoryListViewDto>> Handle(CategoryQueryByLevel request, CancellationToken cancellationToken)
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
        filter.Add("parent", request.Parent);
        filter.Add("level", request.Level);
        filter.Add("levelCount", request.LevelCount);

        var categorys = await _categoryRepository.Filter("", filter);
        var list = categorys.Select(c => new CategoryListViewDto()
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            Title = c.Title,
            FullName = c.FullName,
            Url = c.Url,
            Slug = c.Slug,
            Image = c.Image,
            Image1 = c.Image1,
            Image2 = c.Image2,
            ParentCategoryId = c.ParentCategoryId,
            ParentIds = c.ParentIds,
            GroupCategoryId = c.GroupCategoryId,
            DisplayOrder = c.DisplayOrder,
            JsonData = c.JsonData,
            Level = c.Level,
            Status = c.Status
        });

        return list;
    }
    public async Task<CategoryDto> Handle(CategoryQueryById request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetById(request.CategoryId);

        var parentCategory = category.ParentCategoryId != null ? await _categoryRepository.GetById((Guid)category.ParentCategoryId) : null;
        var result = new CategoryDto()
        {
            Id = category.Id,
            Code = category.Code,
            Name = category.Name,
            Title = category.Title,
            FullName = category.FullName,
            Url = category.Url,
            Slug = category.Slug,
            Image = category.Image,
            Image1 = category.Image1,
            Image2 = category.Image2,
            ParentIds = category.ParentIds,
            Level = category.Level,
            GroupCategoryCode = category.GroupCategoryCode,
            GroupCategoryName = category.GroupCategoryName,
            Description = category.Description,
            GroupCategoryId = category.GroupCategoryId,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategoryName = parentCategory != null ? parentCategory.Name : "",
            DisplayOrder = category.DisplayOrder,
            Status = category.Status,
            Keywords = category.Keywords,
            JsonData = category.JsonData,
            CreatedBy = category.CreatedBy,
            CreatedDate = category.CreatedDate,
            CreatedByName = category.CreatedByName,
            UpdatedBy = category.UpdatedBy,
            UpdatedDate = category.UpdatedDate,
            UpdatedByName = category.UpdatedByName
        };
        return result;
    }
    public async Task<CategoryDto> Handle(CategoryQueryBySlug request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetBySlug(request.Channel, request.Slug);

        var parentCategory = category.ParentCategoryId != null ? await _categoryRepository.GetById((Guid)category.ParentCategoryId) : null;
        var result = new CategoryDto()
        {
            Id = category.Id,
            Code = category.Code,
            Name = category.Name,
            Title = category.Title,
            FullName = category.FullName,
            Url = category.Url,
            Slug = category.Slug,
            Image = category.Image,
            Image1 = category.Image1,
            Image2 = category.Image2,
            ParentIds = category.ParentIds,
            Level = category.Level,
            GroupCategoryCode = category.GroupCategoryCode,
            GroupCategoryName = category.GroupCategoryName,
            Description = category.Description,
            GroupCategoryId = category.GroupCategoryId,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategoryName = parentCategory != null ? parentCategory.Name : "",
            DisplayOrder = category.DisplayOrder,
            Status = category.Status,
            Keywords = category.Keywords,
            JsonData = category.JsonData,
            CreatedBy = category.CreatedBy,
            CreatedDate = category.CreatedDate,
            CreatedByName = category.CreatedByName,
            UpdatedBy = category.UpdatedBy,
            UpdatedDate = category.UpdatedDate,
            UpdatedByName = category.UpdatedByName
        };
        return result;
    }
    public async Task<IEnumerable<CategoryParentDto>> Handle(CategoryQueryAllParent request, CancellationToken cancellationToken)
    {
        List<CategoryParentDto> List = new List<CategoryParentDto>();
        var listP = await RecursionParentId(List, request.CategoryId);
        return listP;
    }

    private async Task<List<CategoryParentDto>> RecursionParentId(List<CategoryParentDto> List, Guid? id)
    {
        if (id != null)
        {
            var category = await _categoryRepository.GetById((Guid)id);
            List.Add(new CategoryParentDto { Id = category.Id, Name = category.Name });
            await RecursionParentId(List, category.ParentCategoryId);
        }
        return List;
    }
    public async Task<PagingResponse<CategoryDto>> Handle(CategoryPagingQuery request, CancellationToken cancellationToken)
    {
        var response = new PagingResponse<CategoryDto>();
        var count = await _categoryRepository.FilterCount(request.Keyword, request.Filter);
        var categorys = await _categoryRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
        var data = categorys.Select(category => new CategoryDto()
        {
            Id = category.Id,
            Code = category.Code,
            Name = category.Name,
            Title = category.Title,
            Url = category.Url,
            Slug = category.Slug,
            Image = category.Image,
            Image1 = category.Image1,
            Image2 = category.Image2,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId,
            Status = category.Status,
            CreatedByName = category.CreatedByName,
            UpdatedByName = category.UpdatedByName,
        });
        response.Items = data;
        response.Total = count;
        response.Count = count;
        response.PageIndex = request.PageIndex;
        response.PageSize = request.PageSize;
        response.Successful();
        return response;
    }

    public async Task<IEnumerable<CategoryListBoxDto>> Handle(CategoryQueryListBox request, CancellationToken cancellationToken)
    {

        var categorys = await _categoryRepository.GetListListBox(request.Filter, request.Keyword);
        var result = categorys.Select(data => new CategoryListBoxDto()
        {
            Value = data.Id,
            Label = data.Name,
            Key = data.Code,
            ParentCategoryId = data.ParentCategoryId,
            DisplayOrder = data.DisplayOrder
        });
        return result;
    }
    public async Task<IEnumerable<CategoryComboboxDto>> Handle(CategoryQueryCombobox request, CancellationToken cancellationToken)
    {

        var categorys = await _categoryRepository.GetCombobox(request.Filter, request.Keyword);
        var result = categorys.Select(data => new CategoryComboboxDto()
        {
            Value = data.Id,
            Label = data.FullName,
            Key = data.Code,
            ParentCategoryId = data.ParentCategoryId,
        });
        return result;
    }
    public async Task<IEnumerable<CategoryComboboxDto>> Handle(CategoryByChannelQueryCbx request, CancellationToken cancellationToken)
    {

        var categorys = await _categoryRepository.GetCombobox(request.Channel, request.Keyword, request.Status);
        var lmin = categorys.Select(x => x.Level).OrderBy(x => x).FirstOrDefault(0);
        var listResult = new List<CategoryComboboxDto>();
        foreach (var cate0 in categorys.Where(x => x.Level == lmin).OrderBy(x => x.DisplayOrder))
        {
            var id0 = cate0.Id;
            listResult.Add(new CategoryComboboxDto()
            {
                Value = cate0.Id,
                Label = cate0.FullName,
                Key = cate0.Code,
                ParentCategoryId = cate0.ParentCategoryId
            });
            foreach (var cate1 in categorys.Where(x => x.ParentIds.ToUpper().Equals(id0.ToString().ToUpper() + "," + x.Id.ToString().ToUpper())).OrderBy(x => x.DisplayOrder))
            {
                var id1 = cate1.Id;
                listResult.Add(new CategoryComboboxDto()
                {
                    Value = cate1.Id,
                    Label = cate1.FullName,
                    Key = cate1.Code,
                    ParentCategoryId = cate1.ParentCategoryId
                });

                foreach (var cate2 in categorys.Where(x => x.ParentIds.ToUpper().Equals(id0.ToString().ToUpper() + "," + id1.ToString().ToUpper() + "," + x.Id.ToString().ToUpper())).OrderBy(x => x.DisplayOrder))
                {
                    var id2 = cate2.Id;
                    listResult.Add(new CategoryComboboxDto()
                    {
                        Value = cate2.Id,
                        Label = cate2.FullName,
                        Key = cate2.Code,
                        ParentCategoryId = cate2.ParentCategoryId
                    });

                    foreach (var cate3 in categorys.Where(x => x.ParentIds.ToUpper().ToUpper().Equals(id0.ToString().ToUpper() + "," + id1.ToString().ToUpper() + "," + id2.ToString().ToUpper() + "," + x.Id.ToString())).OrderBy(x => x.DisplayOrder))
                    {
                        var id3 = cate3.Id;
                        listResult.Add(new CategoryComboboxDto()
                        {
                            Value = cate3.Id,
                            Label = cate3.FullName,
                            Key = cate3.Code,
                            ParentCategoryId = cate3.ParentCategoryId
                        });

                    }

                }
            }
        }


        //var result = categorys.Select(data => new CategoryComboboxDto()
        //{
        //    Value = data.Id,
        //    Label = data.FullName,
        //    Key = data.Code,
        //    ParentCategoryId = data.ParentCategoryId
        //});
        return listResult;
    }
    public async Task<IEnumerable<CategoryListViewDto>> Handle(CategoryQueryBreadcrumb request, CancellationToken cancellationToken)
    {
        var categorys = await _categoryRepository.GetBreadcrumb(request.Group, request.Category);
        var list = categorys.Select(c => new CategoryListViewDto()
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            Title = c.Title,
            FullName = c.FullName,
            Url = c.Url,
            Slug = c.Slug,
            Image = c.Image,
            Image1 = c.Image1,
            Image2 = c.Image2,
            ParentCategoryId = c.ParentCategoryId,
            ParentIds = c.ParentIds,
            GroupCategoryId = c.GroupCategoryId,
            DisplayOrder = c.DisplayOrder,
            JsonData = c.JsonData,
            Level = c.Level,
            Status = c.Status
        });

        return list;
    }
    public async Task<IEnumerable<CategoryListViewDto>> Handle(CategoryQueryByListId request, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetCategoriesByListId(request.ListId);
        var list = result.Select(c => new CategoryListViewDto()
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            Title = c.Title,
            FullName = c.FullName,
            Url = c.Url,
            Slug = c.Slug,
            Image = c.Image,
            Image1 = c.Image1,
            Image2 = c.Image2,
            ParentCategoryId = c.ParentCategoryId,
            ParentIds = c.ParentIds,
            GroupCategoryId = c.GroupCategoryId,
            DisplayOrder = c.DisplayOrder,
            JsonData = c.JsonData,
            Level = c.Level,
            Status = c.Status
        });

        return list;
    }
}
