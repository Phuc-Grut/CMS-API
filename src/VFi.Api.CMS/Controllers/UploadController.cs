using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VFi.Infra.CMS.FileConfig;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.CMS.Controllers;

[Route("api")]
public class UploadController : BaseController
{
    public UploadController(IMediatorHandler mediator, IContextUser context, ILogger<UploadController> logger, IOptions<FileUploadConfig> fileUploadConfig)
        : base(mediator, context, logger, fileUploadConfig)
    {
    }
    [RequestSizeLimit(100000000)]
    [HttpPost("file/upload")]
    public async Task<IActionResult> UploadFile()
    {
        return await Upload("", fileUploadConfig.Value.FileAllowExtensions);
    }

    [RequestSizeLimit(100000000)]
    [HttpPost("image/upload")]
    public async Task<IActionResult> UploadImage()
    {
        return await Upload("", fileUploadConfig.Value.ImageAllowExtensions);
    }
    [RequestSizeLimit(100000000)]
    [HttpPost("image/editor-upload")]
    public async Task<IActionResult> EditorUploadImage()
    {
        //Response.Clear();
        //Response.Headers.Add("name", "editor.png");
        //Response.ContentType = "application/json";
        //Response.Headers.Add("url", "/dam/content/editor.png");
        //Response.StatusCode = 200;
        //return Task.FromResult<IActionResult>(Ok(new { Id = 1,User= _context.GetUserId(), Url = "https://www.easternsun.vn/wp-content/uploads/2021/09/ES-Logo-ngang-slogan-1.png" }));
        return await Upload("content", fileUploadConfig.Value.ImageAllowExtensions);
    }
    [RequestSizeLimit(100000000)]
    [HttpPost("file/upload-mutil")]
    public async Task<IActionResult> UploadMutilFile()
    {
        return await UploadMutil("", fileUploadConfig.Value.FileAllowExtensions);
    }

    [RequestSizeLimit(100000000)]
    [HttpPost("image/upload-mutil")]
    public async Task<IActionResult> UploadMutilImage()
    {
        return await UploadMutil("", fileUploadConfig.Value.ImageAllowExtensions);
    }
}
