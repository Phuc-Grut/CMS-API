using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VFi.Application.CMS.Queries;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.FileConfig;
using VFi.NetDevPack.Mediator;

namespace VFi.Api.CMS.Controllers;

[Route("api")]
public class LinkFileController : ControllerBase
{
    protected readonly IOptions<FileUploadConfig> _fileUploadConfig;
    protected readonly IMediatorHandler _mediator;
    public LinkFileController(IOptions<FileUploadConfig> fileUploadConfig, IItemRepository itemRepository, IMediatorHandler mediator)
    {
        _fileUploadConfig = fileUploadConfig;
        _mediator = mediator;
    }

    [AllowAnonymous]
    [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 7200)]
    [HttpGet("image/{id}")]
    public async Task<IActionResult> GetImageAsync(Guid id)
    {
        try
        {
            var item = await _mediator.Send(new ItemQueryById(id));

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                var stream = new FileStream(item.LocalPath, FileMode.Open);
                var extentsion = Path.GetExtension(item.LocalPath).ToLower().Trim('.');
                var contentType = $"image/{extentsion}";

                return new FileStreamResult(stream, contentType);
            }
        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }

    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 7200)]
    [Route("file/{*path:regex(^(?!file).*$)}")]
    public async Task<IActionResult> GetAsync(string path)
    {
        try
        {
            Guid itemId = Guid.Empty;
            bool isValid = Guid.TryParse(path, out itemId);
            if (isValid)
            {
                var item = await _mediator.Send(new ItemQueryById(itemId));
                if (item == null)
                {
                    return NotFound();
                }
                else
                {
                    var stream = new FileStream(item.LocalPath, FileMode.Open);
                    var extentsion = Path.GetExtension(item.LocalPath).ToLower().Trim('.');
                    new FileExtensionContentTypeProvider().TryGetContentType(item.LocalPath, out var contentType);

                    return new FileStreamResult(stream, contentType);
                }
            }
            else
            {
                var rootFolder = _fileUploadConfig.Value.RootFolder;
                var extentsion = Path.GetExtension(path).ToLower().Trim('.');
                var fullPath = Path.Combine(rootFolder, path);
                var stream = new FileStream(fullPath, FileMode.Open);
                new FileExtensionContentTypeProvider().TryGetContentType(path, out var contentType);
                return new FileStreamResult(stream, contentType);
            }


        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }

}
