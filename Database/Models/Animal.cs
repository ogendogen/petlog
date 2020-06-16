using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    /// <summary>
    /// Animal's model
    /// </summary>
    public class Animal
    {
        /// <summary>
        /// Animal's ID, primary key, auto incremented value
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Animal's type (dog, cat, other)
        /// </summary>
        public AnimalType Type { get; set; }
        /// <summary>
        /// Animal's name (maxlength 64)
        /// </summary>
        [MaxLength(64)]
        public string Name { get; set; }
        /// <summary>
        /// Animal's birth date
        /// </summary>
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// Animal's join date
        /// </summary>
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// Animal's vaccinations collection
        /// </summary>
        public virtual ICollection<Vaccination> Vaccinations { get; set; }
        /// <summary>
        /// Animal's chip (maxlength 15, unique, only digits)
        /// </summary>
        [MaxLength(15)]
        public string Chip { get; set; }
        /// <summary>
        /// Animal's general description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Animal's state description
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Animal's treatments description
        /// </summary>
        public string Treatments { get; set; }
        #nullable enable
        /// <summary>
        /// Animal's adoptive (optional, accepts null, foreign key)
        /// </summary>
        public Adoptive? Adoptive { get; set; }
        /// <summary>
        /// Animal's death info (optional, accepts null)
        /// </summary>
        public Death? DeathInfo { get; set; }
        /// <summary>
        /// Animal's lost info (optional, accepts null)
        /// </summary>
        public Lost? LostInfo { get; set; }

        /// <summary>
        /// Overrided ToString method, returns Name
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return Name;
        }
    }
}
