using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class ExpiringVaccination
    {
        public string Name { get; set; }
        public DateTime VaccinationDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
