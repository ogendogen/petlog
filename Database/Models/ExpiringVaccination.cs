using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    /// <summary>
    /// Expiring vaccination's model
    /// </summary>
    public class ExpiringVaccination
    {
        /// <summary>
        /// Expiring vaccination animal's name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Expiring vaccination date (when applied)
        /// </summary>
        public DateTime VaccinationDate { get; set; }
        /// <summary>
        /// Expiring vaccination expire date
        /// </summary>
        public DateTime ExpireDate { get; set; }
    }
}
