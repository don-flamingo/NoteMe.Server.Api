using System;

namespace NoteMe.Server.Core.Providers
{
    public interface IIdProvider
    {
        Guid Id { get; set; }
    }
}