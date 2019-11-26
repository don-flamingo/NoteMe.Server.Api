using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;
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
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAttachmentCommand command)
        {
            await DispatchAsync(command);
            var cached = GetDto<AttachmentDto>(command.Id);
            return Created("api/attachments", cached);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAsync(GetAttachmentQuery getAttachmentQuery)
        {
            var results = await DispatchQueryAsync<GetAttachmentQuery, PaginationDto<AttachmentDto>>(getAttachmentQuery);
            return Ok(results);
        }
        
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var cmd = new DeleteAttachmentCommand()
            {
                Id = id
            };

            await DispatchAsync(cmd);
            return Ok();
        }
        
        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync([FromForm(Name ="file")] IFormFile formFile)
        {
            await _cdnService.SaveFileAsync(formFile);
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