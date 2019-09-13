using NoteMe.Common.DataTypes.Enums;

namespace NoteMe.Server.Core.Providers
{
    public interface IStatusProvider
    {
        StatusEnum Status { get; set; }
    }
}