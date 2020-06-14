using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    /// <summary>
    /// Animal's death model
    /// </summary>
    public class Death
    {
        /// <summary>
        /// Death's ID (primary key, auto incremented value)
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Death's date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Death's description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Animal associated with death (foreign key)
        /// </summary>
        public Animal Animal { get; set; }
        /// <summary>
        /// Associated animal's id
        /// </summary>
        public int AnimalID { get; set; }
    }
}
