using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;

namespace Database
{
    /// <summary>
    /// Class to manage animals' data
    /// </summary>
    public class AnimalsManager
    {
        /// <summary>
        /// Pet context
        /// </summary>
        internal PetContext Pet { get; set; }

        /// <summary>
        /// Animals manager constructor, creates pet context
        /// </summary>
        public AnimalsManager()
        {
            Pet = new PetContext();
            Pet.Database.EnsureCreated();
        }

        /// <summary>
        /// Get all animals
        /// </summary>
        /// <returns>Collection of animals</returns>
        public List<Animal> GetAllAnimals()
        {
            return Pet.Animals.ToList();
        }

        /// <summary>
        /// Selecting animal by id
        /// </summary>
        /// <param name="id">Animal's id</param>
        /// <returns>Animal's object</returns>
        public Animal GetAnimalById(int id)
        {
            return Pet.Animals.FirstOrDefault(animal => animal.ID == id);
        }

        /// <summary>
        /// Adding new animal to database
        /// </summary>
        /// <param name="name">Animal's name</param>
        /// <param name="type">Animal's type (dog, cat, other)</param>
        /// <param name="birthDate">Animal's birth date</param>
        /// <param name="joinDate">Animal's join date</param>
        /// <param name="vaccinations">Animal's vaccinations</param>
        /// <param name="chip">Animal's chip</param>
        /// <param name="description">Animal's description</param>
        /// <param name="state">Animal's state description</param>
        /// <param name="treatments">Animal's treatments description</param>
        /// <param name="adoptive">Adoptive's object associated with animal (default null)</param>
        /// <param name="deathInfo">Death's object associated with animal (default null)</param>
        /// <param name="lostInfo">Lost information object's associated with animal (default null)</param>
        /// <returns></returns>
        public Animal AddNewAnimal(string name,
                                 AnimalType type,
                                 DateTime birthDate,
                                 DateTime joinDate,
                                 ICollection<Vaccination> vaccinations,
                                 string chip,
                                 string description,
                                 string state,
                                 string treatments,
                                 Adoptive adoptive = null,
                                 Death deathInfo = null,
                                 Lost lostInfo = null)
        {
            var now = DateTime.Now;

            if (birthDate > now)
            {
                throw new Exception("Data urodzenia jest z przyszłości!");
            }

            if (joinDate > now)
            {
                throw new Exception("Data dołączenia jest z przyszłości!");
            }

            if (chip.ToString().Length != 15)
            {
                throw new Exception("Numer chip nie składa się z 15 cyfr!");
            }

            if (Pet.Animals.Any(animal => animal.Chip == chip))
            {
                throw new Exception("Taki numer chip już istnieje w bazie!");
            }

            if (deathInfo != null && deathInfo.Date > now)
            {
                throw new Exception("Data śmierci jest z przyszłości!");
            }

            if (lostInfo != null && lostInfo.Date > now)
            {
                throw new Exception("Data utraty jest z przyszłości!");
            }

            Animal animal = new Animal()
            {
                Name = name,
                Type = type,
                BirthDate = birthDate,
                JoinDate = joinDate,
                Vaccinations = vaccinations,
                Chip = chip,
                Description = description,
                State = state,
                Treatments = treatments,
                Adoptive = adoptive,
                DeathInfo = deathInfo,
                LostInfo = lostInfo
            };

            Animal addedAnimal;
            using (var transaction = Pet.Database.BeginTransaction())
            {
                try
                {
                    addedAnimal = Pet.Animals.Add(animal).Entity;
                    Pet.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return addedAnimal;
        }

        /// <summary>
        /// Returns collection of all adoptives in alphabetical order
        /// </summary>
        /// <returns>Collection of all adoptives in alphabetical order</returns>
        public IEnumerable<Adoptive> GetAllAdoptivesInAlphabeticalOrder()
        {
            return Pet.Adoptives.OrderBy(adoptive => adoptive.Name).ThenBy(adoptive => adoptive.Surname).ToList();
        }

        /// <summary>
        /// Updates animal's entity
        /// </summary>
        /// <param name="animal">Animal's object</param>
        public void UpdateAnimal(Animal animal)
        {
            if (!Pet.Animals.Any(dbAnimal => dbAnimal.ID == animal.ID))
            {
                throw new Exception("Taki zwierzak nie istnieje!");
            }

            if (animal.Chip.ToString().Length != 15)
            {
                throw new Exception("Numer chip nie składa się z 15 cyfr!");
            }

            if (Pet.Animals.Any(dbAnimal => dbAnimal.Chip == animal.Chip && dbAnimal.ID != animal.ID))
            {
                throw new Exception("Taki numer chip już istnieje w bazie!");
            }

            using var transaction = Pet.Database.BeginTransaction();
            try
            {
                Pet.Animals.Update(animal);
                Pet.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Removes animal entity
        /// </summary>
        /// <param name="animal">Animal's object</param>
        public void RemoveAnimal(Animal animal)
        {
            if (!Pet.Animals.Any(animalDb => animalDb.ID == animal.ID))
            {
                throw new Exception("Taki zwierzak nie istnieje!");
            }

            Pet.Animals.Remove(animal);
        }

        /// <summary>
        /// Loads all animals entity and joins associated entites like adoptive, death info, lost info and vaccinations
        /// </summary>
        /// <returns>Collection of animals</returns>
        public ObservableCollection<Animal> Load()
        {
            Pet.Animals.Include(animal => animal.Adoptive)
                       .Include(animal => animal.DeathInfo)
                       .Include(animal => animal.LostInfo)
                       .Include(animal => animal.Vaccinations).Load();
            return Pet.Animals.Local.ToObservableCollection();
        }

        /// <summary>
        /// Returns all vaccinations
        /// </summary>
        /// <returns>Collection of vaccinations</returns>
        public ObservableCollection<Vaccination> GetAnimalVaccinations()
        {
            Pet.Vaccination.Include(vacc => vacc.Animal).Load();
            return Pet.Vaccination.Local.ToObservableCollection();
        }

        /// <summary>
        /// Saves changes to database
        /// </summary>
        /// <returns>Result of saving changes</returns>
        public int SaveChanges()
        {
            return Pet.SaveChanges();
        }

        /// <summary>
        /// Removes death info's entity
        /// </summary>
        /// <param name="deathInfo">Death's info object</param>
        public void RemoveDeath(Death deathInfo)
        {
            if (!Pet.Death.Any(deathDb => deathDb.ID == deathInfo.ID))
            {
                throw new Exception("Taki obiekt zgonu nie istnieje!");
            }

            Pet.Death.Remove(deathInfo);
        }

        /// <summary>
        /// Removes lost info's entity
        /// </summary>
        /// <param name="lostInfo">Lost info object</param>
        public void RemoveLost(Lost lostInfo)
        {
            if (!Pet.Lost.Any(lostDb => lostDb.ID == lostInfo.ID))
            {
                throw new Exception("Taki obiekt zaginienia nie istnieje!");
            }

            Pet.Lost.Remove(lostInfo);
        }
    }
}
