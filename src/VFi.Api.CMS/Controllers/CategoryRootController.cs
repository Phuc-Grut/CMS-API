using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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
public class CategoryRootController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator;
    private readonly ILogger<CategoryRootController> _logger;
    public CategoryRootController(IMediatorHandler mediator, IContextUser context, ILogger<CategoryRootController> logger)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
    }

    [HttpGet("get-by-level")]
    public async Task<IActionResult> GetByLevel(String? parent, int? level, int? levelCount)
    {

        var result = await _mediator.Send(new CategoryRootQueryByLevel(1, parent, level, levelCount));
        return Ok(result);

    }
    [HttpGet("get-listbox")]
    public async Task<IActionResult> Get([FromQuery] ListBoxCategoryRootRequest request)
    {
        var result = await _mediator.Send(new CategoryRootQueryListBox(request.ToBaseQuery(), request.Keyword));
        return Ok(result);
    }


    [HttpGet("paging")]
    public async Task<IActionResult> Paging([FromQuery] PagingCategoryRootRequest request)
    {
        var pageSize = request.Top;
        var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

        CategoryRootPagingQuery query = new CategoryRootPagingQuery(request.Keyword, request.ToBaseQuery(), pageSize, pageIndex);

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddCategoryRootRequest request)
    {
        var categoryRootAddCommand = new CategoryRootAddCommand(
          Guid.NewGuid(),
          request.Code,
          request.Name,
          request.Description,
          !String.IsNullOrEmpty(request.ParentCategoryId) ? new Guid(request.ParentCategoryId) : null,
          request.Status,
          request.DisplayOrder,
          request.IdNumber,
          request.Keywords,
          request.JsonData,
          _context.GetUserId(),
          DateTime.Now,
          _context.UserName
      );
        categoryRootAddCommand.Slug = request.Slug;
        var result = await _mediator.SendCommand(categoryRootAddCommand);
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var rs = await _mediator.Send(new CategoryRootQueryById(id));


        Request.Headers.TryGetValue("publising", out var publising);
        if (publising.Any() && publising.First().Equals("1"))
            return Ok(new { rs.Id, rs.Code, rs.Name, rs.Description, rs.ParentCategoryId, rs.FullName, rs.ParentIds, rs.JsonData, rs.Keywords, rs.Status });

        return Ok(rs);
    }


    [HttpGet("getAllParent/{id}")]
    public async Task<IActionResult> GetAllParent(Guid id)
    {
        var rs = await _mediator.Send(new CategoryRootQueryAllParent(id));
        return Ok(rs);
    }
    [HttpPut("edit")]
    public async Task<IActionResult> Put([FromBody] EditCategoryRootRequest request)
    {
        var categoryRootEditCommand = new CategoryRootEditCommand(
           new Guid(request.Id),
           request.Code,
           request.Name,
           request.Description,
           !String.IsNullOrEmpty(request.ParentCategoryId) ? new Guid(request.ParentCategoryId) : null,
           request.Status,
           request.DisplayOrder,
           request.IdNumber,
           request.Keywords,
           request.JsonData,
           _context.GetUserId(),
           DateTime.Now,
          _context.UserName
       );
        categoryRootEditCommand.Slug = request.Slug;
        var result = await _mediator.SendCommand(categoryRootEditCommand);

        return Ok(result);
    }

    [HttpPut("sort")]
    public async Task<IActionResult> SortList([FromBody] SortRequest request)
    {
        if (request.SortList.Count > 0)
        {
            var categoryRootSortCommand = new CategoryRootSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

            var result = await _mediator.SendCommand(categoryRootSortCommand);
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
        var result = await _mediator.SendCommand(new CategoryRootDeleteCommand(new Guid(id)));

        return Ok(result);
    }
    [HttpGet("get-cbx-by-tree")]
    public async Task<IActionResult> GetListCombobox()
    {
        var result = await _mediator.Send(new CategoryRootQueryCombobox());
        return Ok(result);
    }
    [HttpGet("get-by-slug")]
    public async Task<IActionResult> Get(string channel, string slug)
    {
        var rs = await _mediator.Send(new CategoryRootQueryBySlug(slug));
        Request.Headers.TryGetValue("publising", out var publising);
        if (publising.Any() && publising.First().Equals("1"))
            return Ok(new { rs.Id, rs.Code, rs.Name, rs.Description, rs.ParentCategoryId, rs.FullName, rs.ParentIds, rs.JsonData, rs.Keywords, rs.Status });
        return Ok(rs);
    }
}
