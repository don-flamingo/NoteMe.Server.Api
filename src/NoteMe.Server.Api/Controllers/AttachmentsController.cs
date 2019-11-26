using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NoteMe.Server.Infrastructure.Cdn;
using NoteMe.Server.Infrastructure.Cqrs.Commands;
using NoteMe.Server.Infrastructure.Cqrs.Queries;
using NoteMe.Server.Infrastructure.Framework.Cache;

namespace NoteMe.Server.Api.Controllers
{
    public class AttachmentsController : NoteMeControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        private readonly ICdnService _cdnService;

        public AttachmentsController(
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider,
            ICdnService cdnService,
            IQueryDispatcher queryDispatcher, 
            ICacheService memoryCacheService, 
            ICommandDispatcher commandDispatcher) : base(queryDispatcher, memoryCacheService, commandDispatcher)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;
            _cdnService = cdnService;
        }
        
        
        

        [Authorize]
        [HttpPost("upload/{id}")]
        public async Task<IActionResult> UploadAsync(Guid id, [FromBody] IFormFile formFile)
        {
            await _cdnService.SaveFileAsync(formFile, id);
            return Ok();
        }
        
        [Authorize]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadAsync(Guid id)
        {
            var filePath = await _cdnService.GetFilePathAsync(id);
            _fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType);
            var file = System.IO.File.OpenRead(filePath);
            return File(file, contentType);
        }
        
    }
}