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
    /// <summary>
    /// Class to manage users' data
    /// </summary>
    public class UsersManager
    {
        /// <summary>
        /// Pet context
        /// </summary>
        internal PetContext Pet { get; set; }

        /// <summary>
        /// Users manager constructor
        /// </summary>
        public UsersManager()
        {
            Pet = new PetContext();
            Pet.Database.EnsureCreated();
        }

        /// <summary>
        /// Users manager destructor
        /// </summary>
        ~UsersManager()
        {
            Pet.Dispose();
        }

        /// <summary>
        /// Login method, veifies login and password provided by user
        /// </summary>
        /// <param name="name">User's name</param>
        /// <param name="password">User's password</param>
        /// <returns>User object for successful login or null if failed</returns>
        public User Login(string name, string password)
        {
            string hashedPassword = HashMD5(password);
            return Pet.Users.FirstOrDefault(user => user.Username == name && user.Password == hashedPassword);
        }

        /// <summary>
        /// Adds new user
        /// </summary>
        /// <param name="username">User's username</param>
        /// <param name="password">User's password in plain text</param>
        /// <param name="name">User's name</param>
        /// <param name="surname">User's surname</param>
        /// <param name="isAdmin">Is user admin?</param>
        /// <returns>User object</returns>
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

        /// <summary>
        /// Hashes all plain passwords
        /// </summary>
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

        /// <summary>
        /// Checks if string is a MD5 hash
        /// </summary>
        /// <param name="password">String to verify (usually password)</param>
        /// <returns>True for MD5 hash, false otherwise</returns>
        private bool IsMD5(string password)
        {
            char[] md5Chars = { 'a', 'b', 'c', 'd', 'e', 'f', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            return password.Length == 32 && password.All(c => md5Chars.Contains(c));
        }

        /// <summary>
        /// Removes user's entity
        /// </summary>
        /// <param name="user">User's object</param>
        public void RemoveUser(User user)
        {
            if (!Pet.Users.Any(dbUser => dbUser.ID == user.ID))
            {
                throw new Exception("Taki użytkownik nie istnieje!");
            }

            Pet.Users.Remove(user);
        }

        /// <summary>
        /// Updates user's entity
        /// </summary>
        /// <param name="user">User's object</param>
        public void UpdateUser(User user)
        {
            if (!Pet.Users.Any(dbUser => dbUser.ID == user.ID))
            {
                throw new Exception("Taki użytkownik nie istnieje!");
            }

            Pet.Users.Update(user);
        }

        /// <summary>
        /// Saves changes to database
        /// </summary>
        /// <returns>Save's result</returns>
        public int SaveChanges()
        {
            return Pet.SaveChanges();
        }

        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns>Collection of users</returns>
        public ObservableCollection<User> GetAllUsers()
        {
            Pet.Users.Load();
            return Pet.Users.Local.ToObservableCollection();
        }

        /// <summary>
        /// Hashes string to MD5
        /// </summary>
        /// <param name="password">String to hash (usually password)</param>
        /// <returns>Hashed string</returns>
        private string HashMD5(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Rollback all unsaved changes in database
        /// </summary>
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
