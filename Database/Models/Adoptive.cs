using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    /// <summary>
    /// Adoptive's model
    /// </summary>
    public class Adoptive
    {
        /// <summary>
        /// Adoptive's ID, primary key, auto incremented value
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Adoptive's name (maxlength 32)
        /// </summary>
        [MaxLength(32)]
        public string Name { get; set; }
        /// <summary>
        /// Adoptive's surname (maxlength 32)
        /// </summary>
        [MaxLength(32)]
        public string Surname { get; set; }
        /// <summary>
        /// Adoptive's email (maxlength 64, unique)
        /// </summary>
        [MaxLength(64)]
        public string Email { get; set; }
        /// <summary>
        /// Adoptive's telephone
        /// </summary>
        public int Telephone { get; set; }
        /// <summary>
        /// Adoptive's city (maxlength 64)
        /// </summary>
        [MaxLength(64)]
        public string City { get; set; }
        /// <summary>
        /// Adoptive's street (maxlength 64)
        /// </summary>
        [MaxLength(64)]
        public string Street { get; set; }
        /// <summary>
        /// Adoptive's postal code (maxlength 64)
        /// </summary>
        [MaxLength(64)]
        public string PostalCode { get; set; }
        /// <summary>
        /// Adoptive's house number
        /// </summary>
        public int HouseNumber { get; set; }
        /// <summary>
        /// Adoptive's flat number (optional, accepts nulls)
        /// </summary>
        public int? FlatNumber { get; set; }
        /// <summary>
        /// Adoptive's adopted animals collection
        /// </summary>
        public virtual ICollection<Animal> AdoptedAnimals { get; set; }

        /// <summary>
        /// Adoptive's constructor, initialize adopted animals collection
        /// </summary>
        public Adoptive()
        {
            AdoptedAnimals = new List<Animal>();
        }

        /// <summary>
        /// Overloaded ToString method, returns Name+space+Surname
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} {Surname}";
        }
    }
}
