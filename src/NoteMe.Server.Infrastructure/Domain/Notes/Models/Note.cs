using System;
using System.Collections.Generic;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Providers;

namespace NoteMe.Server.Core.Models
{
    public class Note : IIdProvider, 
        INameProvider, 
        ICreatedAtProvider, 
        IStatusProvider
    {
        public Guid Id { get; set; }
        public StatusEnum Status { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        
        public Guid? ActualNoteId { get; set; }
        public Guid UserId { get; set; }
        
        public User User { get; set; }
        public Note ActualNote { get; set; }
        
        public ICollection<Attachment> Attachments { get; set; } = new HashSet<Attachment>();
        public ICollection<Note> OldNotes { get; set; } = new HashSet<Note>();
    }
}