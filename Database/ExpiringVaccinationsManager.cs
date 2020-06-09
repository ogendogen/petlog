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

        public List<ExpiringVaccination> GetExpiringVaccinations()
        {
            return Pet.ExpiringVaccination.ToList();
        }
    }
}
