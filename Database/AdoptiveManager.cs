using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Database.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Database
{
    /// <summary>
    /// Class to manage adoptives' data
    /// </summary>
    public class AdoptiveManager
    {
        /// <summary>
        /// Pet context
        /// </summary>
        internal PetContext Pet { get; set; }
        /// <summary>
        /// Adoptive manager constructor, creates pet context
        /// </summary>
        public AdoptiveManager()
        {
            Pet = new PetContext();
            Pet.Database.EnsureCreated();
        }

        /// <summary>
        /// Adding new adoptive to database
        /// </summary>
        /// <param name="name">Adoptive's name</param>
        /// <param name="surname">Adoptive's surname</param>
        /// <param name="email">Adoptive's email</param>
        /// <param name="telephone">Adoptive's telephone</param>
        /// <param name="city">Adoptive's city</param>
        /// <param name="street">Adoptive's street</param>
        /// <param name="postalCode">Adoptive's postal code</param>
        /// <param name="houseNumber">Adoptive's house number</param>
        /// <param name="adoptedAnimals">Adoptive's adopted animals</param>
        /// <param name="flatNumber">Adoptive's flat number</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updating certain adoptive
        /// </summary>
        /// <param name="adoptive">Adoptive's object</param>
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

        /// <summary>
        /// Removing adoptive
        /// </summary>
        /// <param name="adoptive">Adoptive's object</param>
        public void RemoveAdoptive(Adoptive adoptive)
        {
            if (!Pet.Adoptives.Any(dbAdoptive => dbAdoptive.ID == adoptive.ID))
            {
                throw new Exception("Taka osoba nie istnieje!");
            }

            Pet.Adoptives.Remove(adoptive);
        }

        /// <summary>
        /// Returing adoptive's adopted animals collection 
        /// </summary>
        /// <param name="adoptive">Adoptive's object</param>
        /// <returns>Collection of animals</returns>
        public IQueryable<Animal> GetAdoptiveAnimals(Adoptive adoptive)
        {
            return Pet.Animals.Where(animal => animal.Adoptive.ID == adoptive.ID);
        }

        /// <summary>
        /// Saves changes to database
        /// </summary>
        /// <returns>Saving result</returns>
        public int SaveChanges()
        {
            return Pet.SaveChanges();
        }
    }
}
