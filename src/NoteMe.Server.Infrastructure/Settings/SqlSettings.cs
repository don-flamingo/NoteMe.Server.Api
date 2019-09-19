namespace NoteMe.Server.Infrastructure.Settings
{
    public class SqlSettings
    {
        public string ConnectionString { get; set; }
        public bool InMemory { get; set; }
        public string Schema { get; set; }
    }
}