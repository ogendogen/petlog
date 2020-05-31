using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Death
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Animal Animal { get; set; }
    }
}
