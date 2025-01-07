using Microsoft.AspNetCore.Mvc;
using VFi.Api.CMS.ViewModels;
using VFi.Application.CMS.Commands;
using VFi.Application.CMS.DTOs;
using VFi.Application.CMS.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.CMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContentTypeController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ContentTypeController> _logger;
    public ContentTypeController(IMediatorHandler mediator, IContextUser context, ILogger<ContentTypeController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-listbox")]
    public async Task<IActionResult> Get([FromQuery] ListBoxContentTypeRequest request)
    {
        var result = await _mediator.Send(new ContentTypeQueryListBox(request.Status, request.Keyword));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PagingContentTypeRequest request)
    {
        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        ContentTypePagingQuery query = new ContentTypePagingQuery(request.Keyword, request.Status, pageSize, pageIndex);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddContentTypeRequest request)
    {
        var contentTypeAddCommand = new ContentTypeAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          request.Status,
          request.DisplayOrder,
          _context.GetUserId(),
          DateTime.Now,
          _context.UserName
      );
        var result = await _mediator.SendCommand(contentTypeAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditContentTypeRequest request)
    {
        var contentTypeEditCommand = new ContentTypeEditCommand(
           new Guid(request.Id),
           request.Code,
           request.Name,
           request.Description,
           request.Status,
           request.DisplayOrder,
           _context.GetUserId(),
           DateTime.Now,
          _context.UserName
       );

        var result = await _mediator.SendCommand(contentTypeEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var contentTypeSortCommand = new ContentTypeSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(contentTypeSortCommand);
            return Ok(result);
        }
        else
        {
            return BadRequest("Please input list sort");
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.SendCommand(new ContentTypeDeleteCommand(new Guid(id)));

        return Ok(result);
    }
}
