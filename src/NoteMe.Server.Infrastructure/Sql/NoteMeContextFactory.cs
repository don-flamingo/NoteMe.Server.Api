using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NoteMe.Server.Infrastructure.Settings;

namespace NoteMe.Server.Infrastructure.Sql
{
    public class NoteMeContextFactory: IDesignTimeDbContextFactory<NoteMeContext>
    {
        private SqlSettings _settings { get; set; }
        
        public NoteMeContextFactory()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _settings = new SqlSettings();
            configuration.GetSection("Sql").Bind(_settings);
        }
        
        public NoteMeContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<NoteMeContext>()
                .UseNpgsql(_settings.ConnectionString);
            
            return new NoteMeContext(builder.Options, _settings);
        }
    }
}
