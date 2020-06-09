using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Models;

namespace Database
{
    public class ExpiringVaccinationsManager
    {
        internal PetContext Pet { get; set; }

        public ExpiringVaccinationsManager()
        {
            Pet = new PetContext();
            Pet.Database.EnsureCreated();
        }
        public List<ExpiringVaccination> GetExpiringVaccinations()
        {
            return Pet.ExpiringVaccination.ToList();
        }
    }
}
