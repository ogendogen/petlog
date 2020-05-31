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
        internal VetContext Vet { get; set; }
        public User Login(string name, string password)
        {
            string hashedPassword = HashMD5(password);
            return Vet.Users.FirstOrDefault(user => user.Name == name && user.Password == hashedPassword);
        }

        public User AddNewUser(string username,
                               string password,
                               string name,
                               string surname,
                               bool isAdmin)
        {
            string hashedPassword = HashMD5(password);
            if (Vet.Users.Any(user => user.Username == username))
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

            return Vet.Users.Add(user).Entity;
        }

        public void RemoveUser(User user)
        {
            if (!Vet.Users.Any(dbUser => dbUser.ID == user.ID))
            {
                throw new Exception("Taki użytkownik nie istnieje!");
            }

            Vet.Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
            if (!Vet.Users.Any(dbUser => dbUser.ID == user.ID))
            {
                throw new Exception("Taki użytkownik nie istnieje!");
            }

            Vet.Users.Update(user);
        }

        public int SaveChanges()
        {
            return Vet.SaveChanges();
        }

        private string HashMD5(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }
    }
}
