using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    /// <summary>
    /// Vaccination's model
    /// </summary>
    public class Vaccination
    {
        /// <summary>
        /// Vaccination's ID (primary key, auto incremented value)
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Vaccination's date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Vaccination's description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Vaccinated animal (foreign key)
        /// </summary>
        public Animal Animal { get; set; }
        public Vaccination()
        {
            Date = DateTime.Today;
        }
    }
}
