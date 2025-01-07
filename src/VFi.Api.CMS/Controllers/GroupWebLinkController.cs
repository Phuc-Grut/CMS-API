using System.ComponentModel.DataAnnotations;
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
public class GroupWebLinkController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<GroupWebLinkController> _logger;
    public GroupWebLinkController(IMediatorHandler mediator, IContextUser context, ILogger<GroupWebLinkController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }
    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <returns>List GroupWebLink</returns>
    [HttpGet("get-list")]
    public async Task<IActionResult> GetList()
    {
        var result = await _mediator.Send(new GroupWebLinkQueryAll());
        return Ok(result);
    }
    [HttpGet("get-listbox")]
    public async Task<IActionResult> Get([FromQuery] ListBoxGroupWebLinkRequest request)
    {
        var result = await _mediator.Send(new GroupWebLinkQueryListBox(request.Status, request.Keyword));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PagingGroupWebLinkRequest request)
    {
        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        GroupWebLinkPagingQuery query = new GroupWebLinkPagingQuery(request.Keyword, request.Status, pageSize, pageIndex);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddGroupWebLinkRequest request)
    {
        var groupWebLink = new GroupWebLinkAddCommand();
        groupWebLink.Id = Guid.NewGuid();
        groupWebLink.Code = request.Code;
        groupWebLink.Name = request.Name;
        groupWebLink.Title = request.Title;
        groupWebLink.Description = request.Description;
        groupWebLink.Image = request.Image;
        groupWebLink.Url = request.Url;
        groupWebLink.Status = request.Status;
        groupWebLink.DisplayOrder = request.DisplayOrder;
        groupWebLink.CreatedBy = _context.GetUserId();
        groupWebLink.CreatedDate = DateTime.Now;
        groupWebLink.CreatedByName = _context.FullName;
        var result = await _mediator.SendCommand(groupWebLink);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditGroupWebLinkRequest request)
    {
        var groupWebLink = new GroupWebLinkEditCommand();
        groupWebLink.Id = new Guid(request.Id);
        groupWebLink.Code = request.Code;
        groupWebLink.Name = request.Name;
        groupWebLink.Title = request.Title;
        groupWebLink.Description = request.Description;
        groupWebLink.Image = request.Image;
        groupWebLink.Url = request.Url;
        groupWebLink.Status = request.Status;
        groupWebLink.DisplayOrder = request.DisplayOrder;
        groupWebLink.UpdatedBy = _context.GetUserId();
        groupWebLink.UpdatedDate = DateTime.Now;
        groupWebLink.UpdatedByName = _context.FullName;
        var result = await _mediator.SendCommand(groupWebLink);

        return Ok(result);
    }

    /// <summary>
    /// Cập nhật xắp sếp
    /// </summary>
    /// <param name="request">Thông  tin</param>
    /// <returns>notification</returns>
    [HttpPut("sort")]
    public async Task<IActionResult> Sort([FromBody] EditGroupWebLinkSortRequest request)
    {
        foreach (var item in request.ListGui)
        {
            var GroupWebLink = await _mediator.Send(new GroupWebLinkQueryById(item.Id));
            if (GroupWebLink == null)
            {
                return BadRequest(new ValidationResult("GroupWebLink not exists"));
            }
        }

        var datas = request.ListGui.Select(x => new GroupWebLinkSortDto
        {
            Id = x.Id,
            SortOrder = x.SortOrder
        });

        var data = new EditGroupWebLinkSortCommand(datas);

        var result = await _mediator.SendCommand(data);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.SendCommand(new GroupWebLinkDeleteCommand(new Guid(id)));

        return Ok(result);
    }
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new GroupWebLinkQueryById(id));


        Request.Headers.TryGetValue("publising", out var publising);
        if (publising.Any() && publising.First().Equals("1"))
            return Ok(new
            {
                rs.Id,
                rs.Code,
                rs.Name,
                rs.Title,
                rs.Description,
                rs.Image,
                rs.Url,
                rs.Status,
                rs.DisplayOrder
            });

        return Ok(rs);
    }
    [HttpGet("get-by-code/{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var rs = await _mediator.Send(new GroupWebLinkQueryByCode(code));
        Request.Headers.TryGetValue("publising", out var publising);
        if (publising.Any() && publising.First().Equals("1"))
            return Ok(new
            {
                rs.Id,
                rs.Code,
                rs.Name,
                rs.Title,
                rs.Description,
                rs.Image,
                rs.Url,
                rs.Status,
                rs.DisplayOrder
            });

        return Ok(rs);
    }
}
