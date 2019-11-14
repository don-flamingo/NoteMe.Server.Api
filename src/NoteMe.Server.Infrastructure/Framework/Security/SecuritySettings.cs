using System;

namespace NoteMe.Server.Infrastructure.Framework.Security
{
    public class SecuritySettings
    {
        public TimeSpan TokenDuration { get; set; }
        public string Key { get; set; }
    }
}