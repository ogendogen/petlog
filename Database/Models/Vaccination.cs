using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class Vaccination
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Animal Animal { get; set; }
        public Vaccination()
        {

        }
    }
}
