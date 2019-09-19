using System;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.DataTypes.Providers;

namespace NoteMe.Server.Core.Models
{
    public class Template : IIdProvider,
        IStatusProvider,
        INameProvider,
        ICreatedAtProvider
    {
        public Guid Id { get; set; }
        public StatusEnum Status { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public Guid UserId { get; set; }
        
        public User User { get; set; }
    }
}