using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using api.Interfaces;
using Microsoft.SqlServer;

namespace api.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<FavoritePets> FavoritePets { get; set; }
        public DbSet<SpecialPackage> SpecialPackages { get; set; }
        public DbSet<RentInfo> RentsInfo { get; set; }
        public DbSet<SpecialPackagePet> SpecialPackagePets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<SpecialPackagePet>()
                .HasKey(sp => new { sp.SpecialPackageId, sp.PetId });

            modelBuilder.Entity<SpecialPackagePet>()
                .HasOne(sp => sp.SpecialPackage)
                .WithMany(s => s.SpecialPackagePets)
                .HasForeignKey(sp => sp.SpecialPackageId);

            modelBuilder.Entity<SpecialPackagePet>()
                .HasOne(sp => sp.Pet)
                .WithMany(p => p.SpecialPackagePets)
                .HasForeignKey(sp => sp.PetId);


            modelBuilder.Entity<FavoritePets>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<FavoritePets>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoritePets>()
                .HasOne(f => f.Pet)
                .WithMany()
                .HasForeignKey(f => f.PetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RentInfo>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<RentInfo>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RentInfo>()
                .HasOne(r => r.Pet)
                .WithMany()
                .HasForeignKey(r => r.PetId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<RentInfo>()
                .HasOne(r => r.SpecialPackage)
                .WithMany()
                .HasForeignKey(r => r.SpecialPackageId)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Review>()
                .HasKey(rv => rv.Id);

            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.User)
                .WithMany()
                .HasForeignKey(rv => rv.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.Pet)
                .WithMany()
                .HasForeignKey(rv => rv.PetId)
                .OnDelete(DeleteBehavior.Cascade);

        }




    }
}