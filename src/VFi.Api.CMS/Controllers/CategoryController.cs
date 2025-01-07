using System.ComponentModel.DataAnnotations;
using MassTransit.Initializers;
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
public class CategoryController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<CategoryController> _logger;
    public CategoryController(IMediatorHandler mediator, IContextUser context, ILogger<CategoryController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }



    [HttpGet("get-breadcrumb")]
    public async Task<IActionResult> GetBreadCrumb([Required] string group, [Required] string category)
    {

        var result = await _mediator.Send(new CategoryQueryBreadcrumb(group, category));
        return Ok(result);

    }

    [HttpGet("get-by-level")]
    public async Task<IActionResult> GetByLevel([Required] string group, String? parent, int? level, int? levelCount)
    {

        var result = await _mediator.Send(new CategoryQueryByLevel(1, group, parent, level, levelCount));
        return Ok(result);

    }
    [HttpGet("get-listbox")]
    public async Task<IActionResult> Get([FromQuery] ListBoxCategoryRequest request)
    {
        var result = await _mediator.Send(new CategoryQueryListBox(request.ToBaseQuery(), request.Keyword));
        return Ok(result);
    }
    [HttpGet("get-cbx")]
    public async Task<IActionResult> GetCbx([FromQuery] ListBoxCategoryRequest request)
    {
        var result = await _mediator.Send(new CategoryQueryCombobox(request.ToBaseQuery(), request.Keyword));
        return Ok(result);
    }

    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PagingCategoryRequest request)
    {
        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        CategoryPagingQuery query = new CategoryPagingQuery(request.Keyword, request.ToBaseQuery(), pageSize, pageIndex);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCategoryRequest request)
    {
        var categoryAddCommand = new CategoryAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          !String.IsNullOrEmpty(request.ParentCategoryId) ? new Guid(request.ParentCategoryId) : null,
          !String.IsNullOrEmpty(request.GroupCategoryId) ? new Guid(request.GroupCategoryId) : null,
          request.Status,
          request.DisplayOrder,
          request.Keywords,
          request.JsonData,
          _context.GetUserId(),
          DateTime.Now,
          _context.FullName

      );
        categoryAddCommand.Image = request.Image;
        categoryAddCommand.Url = request.Url;
        categoryAddCommand.Slug = request.Slug;
        var result = await _mediator.SendCommand(categoryAddCommand);
        return Ok(result);
    }
    /// <summary>
    /// Lấy thông tin 
    /// </summary>
    /// <param name="id">Thông tin</param>
    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new CategoryQueryById(id));


        Request.Headers.TryGetValue("publising", out var publising);
        if (publising.Any() && publising.First().Equals("1"))
            return Ok(new { rs.Id, rs.Code, rs.Name, rs.Description, rs.ParentCategoryId, rs.FullName, rs.ParentIds, rs.JsonData, rs.Keywords, rs.Status });

        return Ok(rs);
    }
    /// <summary>
    /// Lấy thông tin breadcum
    /// </summary>
    [HttpGet("getAllParent/{id}")]
    public async Task<IActionResult> GetAllParent(Guid id)
    {
        var rs = await _mediator.Send(new CategoryQueryAllParent(id));
        return Ok(rs);
    }
    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditCategoryRequest request)
    {
        var categoryEditCommand = new CategoryEditCommand(
           new Guid(request.Id),
           request.Code,
           request.Name,
           request.Description,
           !String.IsNullOrEmpty(request.ParentCategoryId) ? new Guid(request.ParentCategoryId) : null,
           !String.IsNullOrEmpty(request.GroupCategoryId) ? new Guid(request.GroupCategoryId) : null,
           request.Status,
           request.DisplayOrder,
           request.Keywords,
           request.JsonData,
           _context.GetUserId(),
           DateTime.Now,
           _context.FullName
       );
        categoryEditCommand.Image = request.Image;
        categoryEditCommand.Url = request.Url;
        categoryEditCommand.Slug = request.Slug;
        var result = await _mediator.SendCommand(categoryEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var categorySortCommand = new CategorySortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(categorySortCommand);
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
        var result = await _mediator.SendCommand(new CategoryDeleteCommand(new Guid(id)));

        return Ok(result);
    }

    [HttpGet("get-by-slug")]
    public async Task<IActionResult> GetBySlug(string channel, string slug)
    {
        var rs = await _mediator.Send(new CategoryQueryBySlug(channel, slug));
        Request.Headers.TryGetValue("publising", out var publising);
        if (publising.Any() && publising.First().Equals("1"))
            return Ok(new { rs.Id, rs.Code, rs.Name, rs.Description, rs.ParentCategoryId, rs.FullName, rs.ParentIds, rs.JsonData, rs.Keywords, rs.Status });

        return Ok(rs);
    }
    [HttpPost("get-channel-content")]
    public async Task<IActionResult> Get([FromBody] PagingChannelContentRequest request)
    {
        var rs = await _mediator.Send(new CategoryQueryBySlug(request.Channel, request.Slug)).Select(x => new { x.Id, x.Code, x.Name, x.Slug, x.Keywords, x.Description, x.JsonData, x.Url });

        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;
        var contentQuery = new ContentPagingQuery(pageSize, pageIndex);
        contentQuery.Category = rs.Id.ToString();
        contentQuery.Channel = request.Channel;
        contentQuery.Keyword = request.Keyword;
        var contents = await _mediator.Send(contentQuery);
        return Ok(new { Channel = rs, Contents = contents });
    }
    [HttpPost("get-channel-content-full")]
    public async Task<IActionResult> GetContentFull([FromBody] PagingChannelContentRequest request)
    {
        var rs = await _mediator.Send(new CategoryQueryBySlug(request.Channel, request.Slug)).Select(x => new { x.Id, x.Code, x.Name, x.Slug, x.Keywords, x.Description, x.JsonData, x.Url });

        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;
        var contentQuery = new ContentPagingQuery(pageSize, pageIndex);
        contentQuery.Category = rs.Id.ToString();
        contentQuery.Keyword = request.Keyword;
        contentQuery.FullQuery = true;
        var contents = await _mediator.Send(contentQuery);

        return Ok(new { Channel = rs, Contents = contents });
    }
}
