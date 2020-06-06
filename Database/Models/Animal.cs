using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class Animal
    {
        public int ID { get; set; }
        public AnimalType Type { get; set; }
                [MaxLength(64)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime JoinDate { get; set; }
        public virtual ICollection<Vaccination> Vaccinations { get; set; }
        [MaxLength(15)]
        public string Chip { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string Treatments { get; set; }
        #nullable enable
        public Adoptive? Adoptive { get; set; }
        public Death? DeathInfo { get; set; }
        public Lost? LostInfo { get; set; }
    }
}
