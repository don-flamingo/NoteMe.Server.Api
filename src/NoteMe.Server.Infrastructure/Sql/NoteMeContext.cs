using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Settings;

namespace NoteMe.Server.Infrastructure.Sql
{
    public class NoteMeContext: DbContext
    {
        private readonly SqlSettings _settings;
        
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Note> Notes { get; set; }

        public NoteMeContext(DbContextOptions<NoteMeContext> options, SqlSettings settings) : base(options)
        {
            _settings = settings;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_settings.ConnectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NoteMeContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}