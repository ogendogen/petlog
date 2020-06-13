using System.Linq;
using Database;
using NUnit.Framework;

namespace Tests
{
    public class PetLogTests
    {
        [SetUp]
        public void Setup()
        {
        }

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