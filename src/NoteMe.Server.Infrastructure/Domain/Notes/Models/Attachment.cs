using System;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Providers;

namespace NoteMe.Server.Core.Models
{
    public class Attachment : IIdProvider, 
        IStatusProvider, 
        INameProvider
    {
        public Guid Id { get; set; }
        public StatusEnum Status { get; set; }
        public string Name { get; set; }
        public AttachmentTypeEnum Type { get; set; }
        
        public Guid NoteId { get; set; }
        public Note Note { get; set; }
    }
}