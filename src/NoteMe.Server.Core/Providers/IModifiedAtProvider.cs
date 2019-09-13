using System;

namespace NoteMe.Server.Core.Providers
{
    public interface IModifiedAtProvider
    {
        DateTime ModifiedAt { get; set; }
    }
}