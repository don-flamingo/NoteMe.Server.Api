using System;

namespace NoteMe.Server.Core.Providers
{
    public interface ICreatedAtProvider
    {
        DateTime CreatedAt { get; set; }
    }
}