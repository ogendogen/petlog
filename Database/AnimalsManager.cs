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
    public class AnimalsManager
    {
        internal PetContext Pet { get; set; }

        public AnimalsManager()
        {
            Pet = new PetContext();
            Pet.Database.EnsureCreated();
        }

        public List<Animal> GetAllAnimals()
        {
            return Pet.Animals.ToList();
        }

        public Animal GetAnimalById(int id)
        {
            return Pet.Animals.FirstOrDefault(animal => animal.ID == id);
        }

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

            return Pet.Animals.Add(animal).Entity;
        }

        public IEnumerable<Adoptive> GetAllAdoptivesInAlphabeticalOrder()
        {
            return Pet.Adoptives.OrderBy(adoptive => adoptive.Name).ThenBy(adoptive => adoptive.Surname).ToList();
        }

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

            if (Pet.Animals.Any(dbAnimal => dbAnimal.Chip == animal.Chip))
            {
                throw new Exception("Taki numer chip już istnieje w bazie!");
            }

            Pet.Animals.Update(animal);
        }

        public void RemoveAnimal(Animal animal)
        {
            if (!Pet.Animals.Any(animal => animal.ID == animal.ID))
            {
                throw new Exception("Taki zwierzak nie istnieje!");
            }

            Pet.Animals.Remove(animal);
        }

        public ObservableCollection<Animal> Load()
        {
            Pet.Animals.Include(animal => animal.Adoptive)
                       .Include(animal => animal.DeathInfo)
                       .Include(animal => animal.LostInfo)
                       .Include(animal => animal.Vaccinations).Load();
            return Pet.Animals.Local.ToObservableCollection();
        }

        public ObservableCollection<Vaccination> GetAnimalVaccinations()
        {
            Pet.Vaccination.Include(vacc => vacc.Animal).Load();
            return Pet.Vaccination.Local.ToObservableCollection();
        }

        public int SaveChanges()
        {
            return Pet.SaveChanges();
        }
    }
}
