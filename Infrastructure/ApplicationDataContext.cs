using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.DataConnection;


public class ApplicationDataContext : IdentityDbContext<User, Role, string
    , IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>
    , IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
    {

    }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<Topic> Topics { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasMany(u => u.Topics)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Entity<User>()
            .HasMany(UserRole => UserRole.Roles)
            .WithOne(user => user.User)
            .HasForeignKey(user => user.UserId)
            .IsRequired();

        builder.Entity<Role>()
            .HasMany(UserRole => UserRole.Users)
            .WithOne(user => user.Role)
            .HasForeignKey(user => user.RoleId)
            .IsRequired();

        builder.Entity<Topic>()
            .HasMany(t => t.Comments)
            .WithOne()
            .HasForeignKey(c => c.TopicId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();



        builder.SeedRoles();
        builder.SeedUsers();
        builder.SeedUserRoles();



    }
}
