using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using VFi.Api.CMS.ViewModels;
using VFi.Application.CMS.Commands;
using VFi.Infra.CMS.FileConfig;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;

namespace VFi.Api.CMS.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected static readonly FormOptions DefaultFormOptions = new FormOptions();
    protected static object lockFile = new object();

    protected readonly ILogger logger;
    protected readonly IOptions<FileUploadConfig> fileUploadConfig;
    protected readonly IContextUser _context;
    protected readonly IMediatorHandler _mediator;
    public BaseController(IMediatorHandler mediator, IContextUser context, ILogger logger, IOptions<FileUploadConfig> fileUploadConfig)
    {
        this.logger = logger;
        _context = context;
        _mediator = mediator;
        this.fileUploadConfig = fileUploadConfig;
    }

    protected async Task<IActionResult> Upload(string path, params string[] allowExtensions)
    {
        try
        {
            UploadResponse response = new UploadResponse();
            DateTime dtNow = DateTime.UtcNow;
            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), DefaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = reader.ReadNextSectionAsync().Result;

            while (section != null)
            {
                ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                {

                    var fileNameUpload = HeaderUtilities.RemoveQuotes(contentDisposition.FileName).Value;
                    if (!Validate(fileNameUpload, allowExtensions))
                    {
                        response.Fail(new List<DetailError>() { new DetailError(ErrorCodeDefine.FILE_FILE_NAME_IS_NULL_OR_EMPTY, null) });
                        return Ok(response);
                    }
                    string fileName = Path.GetFileNameWithoutExtension(fileNameUpload);
                    string extension = Path.GetExtension(fileNameUpload);
                    string folderPath = Path.Combine(_context.Tenant.ToLower(), path, dtNow.ToString("yy") + dtNow.Month.ToString("00"));
                    string filePath = Path.Combine(folderPath, string.Format("{0}{1}", fileName, extension));
                    string fullFolderPath = Path.Combine(fileUploadConfig.Value.RootFolder, folderPath);
                    if (!Directory.Exists(fullFolderPath))
                    {
                        Directory.CreateDirectory(fullFolderPath);
                    }
                    string fullFilePath = Path.Combine(fileUploadConfig.Value.RootFolder, filePath);

                    lock (lockFile)
                    {
                        int i = 0;
                        while (System.IO.File.Exists(fullFilePath))
                        {
                            i++;
                            fileName += $"_{i}";
                            filePath = Path.Combine(folderPath, string.Format("{0}{1}", fileName, extension));
                            fullFilePath = Path.Combine(fileUploadConfig.Value.RootFolder, filePath);
                        }
                    }
                    string virtualPath = Path.Combine(_context.Tenant.ToLower(), TextGeneration.TickUnique, fileName + extension);
                    using (var targetStream = System.IO.File.Create(fullFilePath))
                    {
                        await section.Body.CopyToAsync(targetStream);
                    }

                    var itemAddCommand = new ItemAddCommand(
                         Guid.NewGuid(),
                         fileName,
                         fileName,
                         fileName,
                         Convert.ToInt32(section.Body.Length),
                         true,
                         null,
                         extension,
                         false,
                         virtualPath,
                         filePath,
                         "",
                         _context.Product,
                         1,
                         "",
                         "",
                         _context.Tenant,
                         _context.GetUserId(),
                         DateTime.Now
                     );

                    var result = await _mediator.SendCommand(itemAddCommand);

                    if (result.Errors.Count > 0)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        response.Status = true;
                        response.Id = itemAddCommand.Id;
                        response.Title = itemAddCommand.Title;
                        response.Name = string.Format("{0}{1}", fileName, extension);
                        response.Size = itemAddCommand.Size;
                        response.Path = filePath;
                        response.VirtualPath = virtualPath;
                        response.Type = extension;
                        return Ok(response);
                    }
                }
                section = await reader.ReadNextSectionAsync();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
    protected async Task<IActionResult> UploadMutil(string path, params string[] allowExtensions)
    {
        try
        {
            UploadMultiResponse response = new UploadMultiResponse();
            List<InfoFileResponse> infoFileResponses = new List<InfoFileResponse>();
            DateTime dtNow = DateTime.UtcNow;
            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), DefaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = reader.ReadNextSectionAsync().Result;

            while (section != null)
            {
                ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                //var createdUid = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
                //if (string.IsNullOrEmpty(createdUid))
                //{
                //    response.Fail(new List<DetailError>() { new DetailError(ErrorCodeDefine.FILE_CREATED_USER_ID_IS_NULL_OR_EMPTY, null) });
                //    return Ok(response);
                //}

                var fileNameUpload = HeaderUtilities.RemoveQuotes(contentDisposition.FileName).Value;
                if (!Validate(fileNameUpload, allowExtensions))
                {
                    response.Fail(new List<DetailError>() { new DetailError(ErrorCodeDefine.FILE_FILE_NAME_IS_NULL_OR_EMPTY, null) });
                    return Ok(response);
                }

                string fileName = Path.GetFileNameWithoutExtension(fileNameUpload);
                string extension = Path.GetExtension(fileNameUpload);
                string folderPath = Path.Combine(_context.Tenant.ToLower(), path, dtNow.ToString("yy") + dtNow.Month.ToString("00"));
                string filePath = Path.Combine(folderPath, string.Format("{0}{1}", fileName, extension));
                string fullFolderPath = Path.Combine(fileUploadConfig.Value.RootFolder, folderPath);
                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }
                string fullFilePath = Path.Combine(fileUploadConfig.Value.RootFolder, filePath);

                lock (lockFile)
                {
                    int i = 0;
                    while (System.IO.File.Exists(fullFilePath))
                    {
                        i++;
                        fileName += $"_{i}";
                        filePath = Path.Combine(folderPath, string.Format("{0}{1}", fileName, extension));
                        fullFilePath = Path.Combine(fileUploadConfig.Value.RootFolder, filePath);
                    }
                }
                string virtualPath = Path.Combine(_context.Tenant.ToLower(), TextGeneration.TickUnique, fileName + extension);
                using (var targetStream = System.IO.File.Create(fullFilePath))
                {
                    await section.Body.CopyToAsync(targetStream);
                }

                var itemAddCommand = new ItemAddCommand(
                     Guid.NewGuid(),
                     fileName,
                     fileName,
                     fileName,
                     Convert.ToInt32(section.Body.Length),
                     true,
                     null,
                     extension,
                     false,
                     virtualPath,
                     filePath,
                     "",
                     _context.Product,
                     1,
                     "",
                     "",
                     _context.Tenant,
                     _context.GetUserId(),
                     DateTime.Now
                 );

                var result = await _mediator.SendCommand(itemAddCommand);

                if (result.Errors.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    infoFileResponses.Add(new InfoFileResponse()
                    {
                        Status = true,
                        Id = itemAddCommand.Id,
                        Title = itemAddCommand.Title,
                        Name = string.Format("{0}{1}", fileName, extension),
                        Size = itemAddCommand.Size,
                        Path = filePath,
                        VirtualPath = virtualPath,
                        Type = extension
                    });
                }
                section = await reader.ReadNextSectionAsync();
            }
            response.infoFiles = infoFileResponses;
            response.Status = true;
            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return StatusCode(500, ex.Message);
        }
    }

    private bool Validate(string fileName, params string[] extensions)
    {
        string extension = Path.GetExtension(fileName).ToLower().Trim('.');

        return extensions.Any(m => m.Equals(extension));
    }
}
