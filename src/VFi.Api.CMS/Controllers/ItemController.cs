using System.IO;
using System.Net.Mime;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using VFi.Api.CMS.ViewModels;
using VFi.Application.CMS.Commands;
using VFi.Application.CMS.Queries;
using VFi.Infra.CMS.FileConfig;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.CMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<ItemController> _logger;
    protected readonly IOptions<FileUploadConfig> _fileUploadConfig;

    protected static readonly FormOptions DefaultFormOptions = new FormOptions();
    protected static object lockFile = new object();


    public ItemController(IMediatorHandler mediator, IContextUser context, ILogger<ItemController> logger, IOptions<FileUploadConfig> fileUploadConfig)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
        _fileUploadConfig = fileUploadConfig;

    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new ItemQueryById(id));
        return Ok(rs);
    }
    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PagingItemRequest request)
    {
        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        ItemPagingQuery query = new ItemPagingQuery(request.Keyword, request.ToBaseQuery(), pageSize, pageIndex);

        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("getAllParent/{id}")]
    public async Task<IActionResult> GetAllParent(Guid id)
    {
        var rs = await _mediator.Send(new ItemQueryAllParent(id));
        return Ok(rs);
    }
    [HttpPost("add-folder")]
    public async Task<IActionResult> AddFolder([FromBody] AddFolderRequest request)
    {
        DateTime dtNow = DateTime.UtcNow;
        var createdUid = HeaderUtilities.RemoveQuotes(request.Title).Value;
        string fileName = dtNow.Ticks.ToString() + "_" + createdUid;
        var itemAddCommand = new FolderAddCommand(
          Guid.NewGuid(),
          fileName,
          request.Title,
          request.Title,
          0,
          true,
          !String.IsNullOrEmpty(request.ParentId) ? new Guid(request.ParentId) : null,
          "",
          false,
          fileName,
          "",
          request.Product,
          1,
          "",
          "",
          _context.Tenant,
          _context.GetUserId(),
          DateTime.Now
      );
        var result = await _mediator.SendCommand(itemAddCommand);
        return Ok(result);
    }
    [HttpPut("edit-folder")]
    public async Task<IActionResult> EditFolder([FromBody] EditFolderRequest request)
    {
        DateTime dtNow = DateTime.UtcNow;
        var createdUid = HeaderUtilities.RemoveQuotes(request.Title).Value;
        string fileName = dtNow.Ticks.ToString() + "_" + createdUid;
        var itemAddCommand = new FolderEditCommand(
          new Guid(request.Id),
          fileName,
          request.Title,
          request.Title,
          0,
          request.IsFile,
          !String.IsNullOrEmpty(request.ParentId) ? new Guid(request.ParentId) : null,
          "",
          false,
          fileName,
          "",
          request.Product,
          1,
          "",
          "",
          _context.Tenant,
          _context.GetUserId(),
          DateTime.Now
      );
        var result = await _mediator.SendCommand(itemAddCommand);
        return Ok(result);
    }
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddItemRequest request)
    {
        var itemAddCommand = new ItemAddCommand(
          Guid.NewGuid(),
          request.Name,
          request.Title,
          request.Description,
          request.Size,
          request.IsFile,
          !String.IsNullOrEmpty(request.ParentId) ? new Guid(request.ParentId) : null,
          request.MimeType,
          request.HasChild,
          "",
          request.LocalPath,
          request.Cdn,
          request.Product,
          request.Status,
          request.Workspace,
          request.Content,
          _context.Tenant,
          _context.GetUserId(),
          DateTime.Now

      );
        var result = await _mediator.SendCommand(itemAddCommand);
        return Ok(result);
    }
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.SendCommand(new ItemDeleteCommand(new Guid(id)));

        return Ok(result);
    }
}
