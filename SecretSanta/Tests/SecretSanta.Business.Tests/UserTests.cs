using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Create_Gift_Success()
        {

            //arrange
            const int id = 01101;
            const string firstName = "Bike";
            const string lastName = "Wrapped like a boat, but its a bike";
            List<Gift> gifts = new List<Gift>();

            //const User user;

            //act
            User user = new User(id, firstName, lastName, gifts);
            //assert 
            Assert.AreEqual<int>(id, user.Id, "Id value is unexpected");
            Assert.AreEqual<string>(firstName, user.FirstName, "FirstName value is unexpected");
            Assert.AreEqual<string>(lastName, user.LastName, "LastName value is unexpected");
            Assert.AreEqual<List<Gift>>(gifts, user.Gifts, "Gifts value is unexpected");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_VerifyPropertiesAreNotNull_NotNull()
        {
            User user = new User(0, "", "", new List<Gift>());
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Properties_AssignNull_ThrowArgumentNullException()
        {
            User user = new User(0, "1", "1", new List<Gift>());
            user.FirstName = "";
            user.LastName = null;
        }
    }
}
