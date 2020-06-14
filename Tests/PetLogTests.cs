using System.Linq;
using Database;
using NUnit.Framework;

namespace Tests
{
    /// <summary>
    /// Pet log test class
    /// </summary>
    public class PetLogTests
    {
        /// <summary>
        /// Setup tests method
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Test for sorted adoptives method
        /// </summary>
        [Test]
        public void SortedAdoptivesTest()
        {
            AnimalsManager animalsManager = new AnimalsManager();
            var sortedAdoptives = animalsManager.GetAllAdoptivesInAlphabeticalOrder();

            Assert.AreEqual(sortedAdoptives.First().Name, "asd");
            Assert.AreEqual(sortedAdoptives.First().Surname, "asd");

            Assert.AreEqual(sortedAdoptives.Last().Name, "Jan");
            Assert.AreEqual(sortedAdoptives.Last().Surname, "Kowalski");
        }

        /// <summary>
        /// Test for updating user name
        /// </summary>
        [Test]
        public void ChangeUserNameTest()
        {
            UsersManager usersManager = new UsersManager();
            var users = usersManager.GetAllUsers();
            
            var mainUser = users.First(user => user.Name == "aaa");
            mainUser.Name = "test test";
            usersManager.UpdateUser(mainUser);

            users = usersManager.GetAllUsers();
            Assert.AreNotEqual(users.FirstOrDefault(user => user.Name == "test test"), null);
        }
    }
}