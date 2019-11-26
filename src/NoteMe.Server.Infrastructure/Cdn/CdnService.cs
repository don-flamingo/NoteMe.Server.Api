using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NoteMe.Common.Exceptions;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Sql;

namespace NoteMe.Server.Infrastructure.Cdn
{
    public interface ICdnService
    {
        Task SaveFileAsync(IFormFile formFile);
        Task<String> GetFilePathAsync(Guid id);
    }
    
    public class CdnService : ICdnService
    {
        private readonly CdnSettings _settings;
        private readonly NoteMeContext _context;

        public CdnService(
            CdnSettings settings,
            NoteMeContext context)
        {
            _settings = settings;
            _context = context;
        }
        
        public async Task SaveFileAsync(IFormFile formFile)
        {
            var ext = Path.GetExtension(formFile.FileName);
            var id = Guid.Parse(Path.GetFileNameWithoutExtension(formFile.FileName));
            var file = await _context.Attachments.FirstAsync(x => x.Id == id);
            var fileFullName = GetFileFullName(ext, id, file);
            using (var fileStream = new FileStream(fileFullName, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
        }
        
        private string GetFileFullName(string ext, Guid fileId, Attachment file)
        {
            var dir = Path.Combine(_settings.Path, file.NoteId.ToString());

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var fileFullName = Path.Combine(dir, fileId.ToString() + ext);
            return fileFullName;
        }

        public async Task<string> GetFilePathAsync(Guid id)
        {
            var file = await _context.Attachments.FirstAsync(x => x.Id == id);
            if (file == null)
            {
                throw new ServerException(ErrorCodes.InvalidParameter);
            }

            var dir = Path.Combine(_settings.Path, file.NoteId.ToString());
            return  Directory.GetFiles(dir, $"{id}.*").First();
        }
    }
}