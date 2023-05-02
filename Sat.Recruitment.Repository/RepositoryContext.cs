using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Sat.Recruitment.Model;

namespace Sat.Recruitment.Repository
{
    public class RepositoryContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlite();
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
            Database.Migrate();
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public RepositoryContext() : base() { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);            
            modelBuilder.Entity<User>().HasKey(x => x.UserId);
        }
    }
}
