using Microsoft.AspNetCore.Mvc;
using VFi.Api.CMS.ViewModels;
using VFi.Application.CMS.Commands;
using VFi.Application.CMS.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.CMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContentCategoryMappingController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ContentCategoryMappingController> _logger;

    public ContentCategoryMappingController(IMediatorHandler mediator, IContextUser context, ILogger<ContentCategoryMappingController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PagingContentCategoryMappingRequest request)
    {
        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        ContentCategoryMappingPagingQuery query = new ContentCategoryMappingPagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddContentCategoryMappingRequest request)
    {
        var contentAddCommand = new ContentCategoryMappingAddCommand(
            Guid.NewGuid(),
            new Guid(request.CategoryId),
            new Guid(request.ContentId),
            request.DisplayOrder
      );
        var result = await _mediator.SendCommand(contentAddCommand);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditContentCategoryMappingRequest request)
    {
        var contentEditCommand = new ContentCategoryMappingEditCommand(
            Guid.NewGuid(),
            new Guid(request.CategoryId),
            new Guid(request.ContentId),
            request.DisplayOrder
       );

        var result = await _mediator.SendCommand(contentEditCommand);

        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.SendCommand(new ContentCategoryMappingDeleteCommand(new Guid(id)));

        return Ok(result);
    }
}
