using System;
using System.Collections.Generic;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.DataTypes.Providers;

namespace NoteMe.Server.Core.Models
{
    public class User : IIdProvider, 
        IStatusProvider, 
        INameProvider, 
        ICreatedAtProvider, 
        IModifiedAtProvider
    {
        public Guid Id { get; set; }
        public StatusEnum Status { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
        
        public ICollection<Template> Templates { get; set; } = new HashSet<Template>();
        public ICollection<Note> Notes { get; set; } = new HashSet<Note>();
    }
}