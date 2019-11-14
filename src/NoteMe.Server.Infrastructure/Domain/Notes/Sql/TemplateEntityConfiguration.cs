using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteMe.Server.Core.Models;

namespace NoteMe.Server.Infrastructure.Domain.Notes.Sql
{
    public class TemplateEntityConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.HasOne(x => x.User)
                .WithMany(x => x.Templates)
                .HasForeignKey(x => x.UserId);
        }
    }
}