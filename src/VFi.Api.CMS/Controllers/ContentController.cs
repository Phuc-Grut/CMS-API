using Microsoft.AspNetCore.Mvc;
using VFi.Api.CMS.ViewModels;
using VFi.Application.CMS.Commands;
using VFi.Application.CMS.DTOs;
using VFi.Application.CMS.Queries;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.CMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContentController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ContentController> _logger;
    private readonly CodeSyntaxConfig _codeSyntax;
    public ContentController(IMediatorHandler mediator, IContextUser context, ILogger<ContentController> logger, CodeSyntaxConfig codeSyntax)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
        _codeSyntax = codeSyntax;
    }
    private async Task<IActionResult> HandleResult(FluentValidation.Results.ValidationResult result, Guid ContentId)
    {
        if (result.IsValid == true)
        {
            return Ok(
                new ContentsResult()
                {
                    Id = ContentId,
                    IsValid = result.IsValid,
                }
                );
        }
        return Ok(result);
    }
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddContentRequest request)
    {
        var ContentId = Guid.NewGuid();
        var Code = request.Code;
        int UsedStatus = 1;
        if (request.IsAuto == 1)
        {
            Code = await _mediator.Send(new GetCodeQuery(_codeSyntax.CMS_Content, UsedStatus));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
               _codeSyntax.CMS_Content,
                Code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var categories = request.Categories?.Select(x => new ContentCategoriesDto()
        {
            CategoryId = x.CategoryId,
            GroupCategoryId = x.GroupCategoryId,
        }).ToList();
        var contentAddCommand = new ContentAddCommand(
            ContentId,
            request.ContentTypeId,
            request.ContentType,
            Code,
            request.Name,
            request.LinkInfo,
            request.ShortDescription,
            request.FullDescription,
            request.CategoryRootId,
            request.CategoryRoot,
            request.IdGroupCategories,
            categories,
            request.IdCategories,
            request.Image,
            request.Deleted,
            request.Status,
            _context.GetUserId(),
            DateTime.Now,
            _context.UserName,
            request.Tags
      );
        contentAddCommand.Slug = request.Slug;
        var result = await _mediator.SendCommand(contentAddCommand);
        if (result.IsValid == false && request.IsAuto == 1 && result.Errors[0].ToString() == "ContentCode AlreadyExists")
        {
            int loopTime = 5;
            for (int i = 0; i < loopTime; i++)
            {
                contentAddCommand.Code = await _mediator.Send(new GetCodeQuery(_codeSyntax.CMS_Content, UsedStatus));
                var res = await _mediator.SendCommand(contentAddCommand);
                if (res.IsValid == true)
                {
                    return await HandleResult(res, ContentId);
                }
            }
        }
        return await HandleResult(result, ContentId);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditContentRequest request)
    {
        var categories = request.Categories?.Select(x => new ContentCategoriesDto()
        {
            CategoryId = x.CategoryId,
            GroupCategoryId = x.GroupCategoryId,
        }).ToList();
        var contentEditCommand = new ContentEditCommand(
            new Guid(request.Id),
            request.ContentTypeId,
            request.ContentType,
            request.Code,
            request.Name,
            request.LinkInfo,
            request.ShortDescription,
            request.FullDescription,
            request.CategoryRootId,
            request.CategoryRoot,
            request.IdGroupCategories,
            categories,
            request.IdCategories,
            request.Image,
            request.Deleted,
            request.Status,
            _context.GetUserId(),
            DateTime.Now,
            _context.UserName,
            request.Tags
       );
        contentEditCommand.Slug = request.Slug;
        var result = await _mediator.SendCommand(contentEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.SendCommand(new ContentDeleteCommand(new Guid(id)));

        return Ok(result);
    }
    [HttpGet("get-code")]
    public async Task<IActionResult> GetCode(string syntax, int use)
    {
        var rs = await _mediator.Send(new GetCodeQuery(syntax, use));
        return Ok(rs);
    }
    [HttpGet("use-code")]
    public async Task<IActionResult> UseCode(string syntax)
    {
        return Ok();
    }
    [HttpGet("get-listbox")]
    public async Task<IActionResult> Get([FromQuery] ListBoxContentRequest request)
    {
        var result = await _mediator.Send(new ContentQueryListBox(request.ToBaseQuery(), request.Keyword));
        return Ok(result);
    }

    [HttpGet("get-cbx-by-categories")]
    public async Task<IActionResult> GetListBoxBycategory([FromQuery] PagingContentCategoryMappingRequest request)
    {
        var result = await _mediator.Send(new ContentCategoryMappingQueryListBox(request.ToBaseQuery()));
        return Ok(result);
    }
    [HttpGet("get-by-id/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new ContentQueryById(id));
        return Ok(rs);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Pagedresult([FromQuery] FilterQuery request)
    {
        var query = new ContentPagingFilterQuery(request?.Keyword, request?.Filter, request?.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpPost("duplicate")]
    public async Task<IActionResult> Duplicate([FromBody] DuplicateContentRequest request)
    {
        var ContentId = request.Id;
        var Code = request.Code;
        int UsedStatus = 1;
        if (request.IsAuto == 1)
        {
            Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
                request.ModuleCode,
                Code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var contentDuplicateCommand = new ContentDuplicateCommand(
            ContentId,
            Code,
            request.Name,
            UsedStatus,
             _context.GetUserId(),
             _context.UserName
       );
        var result = await _mediator.SendCommand(contentDuplicateCommand);

        var contentId = await _mediator.Send(new ContentQueryByCode(Code));

        if (result.IsValid == false && request.IsAuto == 1 && result.Errors[0].ToString() == "ContentCode AlreadyExists")
        {
            int loopTime = 5;
            for (int i = 0; i < loopTime; i++)
            {
                contentDuplicateCommand.Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
                var res = await _mediator.SendCommand(contentDuplicateCommand);
                if (res.IsValid == true)
                {
                    return await HandleResult(res, ContentId);
                }
            }
        }
        return await HandleResult(result, contentId.Id);
    }
    [HttpGet("item/{id}")]
    public async Task<IActionResult> Get(string id, int? getrelate)
    {
        Guid itemId = Guid.Empty;
        bool isValid = Guid.TryParse(id, out itemId);
        if (isValid)
        {
            var rs = await _mediator.Send(new ContentQueryById(itemId));
            if (rs.Deleted.HasValue && rs.Deleted.Value)
                return NotFound();
            return Ok(new { rs.Id, rs.Image, rs.ShortDescription, rs.FullDescription, rs.CreatedDate, rs.Categories, rs.IdCategories, rs.Code });
        }
        else
        {
            int idNumber = 0;
            bool isValidId = Int32.TryParse(id, out idNumber);
            if (isValidId)
            {
                var rs = await _mediator.Send(new ContentQueryByIdNumber(idNumber));
                if (rs.Deleted.HasValue && rs.Deleted.Value)
                    return NotFound();
                return Ok(new { rs.Id, rs.Image, rs.ShortDescription, rs.FullDescription, rs.CreatedDate, rs.Categories, rs.IdCategories, rs.Code });

            }
            else
                return NotFound();

        }


    }

    [HttpGet("item2/{channel}/{slug}")]
    public async Task<IActionResult> GetBySlug(string channel, string slug)
    {
        var rs = await _mediator.Send(new ContentQueryBySlug(channel, slug));
        return Ok(rs);
    }
    [HttpGet("item3/{Achannel}/{category}/{slug}")]
    public async Task<IActionResult> GetBySlug(string channel, string category, string slug, int? getrelate)
    {
        var rs = await _mediator.Send(new ContentQueryByCategorySlug(channel, category, slug));
        return Ok(rs);
    }
    [HttpGet("item1/{id}")]
    public async Task<IActionResult> Get(int id, int? getrelate)
    {
        var rs = await _mediator.Send(new ContentQueryByIdNumber(id));
        return Ok(rs);
    }

    [HttpGet("pre-next")]
    public async Task<IActionResult> PreNext(string current, int pre, int next)
    {
        var query = new PreNextContentQuery(current, pre, next);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search(string keyword, string channel, int? top)
    {
        if (!top.HasValue)
            top = 10;
        var query = new ContentSearchQuery() { Keyword = keyword, Channel = channel, Status = 1, Top = top.Value };
        var result = await _mediator.Send(query);
        var listIdGroupCategories = result.Select(x => x.IdGroupCategories).ToList();
        var listGroupId = new List<Guid>();

        foreach (var item in listIdGroupCategories)
        {
            listGroupId.AddRange(item.Split(',').Select(x => Guid.Parse(x)));
        }
        listGroupId = listGroupId.Distinct().ToList();
        var queryGroupCategory = new GroupCategoryQueryByListId()
        {
            ListId = listGroupId
        };
        var listGroupCategory = await _mediator.Send(queryGroupCategory);
        //--------------- 

        var listIdCategories = result.Select(x => x.IdCategories).ToList();
        var listId = new List<Guid>();
        foreach (var item in listIdCategories)
        {
            listId.AddRange(item.Split(',').Select(x => Guid.Parse(x)));
        }
        listId = listId.Distinct().ToList();
        var queryCategory = new CategoryQueryByListId()
        {
            ListId = listId
        };
        var listCategory = await _mediator.Send(queryCategory);

        return Ok(result.Select(x => new
        {
            x.Id,
            x.Name,
            x.Code,
            x.Title,
            x.Slug,
            x.ShortDescription,
            x.Image,
            x.Tags,
            Path = GetPath(x.IdGroupCategories, x.IdCategories, listGroupCategory.ToList(), listCategory.ToList()) + "/" + x.Slug
        }));
    }
    private string GetPath(string strGroup, string strCagegory, List<GroupCategoryDto> optGroup, List<CategoryListViewDto> optCategory)
    {

        var channelSlug = "";
        var cateSlug = "";
        var groupid = strGroup.Split(',').FirstOrDefault().ToLower();
        var cateId = strCagegory.Split(',').FirstOrDefault().ToLower();
        channelSlug = optGroup.Where(x => x.Id.ToString().ToLower().Equals(groupid)).FirstOrDefault().Slug;
        cateSlug = optCategory.Where(x => x.Id.ToString().ToLower().Equals(cateId)).FirstOrDefault().Slug;
        return "/" + channelSlug + "/" + cateSlug;
    }
    [HttpGet("related/{channel}/{id}")]
    public async Task<IActionResult> GetRelated(string channel, Guid id, string category, int? size)
    {
        if (!size.HasValue)
            size = 10;
        var result = await _mediator.Send(new ContentRelatedQuery()
        {
            Channel = channel,
            ContentId = id,
            Category = category,
            Max = size.Value
        });
        var listIdGroupCategories = result.Select(x => x.IdGroupCategories).ToList();
        var listGroupId = new List<Guid>();

        foreach (var item in listIdGroupCategories)
        {
            listGroupId.AddRange(item.Split(',').Select(x => Guid.Parse(x)));
        }
        listGroupId = listGroupId.Distinct().ToList();
        var queryGroupCategory = new GroupCategoryQueryByListId()
        {
            ListId = listGroupId
        };
        var listGroupCategory = await _mediator.Send(queryGroupCategory);
        //--------------- 

        var listIdCategories = result.Select(x => x.IdCategories).ToList();
        var listId = new List<Guid>();
        foreach (var item in listIdCategories)
        {
            listId.AddRange(item.Split(',').Select(x => Guid.Parse(x)));
        }
        listId = listId.Distinct().ToList();
        var queryCategory = new CategoryQueryByListId()
        {
            ListId = listId
        };
        var listCategory = await _mediator.Send(queryCategory);

        return Ok(result.Select(x => new
        {
            x.Id,
            x.Name,
            x.Code,
            x.Title,
            x.Slug,
            x.ShortDescription,
            x.Image,
            x.Tags,
            Path = GetPath(x.IdGroupCategories, x.IdCategories, listGroupCategory.ToList(), listCategory.ToList()) + "/" + x.Slug
        }));
        // return Ok(rs);
    }
    [HttpGet("top")]
    public async Task<IActionResult> Top(string channel, string category, int top, int? body)
    {
        if (!body.HasValue)
            body = 0;
        var query = new ContentTopQuery(channel, category, top);
        query.WithBody = body.Value;
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("get-display-top")]
    public async Task<IActionResult> GetDisplayTop(string channel, string? category, int? top)
    {
        if (!top.HasValue)
            top = 10;
        var rs = await _mediator.Send(new ContentDisplayTopQuery()
        {
            Channel = channel,
            Category = category,
            Status = 1,
            Top = top.Value
        });
        return Ok(rs);
    }
    [HttpGet("get-display-top1")]
    public async Task<IActionResult> GetDisplayTop1(string channel, string? category, int? top)
    {
        if (!top.HasValue)
            top = 10;
        var rs = await _mediator.Send(new ContentDisplayTop1Query()
        {
            Channel = channel,
            Category = category,
            Status = 1,
            Top = top.Value
        });
        return Ok(rs);
    }
    [HttpGet("get-display-top2")]
    public async Task<IActionResult> GetDisplayTop2(string channel, string? category, int? top)
    {
        if (!top.HasValue)
            top = 10;
        var rs = await _mediator.Send(new ContentDisplayTop2Query()
        {
            Channel = channel,
            Category = category,
            Status = 1,
            Top = top.Value
        });
        return Ok(rs);
    }

    [HttpGet("paging-public")]
    public async Task<IActionResult> PagingPublic(string channel, string category, string keyword, int? size, int? page)
    {
        if (!size.HasValue)
            size = 10;
        if (!page.HasValue)
            page = 1;
        var query = new ContentPagingQuery(size.Value, page.Value)
        {
            Channel = channel,
            Category = category,
            Keyword = keyword
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
