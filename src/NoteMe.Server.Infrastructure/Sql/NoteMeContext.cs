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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Attachment>()
                .HasOne(x => x.Note)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.NoteId);

            modelBuilder.Entity<Note>()
                .HasOne(x => x.ActualNote)
                .WithMany(x => x.OldNotes)
                .HasForeignKey(x => x.ActualNoteId);

            modelBuilder.Entity<Note>()
                .HasOne(x => x.User)
                .WithMany(x => x.Notes)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Template>()
                .HasOne(x => x.User)
                .WithMany(x => x.Templates)
                .HasForeignKey(x => x.UserId);
        }
    }
}