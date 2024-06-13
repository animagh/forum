using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure;

public static class DataSeeder
{


    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        PasswordHasher<User> hasher = new();
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = "8716071C-1D9B-48FD-B3D0-F059C4FB8031",
                UserName = "admin",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "Admin123"),
                PhoneNumber = "555334455",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                Name = "Admin",
                Surname = "Admininistrator"
            }, new User
            {
                Id = "D514EDC9-94BB-416F-AF9D-7C13669689C9",
                UserName = "ani17",
                NormalizedUserName = "ANI17",
                Email = "ani@gmail.com",
                NormalizedEmail = "ANI@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "Asdzxc123"),
                PhoneNumber = "555334456",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                Name = "Ani",
                Surname = "Magradze"

            },
            new User
            {
                Id = "87746F88-DC38-4756-924A-B95CFF3A1D8A",
                UserName = "rezirezi",
                NormalizedUserName = "REZIREZI",
                Email = "rezi@gmail.com",
                NormalizedEmail = "Rezi@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "Qweasd123"),
                PhoneNumber = "555334457",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                Name = "Rezi",
                Surname = "Magradze"
            }
        );
    }

    public static void SeedRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
                new Role { Id = "33B7ED72-9434-434A-82D4-3018B018CB87", Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Id = "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", Name = "User", NormalizedName = "USER" }
        );
    }

    public static void SeedUserRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>().HasData(
                new UserRole { RoleId = "33B7ED72-9434-434A-82D4-3018B018CB87", UserId = "8716071C-1D9B-48FD-B3D0-F059C4FB8031" },
                new UserRole { RoleId = "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", UserId = "D514EDC9-94BB-416F-AF9D-7C13669689C9" },
                new UserRole { RoleId = "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", UserId = "87746F88-DC38-4756-924A-B95CFF3A1D8A" }
        );
    }

}