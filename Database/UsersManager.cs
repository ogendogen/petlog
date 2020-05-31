using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Database.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Database
{
    public class UsersManager
    {
        internal PetContext Pet { get; set; }

        public UsersManager()
        {
            Pet = new PetContext();
            Pet.Database.EnsureCreated();
        }

        public User Login(string name, string password)
        {
            string hashedPassword = HashMD5(password);
            return Pet.Users.FirstOrDefault(user => user.Name == name && user.Password == hashedPassword);
        }

        public User AddNewUser(string username,
                               string password,
                               string name,
                               string surname,
                               bool isAdmin)
        {
            string hashedPassword = HashMD5(password);
            if (Pet.Users.Any(user => user.Username == username))
            {
                throw new Exception("Taki login jest już zajęty!");
            }
            
            User user = new User(){
                Username = username,
                Password = hashedPassword,
                Name = name,
                Surname = surname,
                IsAdmin = isAdmin
            };

            return Pet.Users.Add(user).Entity;
        }

        public void RemoveUser(User user)
        {
            if (!Pet.Users.Any(dbUser => dbUser.ID == user.ID))
            {
                throw new Exception("Taki użytkownik nie istnieje!");
            }

            Pet.Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
            if (!Pet.Users.Any(dbUser => dbUser.ID == user.ID))
            {
                throw new Exception("Taki użytkownik nie istnieje!");
            }

            Pet.Users.Update(user);
        }

        public int SaveChanges()
        {
            return Pet.SaveChanges();
        }

        private string HashMD5(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }
    }
}
