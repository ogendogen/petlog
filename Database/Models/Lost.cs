using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    /// <summary>
    /// Lost info's model
    /// </summary>
    public class Lost
    {
        /// <summary>
        /// Lost info's ID (primary key, auto incremented value)
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Date of lost
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Description of lost
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Lost animal (foreign key)
        /// </summary>
        public Animal Animal { get; set; }
        /// <summary>
        /// Animal's ID associated with lost info
        /// </summary>
        public int AnimalID { get; set; }
    }
}
