using Application.Entites;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Application.Models;
using Application.Entities;

namespace Application.Context
{
    public class SubcrybeContext : IdentityDbContext<AppUsers, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public SubcrybeContext(DbContextOptions<SubcrybeContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<AppUsers>()
            //    .HasOne<UserSettings>(us => us.usersettings)
            //    .WithOne(u => u.User)
            //    .HasForeignKey<UserSettings>(us => us.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);
           // builder.Entity<PendingUsers>().ToTable("PendingUsers");

            builder.Entity<AppUsers>().Property<DateTime>("CreatedDate");
            builder.Entity<AppUsers>().Property<DateTime>("UpdatedDate");
            //builder.Entity<RefreshToken>().Property<DateTime>("CreatedDate");
            //builder.Entity<RefreshToken>().Property<DateTime>("UpdatedDate");
            //builder.Entity<ResetPasswordTokens>().Property<DateTime>("CreatedDate");
            //builder.Entity<ResetPasswordTokens>().Property<DateTime>("UpdatedDate");
            //builder.Entity<AppUserRole>(userRole =>
            //{
            //    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
            //    userRole.HasOne(ur => ur.Role)
            //    .WithMany(u => u.UserRole)
            //    .HasForeignKey(u => u.RoleId);

            //    //userRole.HasOne(ur => ur.Users)
            //    //.WithMany(ur => ur.AppUserRole)
            //    //.HasForeignKey(ur => ur.UserId);

            //});
            builder.Entity<RefreshToken>().HasKey(t => t.TokenId);
            builder.Entity<ResetPasswordTokens>().HasKey(t => t.UserId);
            Seed.Seed.Run(builder);
        }
        public DbSet<AppProperties> AppProperties { get; set; }
         public DbSet<PropertyImages> PropertyImages { get; set; }
        public DbSet<AppUsers> users { get; set; }
       // public DbSet<UserSettings> UserSettings { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public DbSet<ResetPasswordTokens> ResetPasswordTokens { get; set; }

        //public DbSet<UserJobs> UserJobs { get; set; }


        public DbSet<UserSettings> UserSettings { get; set; }
        
        //public DbSet<UserApprovalTokens> UserApprovals { get; set; }

        //public DbSet<PendingUsers> PendingUsers { get; set; }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e =>
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                try
                {
                    entityEntry.Property("UpdatedDate").CurrentValue = DateTime.Now;

                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Property("CreatedDate").CurrentValue = DateTime.Now;
                    }
                }
                catch (Exception ex) { }
            }

            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
              .Entries()
              .Where(e =>
                      e.State == EntityState.Added
                      || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                try
                {
                    entityEntry.Property("UpdatedDate").CurrentValue = DateTime.Now;

                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Property("CreatedDate").CurrentValue = DateTime.Now;
                    }
                }
                catch (Exception ex) { }

            }

            // _dbContextEvents.SaveChanges(ChangeTracker);
            return base.SaveChangesAsync(cancellationToken);
        }
        // public DbSet<UserSettings> UserUpdates { get; set; }
    }
}
