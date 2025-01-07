using Microsoft.AspNetCore.Mvc;
using VFi.Domain.CMS.Models;

namespace VFi.Api.CMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDAMContextProcedures _damContextProcedures;
    public DashboardController(IDAMContextProcedures damContextProcedures)
    {
        _damContextProcedures = damContextProcedures;
    }
    [HttpGet("get-top-new-content")]
    public async Task<IActionResult> Get_Top_New_Content()
    {
        var result = await _damContextProcedures.SP_GET_TOP_NEW_CONTENTAsync();
        return Ok(result);
    }
    [HttpGet("get-top-category")]
    public async Task<IActionResult> Get_Top_Category()
    {
        var result = await _damContextProcedures.SP_GET_TOP_CATEGORYAsync();
        return Ok(result);
    }
    [HttpGet("get-content-by-type")]
    public async Task<IActionResult> Count_Product_By_Type()
    {
        var result = await _damContextProcedures.SP_GET_CONTENT_BY_CONTENTTYPEAsync();
        return Ok(result);
    }
}
