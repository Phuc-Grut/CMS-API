using Microsoft.AspNetCore.Mvc;
using VFi.Application.CMS.Queries;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.CMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemProductController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ItemProductController> _logger;
    public ItemProductController(IMediatorHandler mediator, IContextUser context, ILogger<ItemProductController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }
    [HttpGet("get-all")]
    public async Task<IActionResult> Get()
    {
        var rs = await _mediator.Send(new ItemProductQueryAll());
        return Ok(rs);
    }
}
