using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using NoteMe.Common.Providers;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Sql;

namespace NoteMe.Server.Infrastructure.Framework.Generators
{
    public interface IDataSeeder<TEntity>
        where TEntity : IIdProvider
    {
        ICollection<TEntity> GetDataToSeed();
    }
    
    public interface IDataSeeder
    {
        Task SeedAsync();
    }
    
    public class DataSeeder : IDataSeeder
    {
        private readonly IDataSeeder<User> _userSeeder;
        private readonly NoteMeContext _noteMeContext;

        public DataSeeder(IDataSeeder<User> userSeeder,
            NoteMeContext noteMeContext)
        {
            _userSeeder = userSeeder;
            _noteMeContext = noteMeContext;
        }
        
        public async Task SeedAsync()
        {
            var users = _userSeeder.GetDataToSeed();
            var ids = users.Select(x => x.Id);
            var existed = await _noteMeContext.Users.Where(x => ids.Any(s => s == x.Id)).ToListAsync();
            var toInsert = users.Where(x => !existed.Any(s => s.Id == x.Id));

            await _noteMeContext.AddRangeAsync(toInsert);

            await _noteMeContext.SaveChangesAsync();
        }
    }
}