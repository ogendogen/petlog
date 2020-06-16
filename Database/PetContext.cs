﻿using System;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    /// <summary>
    /// Pet context class
    /// </summary>
    internal class PetContext : DbContext
    {
        /// <summary>
        /// Animals' db set
        /// </summary>
        internal DbSet<Animal> Animals { get; set; }
        /// <summary>
        /// Users' db set
        /// </summary>
        internal DbSet<User> Users { get; set; }
        /// <summary>
        /// Adoptives' db set
        /// </summary>
        internal DbSet<Adoptive> Adoptives { get; set; }
        /// <summary>
        /// Vaccinations db set
        /// </summary>
        public DbSet<Vaccination> Vaccination { get; set; }
        /// <summary>
        /// Death info's db set
        /// </summary>
        public DbSet<Death> Death { get; set; }
        /// <summary>
        /// Lost infos' db set
        /// </summary>
        public DbSet<Lost> Lost { get; set; }
        /// <summary>
        /// Expiring vaccinations' db set
        /// </summary>
        public DbSet<ExpiringVaccination> ExpiringVaccination { get; set; }

        /// <summary>
        /// Pet context constructor, creates view for expiring vaccinations
        /// </summary>
        public PetContext()
        {
            string viewQuery = "CREATE VIEW IF NOT EXISTS View_ExpiringVaccination AS " +
                                    "SELECT Name AS \"Imię\", vaccination.Date AS \"Data szczepienia\", DATE_ADD(vaccination.Date, INTERVAL 1 YEAR) AS \"Data ważności\" FROM animals " +
                                    "LEFT JOIN vaccination ON animals.ID = vaccination.AnimalID " +
                                    "LEFT JOIN death ON animals.ID = death.AnimalID " +
                                    "LEFT JOIN lost ON animals.ID = lost.AnimalID " +
                                    "WHERE CURRENT_DATE() > DATE_ADD(DATE_ADD(vaccination.Date, INTERVAL 1 YEAR), INTERVAL - 1 WEEK) " +
                                    "AND CURRENT_DATE() < DATE_ADD(vaccination.Date, INTERVAL 1 YEAR)";

            Database.ExecuteSqlRaw(viewQuery);
        }

        /// <summary>
        /// Overrided method with database configuration
        /// </summary>
        /// <param name="optionsBuilder">DbContext options builder</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=pet;user=root;password=");
        }

        /// <summary>
        /// Overrided method with model creation
        /// </summary>
        /// <param name="modelBuilder">DbContext model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.ID).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Username).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();

                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Surname).IsRequired();
                entity.Property(e => e.IsAdmin).IsRequired();
            });

            modelBuilder.Entity<Adoptive>(entity =>
            {
                entity.Property(e => e.ID).IsRequired().ValueGeneratedOnAdd();
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
                
                entity.HasMany(e => e.AdoptedAnimals)
                      .WithOne(e => e.Adoptive);
            });

            modelBuilder.Entity<Lost>(entity =>
            {
                entity.Property(e => e.ID).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<Death>(entity =>
            {
                entity.Property(e => e.ID).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<Vaccination>(entity =>
            {
                entity.Property(e => e.ID).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<Animal>(entity =>
            {
                entity.Property(e => e.ID).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Name).HasDefaultValue("Bez Imienia");

                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.BirthDate).IsRequired();
                entity.Property(e => e.JoinDate).IsRequired();
                
                entity.HasMany(e => e.Vaccinations)
                    .WithOne(e => e.Animal)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.Property(e => e.Chip).IsRequired();
                entity.HasIndex(e => e.Chip).IsUnique();
                
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.State).IsRequired();
                entity.Property(e => e.Treatments).IsRequired();

                entity.HasOne(e => e.Adoptive)
                    .WithMany(e => e.AdoptedAnimals);

                entity.HasOne(e => e.DeathInfo)
                    .WithOne(e => e.Animal)
                    .HasForeignKey<Death>(e => e.AnimalID);

                entity.HasOne(e => e.LostInfo)
                    .WithOne(e => e.Animal)
                    .HasForeignKey<Lost>(e => e.AnimalID);
            });

            modelBuilder.Entity<ExpiringVaccination>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("View_ExpiringVaccination");
                
                entity.Property(v => v.Name).HasColumnName("Imię");
                entity.Property(v => v.VaccinationDate).HasColumnName("Data szczepienia");
                entity.Property(v => v.ExpireDate).HasColumnName("Data ważności");
            });
        }
    }
}
