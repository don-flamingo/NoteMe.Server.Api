using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteMe.Server.Core.Models;

namespace NoteMe.Server.Infrastructure.Domain.Notes.Sql
{
    public class NoteEntityConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.HasOne(x => x.ActualNote)
                .WithMany(x => x.OldNotes)
                .HasForeignKey(x => x.ActualNoteId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Notes)
                .HasForeignKey(x => x.UserId);
        }
    }
}