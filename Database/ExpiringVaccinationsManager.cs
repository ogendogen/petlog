using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database.Models;

namespace Database
{
    /// <summary>
    /// Class to manage expiring vaccinations' data
    /// </summary>
    public class ExpiringVaccinationsManager
    {
        /// <summary>
        /// Pet context
        /// </summary>
        internal PetContext Pet { get; set; }

        /// <summary>
        /// Expiring vaccination manager constructor, creates pet context
        /// </summary>
        public ExpiringVaccinationsManager()
        {
            Pet = new PetContext();
            Pet.Database.EnsureCreated();
        }
        
        /// <summary>
        /// Returns expiring vaccinations
        /// </summary>
        /// <returns>Collection of expiring vaccinations</returns>
        public List<ExpiringVaccination> GetExpiringVaccinations()
        {
            return Pet.ExpiringVaccination.ToList();
        }

        /// <summary>
        /// Returns amount of expiring vaccinations
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Pet.ExpiringVaccination.Count();
        }
    }
}
