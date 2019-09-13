using System;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Server.Core.Providers;

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
    }
}