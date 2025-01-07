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
public class GroupCategoryController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<GroupCategoryController> _logger;
    public GroupCategoryController(IMediatorHandler mediator, IContextUser context, ILogger<GroupCategoryController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }
    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <returns>List GroupCategory</returns>
    [HttpGet("get-list")]
    public async Task<IActionResult> GetList()
    {
        var result = await _mediator.Send(new GroupCategoryQueryAll());
        return Ok(result);
    }
    [HttpGet("get-listbox")]
    public async Task<IActionResult> Get([FromQuery] ListBoxGroupCategoryRequest request)
    {
        var result = await _mediator.Send(new GroupCategoryQueryListBox(request.Status, request.Keyword));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PagingGroupCategoryRequest request)
    {
        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        GroupCategoryPagingQuery query = new GroupCategoryPagingQuery(request.Keyword, request.Status, pageSize, pageIndex);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddGroupCategoryRequest request)
    {
        var groupCategory = new GroupCategoryAddCommand();
        groupCategory.Id = Guid.NewGuid();
        groupCategory.Code = request.Code;
        groupCategory.Name = request.Name;
        groupCategory.Title = request.Title;
        groupCategory.Description = request.Description;
        groupCategory.Image = request.Image;
        groupCategory.Logo = request.Logo;
        groupCategory.Logo2 = request.Logo2;
        groupCategory.Favicon = request.Favicon;
        groupCategory.Url = request.Url;
        groupCategory.Tags = request.Tags;
        groupCategory.Email = request.Email;
        groupCategory.Phone = request.Phone;
        groupCategory.Address = request.Address;
        groupCategory.Facebook = request.Facebook;
        groupCategory.Youtube = request.Youtube;
        groupCategory.Zalo = request.Zalo;
        groupCategory.Status = request.Status;
        groupCategory.DisplayOrder = request.DisplayOrder;
        groupCategory.CreatedBy = _context.GetUserId();
        groupCategory.CreatedDate = DateTime.Now;
        groupCategory.CreatedByName = _context.FullName;
        var result = await _mediator.SendCommand(groupCategory);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditGroupCategoryRequest request)
    {
        var groupCategory = new GroupCategoryEditCommand();
        groupCategory.Id = new Guid(request.Id);
        groupCategory.Code = request.Code;
        groupCategory.Name = request.Name;
        groupCategory.Title = request.Title;
        groupCategory.Description = request.Description;
        groupCategory.Image = request.Image;
        groupCategory.Logo = request.Logo;
        groupCategory.Logo2 = request.Logo2;
        groupCategory.Favicon = request.Favicon;
        groupCategory.Url = request.Url;
        groupCategory.Tags = request.Tags;
        groupCategory.Email = request.Email;
        groupCategory.Phone = request.Phone;
        groupCategory.Address = request.Address;
        groupCategory.Facebook = request.Facebook;
        groupCategory.Youtube = request.Youtube;
        groupCategory.Zalo = request.Zalo;
        groupCategory.Status = request.Status;
        groupCategory.DisplayOrder = request.DisplayOrder;
        groupCategory.UpdatedBy = _context.GetUserId();
        groupCategory.UpdatedDate = DateTime.Now;
        groupCategory.UpdatedByName = _context.FullName;
        var result = await _mediator.SendCommand(groupCategory);

        return Ok(result);
    }

    /// <summary>
    /// Cập nhật xắp sếp
    /// </summary>
    /// <param name="request">Thông  tin</param>
    /// <returns>notification</returns>
    [HttpPut("sort")]
    public async Task<IActionResult> Sort([FromBody] EditGroupCategorySortRequest request)
    {
        foreach (var item in request.ListGui)
        {
            var GroupCategory = await _mediator.Send(new GroupCategoryQueryById(item.Id));
            if (GroupCategory == null)
            {
                return BadRequest(new ValidationResult("GroupCategory not exists"));
            }
        }

        var datas = request.ListGui.Select(x => new GroupCategorySortDto
        {
            Id = x.Id,
            SortOrder = x.SortOrder
        });

        var data = new EditGroupCategorySortCommand(datas);

        var result = await _mediator.SendCommand(data);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _mediator.SendCommand(new GroupCategoryDeleteCommand(new Guid(id)));

        return Ok(result);
    }
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new GroupCategoryQueryById(id));


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
                rs.Logo,
                rs.Logo2,
                rs.Favicon,
                rs.Url,
                rs.Tags,
                rs.Email,
                rs.Phone,
                rs.Address,
                rs.Facebook,
                rs.Youtube,
                rs.Zalo,
                rs.Status,
                rs.DisplayOrder
            });

        return Ok(rs);
    }
    [HttpGet("get-by-code/{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var rs = await _mediator.Send(new GroupCategoryQueryByCode(code));
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
                rs.Logo,
                rs.Logo2,
                rs.Favicon,
                rs.Url,
                rs.Tags,
                rs.Email,
                rs.Phone,
                rs.Address,
                rs.Facebook,
                rs.Youtube,
                rs.Zalo,
                rs.Status,
                rs.DisplayOrder
            });

        return Ok(rs);
    }
}
