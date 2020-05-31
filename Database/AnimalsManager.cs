using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Database
{
    public class AnimalsManager
    {
        internal PetContext Vet { get; set; }

        public AnimalsManager()
        {
            Vet = new PetContext();
            Vet.Database.EnsureCreated();
        }

        public List<Animal> GetAllAnimals()
        {
            return Vet.Animals.ToList();
        }

        public Animal GetAnimalById(int id)
        {
            return Vet.Animals.FirstOrDefault(animal => animal.ID == id);
        }

        public Animal AddNewAnimal(AnimalType type,
                                 DateTime birthDate,
                                 DateTime joinDate,
                                 ICollection<Vaccination> vaccinations,
                                 int chip,
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

            if (Vet.Animals.Any(animal => animal.Chip == chip))
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

            return Vet.Animals.Add(animal).Entity;
        }

        public void UpdateAnimal(Animal animal)
        {
            if (!Vet.Animals.Any(animal => animal.ID == animal.ID))
            {
                throw new Exception("Taki zwierzak nie istnieje!");
            }

            Vet.Animals.Update(animal);
        }

        public void RemoveAnimal(Animal animal)
        {
            if (!Vet.Animals.Any(animal => animal.ID == animal.ID))
            {
                throw new Exception("Taki zwierzak nie istnieje!");
            }

            Vet.Animals.Remove(animal);
        }

        public int SaveChanges()
        {
            return Vet.SaveChanges();
        }
    }
}
