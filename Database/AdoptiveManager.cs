using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Database.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Database
{
    public class AdoptiveManager
    {
        internal PetContext Pet { get; set; }
        public AdoptiveManager()
        {
            Pet = new PetContext();
            Pet.Database.EnsureCreated();
        }

        public Adoptive AddNewAdoptive(string name,
                                       string surname,
                                       string email,
                                       int telephone,
                                       string city,
                                       string street,
                                       string postalCode,
                                       int houseNumber,
                                       ICollection<Animal> adoptedAnimals,
                                       int? flatNumber = null)
        {
            if (Pet.Adoptives.Any(adoptive => adoptive.Email == email))
            {
                throw new Exception("Ten email już jest wykorzystany!");
            }

            Adoptive adoptive = new Adoptive()
            {
                Name = name,
                Surname = surname,
                Email = email,
                Telephone = telephone,
                City = city,
                Street = street,
                PostalCode = postalCode,
                HouseNumber = houseNumber,
                AdoptedAnimals = adoptedAnimals,
                FlatNumber = flatNumber
            };
            
            return Pet.Adoptives.Add(adoptive).Entity;
        }

        public void UpdateAdoptive(Adoptive adoptive)
        {
            if (!Pet.Adoptives.Any(dbAdoptive => dbAdoptive.ID == adoptive.ID))
            {
                throw new Exception("Taka osoba nie istnieje!");
            }

            if (Pet.Adoptives.Any(dbAdoptive => dbAdoptive.Email == adoptive.Email))
            {
                throw new Exception("Ten email już jest wykorzystany!");
            }

            Pet.Adoptives.Update(adoptive);
        }

        public void RemoveAdoptive(Adoptive adoptive)
        {
            if (!Pet.Adoptives.Any(dbAdoptive => dbAdoptive.ID == adoptive.ID))
            {
                throw new Exception("Taka osoba nie istnieje!");
            }

            Pet.Adoptives.Remove(adoptive);
        }

        public IQueryable<Animal> GetAdoptiveAnimals(Adoptive adoptive)
        {
            return Pet.Animals.Where(animal => animal.Adoptive.ID == adoptive.ID);
        }

        public int SaveChanges()
        {
            return Pet.SaveChanges();
        }
    }
}
