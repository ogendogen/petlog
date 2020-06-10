using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Database.Models;
using Microsoft.EntityFrameworkCore;
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

        ~UsersManager()
        {
            Pet.Dispose();
        }

        public User Login(string name, string password)
        {
            string hashedPassword = HashMD5(password);
            return Pet.Users.FirstOrDefault(user => user.Username == name && user.Password == hashedPassword);
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

        public void HashPlainPasswords()
        {
            foreach (var user in Pet.Users)
            {
                if (!IsMD5(user.Password))
                {
                    user.Password = HashMD5(user.Password);
                }
            }
            Pet.SaveChanges();
        }

        private bool IsMD5(string password)
        {
            char[] md5Chars = { 'a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            return password.Length == 32 && password.All(c => md5Chars.Contains(c));
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

        public ObservableCollection<User> GetAllUsers()
        {
            Pet.Users.Load();
            return Pet.Users.Local.ToObservableCollection();
        }

        private string HashMD5(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        public void RollBack()
        {
            var changedEntries = Pet.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch(entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
         }
    }
}
