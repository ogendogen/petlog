using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    public class User
    {
        public int ID { get; set; }
        [MaxLength(20)]
        public string Username { get; set; }
        [MaxLength(32)]
        public string Password { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Surname { get; set; }
        public bool IsAdmin { get; set; }
    }
}
