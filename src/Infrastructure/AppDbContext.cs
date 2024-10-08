﻿using Domain.Comman;
using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Database.Migrate();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Configuration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var softDeleteEntries = ChangeTracker
                .Entries<IDeletable>()
                .Where(x => x.State == EntityState.Deleted);

            foreach (var entry in softDeleteEntries)
            {
                entry.State = EntityState.Modified;
                entry.Property(nameof(IDeletable.IsDeleted)).CurrentValue = true;
            }

            var createdAt = ChangeTracker
                .Entries<Auditable>()
                .Where(x => x.State == EntityState.Added);

            foreach (var entry in createdAt)
            {
                entry.Property(nameof(Auditable.CreatedAt)).CurrentValue = DateTime.UtcNow;
            }

                var result = await base.SaveChangesAsync(cancellationToken);
            ChangeTracker.Clear();

            return result;
        }
    }
}
