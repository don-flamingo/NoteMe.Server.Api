using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NoteMe.Common.Providers;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Settings;

namespace NoteMe.Server.Infrastructure.Sql
{
    public class NoteMeContext: DbContext
    {
        private readonly SqlSettings _settings;
        
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Note> Notes { get; set; }

        public NoteMeContext(SqlSettings settings) 
        {
            _settings = settings;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_settings.ConnectionString, x => x.UseNetTopologySuite())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NoteMeContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }

        public override EntityEntry Add(object entity)
        {
            BeforeSave(entity);
            
            return base.Add(entity);
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            BeforeSave(entity);
            
            return base.Add(entity);
        }

        public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        {
            BeforeSave(entity);
            
            return base.AddAsync(entity, cancellationToken);
        }

        public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = new CancellationToken())
        {
            BeforeSave(entity);
            
            return base.AddAsync(entity, cancellationToken);
        }

        public override void AddRange(params object[] entities)
        {
            BeforeSave(entities);
            
            base.AddRange(entities);
        }

        public override Task AddRangeAsync(params object[] entities)
        {
            BeforeSave(entities);
            
            return base.AddRangeAsync(entities);
        }

        private void BeforeSave(params object[] entities)
        {
            foreach (var entity in entities)
            {
                BeforeSave(entity);
            }
        }

        private void BeforeSave(object entity)
        {
            if (entity is ICreatedAtProvider createdAtProvider)
            {
                createdAtProvider.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}