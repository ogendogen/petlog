using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Database.Models
{
    /// <summary>
    /// User's model
    /// </summary>
    public class User
    {
        /// <summary>
        /// User's ID (primary key, auto incremented value)
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// User's username/login (maxlength 20)
        /// </summary>
        [MaxLength(20)]
        public string Username { get; set; }
        /// <summary>
        /// User's password (maxlength 32)
        /// </summary>
        [MaxLength(32)]
        public string Password { get; set; }
        /// <summary>
        /// User's name (maxlength 32)
        /// </summary>
        [MaxLength(32)]
        public string Name { get; set; }
        /// <summary>
        /// User's surname (maxlength 32)
        /// </summary>
        [MaxLength(32)]
        public string Surname { get; set; }
        /// <summary>
        /// User's admin flag (true = admin, false = user)
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
