using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VFi.Api.CMS.ViewModels;
using VFi.Application.CMS.Commands;
using VFi.Application.CMS.DTOs;
using VFi.Application.CMS.Queries;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.CMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebLinkController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<WebLinkController> _logger;
    public WebLinkController(IMediatorHandler mediator, IContextUser context, ILogger<WebLinkController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }



    [HttpGet("get-breadcrumb")]
    public async Task<IActionResult> GetBreadCrumb([Required] string group, [Required] string webLink)
    {

        var result = await _mediator.Send(new WebLinkQueryBreadcrumb(group, webLink));
        return Ok(result);

    }

    [HttpGet("get-by-level")]
    public async Task<IActionResult> GetByLevel([Required] string group, String? parent, int? level, int? levelCount, int? top)
    {

        var result = await _mediator.Send(new WebLinkQueryByLevel(1, group, parent, level, levelCount, top));
        return Ok(result);

    }
    [HttpGet("get-by-group")]
    public async Task<IActionResult> GetByGroup([Required] string group, int? status)
    {

        var result = await _mediator.Send(new WebLinkQueryByGroup(group.Split(";").ToList(), status));
        return Ok(result);

    }
    [HttpGet("get-listbox")]
    public async Task<IActionResult> Get([FromQuery] ListBoxWebLinkRequest request)
    {
        var result = await _mediator.Send(new WebLinkQueryListBox(request.ToBaseQuery(), request.Keyword));
        return Ok(result);
    }
    [HttpGet("get-cbx")]
    public async Task<IActionResult> GetCbx([FromQuery] ListBoxWebLinkRequest request)
    {
        var result = await _mediator.Send(new WebLinkQueryCombobox(request.ToBaseQuery(), request.Keyword));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PagingWebLinkRequest request)
    {
        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        WebLinkPagingQuery query = new WebLinkPagingQuery(request.Keyword, request.ToBaseQuery(), pageSize, pageIndex);

        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchWebLinkRequest request)
    {
        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        var query = new WebLinkSearchQuery(pageSize, pageIndex)
        {
            Group = request.Group,
            Keyword = request.Keyword,
            Status = 1
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddWebLinkRequest request)
    {
        var webLinkAddCommand = new WebLinkAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          !String.IsNullOrEmpty(request.ParentWebLinkId) ? new Guid(request.ParentWebLinkId) : null,
          !String.IsNullOrEmpty(request.GroupWebLinkId) ? new Guid(request.GroupWebLinkId) : null,
          request.Status,
          request.DisplayOrder,
          request.Keywords,
          _context.GetUserId(),
          DateTime.Now,
          _context.FullName

      );
        webLinkAddCommand.Image = request.Image;
        webLinkAddCommand.Image2 = request.Image2;
        webLinkAddCommand.Image3 = request.Image3;
        webLinkAddCommand.Url = request.Url;
        var result = await _mediator.SendCommand(webLinkAddCommand);
        return Ok(result);
    }
    /// <summary>
    /// Lấy thông tin 
    /// </summary>
    /// <param name="id">Thông tin</param>
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new WebLinkQueryById(id));


        Request.Headers.TryGetValue("publising", out var publising);
        if (publising.Any() && publising.First().Equals("1"))
            return Ok(new
            {
                rs.Id,
                rs.Code,
                rs.Name,
                rs.Description,
                rs.ParentWebLinkId,
                rs.FullName,
                rs.Image,
                rs.Image2,
                rs.Image3,
                rs.ParentIds,
                rs.Keywords,
                rs.Status
            });

        return Ok(rs);
    }
    /// <summary>
    /// Lấy thông tin breadcum
    /// </summary>
    [HttpGet("getAllParent/{id}")]
    public async Task<IActionResult> GetAllParent(Guid id)
    {
        var rs = await _mediator.Send(new WebLinkQueryAllParent(id));
        return Ok(rs);
    }
    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditWebLinkRequest request)
    {
        var webLinkEditCommand = new WebLinkEditCommand(
           new Guid(request.Id),
           request.Code,
           request.Name,
           request.Description,
           !String.IsNullOrEmpty(request.ParentWebLinkId) ? new Guid(request.ParentWebLinkId) : null,
           !String.IsNullOrEmpty(request.GroupWebLinkId) ? new Guid(request.GroupWebLinkId) : null,
           request.Status,
           request.DisplayOrder,
           request.Keywords,
           _context.GetUserId(),
           DateTime.Now,
           _context.FullName
       );
        webLinkEditCommand.Image = request.Image;
        webLinkEditCommand.Image2 = request.Image2;
        webLinkEditCommand.Image3 = request.Image3;
        webLinkEditCommand.Url = request.Url;
        var result = await _mediator.SendCommand(webLinkEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var webLinkSortCommand = new WebLinkSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(webLinkSortCommand);
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
        var result = await _mediator.SendCommand(new WebLinkDeleteCommand(new Guid(id)));

        return Ok(result);
    }
}
