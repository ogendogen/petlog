﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class Adopter
    {
        public int ID { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Surname { get; set; }
        [MaxLength(64)]
        public string Email { get; set; }
        public int Telephone { get; set; }
        [MaxLength(64)]
        public string City { get; set; }
        [MaxLength(64)]
        public string Street { get; set; }
        [MaxLength(64)]
        public string PostalCode { get; set; }
        public int HouseNumber { get; set; }
        public int? FlatNumber { get; set; }
        public Animal Animal { get; set; }
    }
}
