using System;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    internal class VetContext : DbContext
    {
        internal DbSet<Animal> Animals { get; set; }
        internal DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=vet;user=root;password=");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Username).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();

                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Surname).IsRequired();
                entity.Property(e => e.IsAdmin).IsRequired();
            });

            modelBuilder.Entity<Adopter>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Surname).IsRequired();

                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Telephone).IsRequired();
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.Street).IsRequired();
                entity.Property(e => e.PostalCode).IsRequired();
                entity.Property(e => e.HouseNumber).IsRequired();
                entity.Property(e => e.Animal).IsRequired();
            });

            modelBuilder.Entity<Lost>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Animal).IsRequired();
            });

            modelBuilder.Entity<Death>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Animal).IsRequired();
            });

            modelBuilder.Entity<Vaccination>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Animal).IsRequired();
            });

            modelBuilder.Entity<Animal>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.BirthDate).IsRequired();
                entity.Property(e => e.JoinDate).IsRequired();
                
                entity.HasMany(e => e.Vaccinations)
                    .WithOne(e => e.Animal)
                    .HasForeignKey(e => e.Animal);
                
                entity.Property(e => e.Chip).IsRequired();
                entity.HasIndex(e => e.Chip).IsUnique();
                
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.State).IsRequired();
                entity.Property(e => e.Treatments).IsRequired();

                entity.HasOne(e => e.Adopter)
                    .WithOne(e => e.Animal)
                    .HasForeignKey<Adopter>(e => e.Animal);

                entity.HasOne(e => e.DeathInfo)
                    .WithOne(e => e.Animal)
                    .HasForeignKey<Death>(e => e.Animal);

                entity.HasOne(e => e.LostInfo)
                    .WithOne(e => e.Animal)
                    .HasForeignKey<Lost>(e => e.Animal);
            });
        }
    }
}
