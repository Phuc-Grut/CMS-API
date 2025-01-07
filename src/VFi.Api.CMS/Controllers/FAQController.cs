using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.CMS.ViewModels;
using VFi.Application.CMS.Commands;
using VFi.Application.CMS.DTOs;
using VFi.Application.CMS.Queries;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.CMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FAQController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ContentController> _logger;
    private readonly CodeSyntaxConfig _codeSyntax;
    public FAQController(IMediatorHandler mediator, IContextUser context, ILogger<ContentController> logger, CodeSyntaxConfig codeSyntax)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
        _codeSyntax = codeSyntax;
    }
    [HttpGet("paging")]
    public async Task<IActionResult> Pagedresult([FromQuery] FilterQuery request)
    {
        var query = new ContentPagingFilterQuery(request?.Keyword, "FAQ", request?.Filter, request?.Order, request.PageNumber, request.PageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("get-cate")]
    public async Task<IActionResult> GetByLevel(string? parent, int? level, int? levelCount)
    {

        // var result = await _mediator.Send(new CategoryQueryByLevel(1, "FAQ", parent, level, levelCount));
        var result = await _mediator.Send(new CategoryByChannelQueryCbx()
        {
            Channel = "FAQ",
            Keyword = "",
            Status = 1
        });
        return Ok(result);

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
    public async Task<IActionResult> Add([FromBody] AddFaqRequest request)
    {
        var itemId = Guid.NewGuid();
        var code = request.Code;
        if (request.IsAuto.HasValue && request.IsAuto.Value)
        {
            code = await _mediator.Send(new GetCodeQuery(_codeSyntax.CMS_Content, 1));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(_codeSyntax.CMS_Content, code);
            _mediator.SendCommand(useCodeCommand);
        }


        request.Code = code;
        var cmd = new FaqAddCommand()
        {
            Id = itemId,
            Code = code,
            Name = request.Name,
            Title = request.Title,
            Slug = request.Slug,
            ShortDescription = request.ShortDescription,
            FullDescription = request.FullDescription,
            Tags = request.Tags,
            Image = request.Image,
            Status = request.Status,
            ListIdCategories = request.ListIdCategories,
            CreatedDate = DateTime.Now,
            CreatedBy = _context.GetUserId(),
            CreatedByName = _context.FullName
        };

        var result = await _mediator.SendCommand(cmd);
        if (result.IsValid == false && request.IsAuto.HasValue && request.IsAuto.Value & result.Errors[0].ToString() == "Code Already Exists")
        {
            int loopTime = 5;
            for (int i = 0; i < loopTime; i++)
            {
                cmd.Code = await _mediator.Send(new GetCodeQuery(_codeSyntax.CMS_Content, 1));
                var res = await _mediator.SendCommand(cmd);
                if (res.IsValid == true)
                {
                    return await HandleResult(res, itemId);
                }
            }
        }
        return await HandleResult(result, itemId);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditFaqRequest request)
    {

        var cmd = new FaqEditCommand()
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Title = request.Title,
            Image = request.Image,
            Slug = request.Slug,
            ShortDescription = request.ShortDescription,
            FullDescription = request.FullDescription,
            Tags = request.Tags,
            Status = request.Status,
            ListIdCategories = request.ListIdCategories,
            UpdatedDate = DateTime.Now,
            UpdatedBy = _context.GetUserId(),
            UpdatedByName = _context.FullName
        };

        var result = await _mediator.SendCommand(cmd);

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


    [HttpPost("duplicate")]
    public async Task<IActionResult> Duplicate([FromBody] DuplicateFaqRequest request)
    {
        var ContentId = request.Id;
        var code = request.Code;
        if (request.IsAuto.HasValue && request.IsAuto.Value)
        {
            code = await _mediator.Send(new GetCodeQuery(_codeSyntax.CMS_Content, 1));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(
               _codeSyntax.CMS_Content,
                code
                );
            _mediator.SendCommand(useCodeCommand);
        }
        var contentDuplicateCommand = new FaqDuplicateCommand(
            ContentId,
            code,
            request.Name,
            1,
             _context.GetUserId(),
             _context.UserName
       );
        var result = await _mediator.SendCommand(contentDuplicateCommand);

        var contentId = await _mediator.Send(new ContentQueryByCode(code));

        if (result.IsValid == false && request.IsAuto.HasValue && request.IsAuto.Value && result.Errors[0].ToString() == "Code Already Exists")
        {
            int loopTime = 5;
            for (int i = 0; i < loopTime; i++)
            {
                contentDuplicateCommand.Code = await _mediator.Send(new GetCodeQuery(_codeSyntax.CMS_Content, 1));
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
    public async Task<IActionResult> Get(string id)
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
    [HttpGet("item3/{channel}/{category}/{slug}")]
    public async Task<IActionResult> GetBySlug(string channel, string category, string slug)
    {
        var rs = await _mediator.Send(new ContentQueryByCategorySlug(channel, category, slug));
        return Ok(rs);
    }
    [HttpGet("item1/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var rs = await _mediator.Send(new ContentQueryByIdNumber(id));
        return Ok(rs);
    }
    [HttpGet("top")]
    public async Task<IActionResult> Top(string channel, string category, int top)
    {
        var query = new ContentTopQuery(channel, category, top);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("pre-next")]
    public async Task<IActionResult> PreNext(string current, int pre, int next)
    {
        var query = new PreNextContentQuery(current, pre, next);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
