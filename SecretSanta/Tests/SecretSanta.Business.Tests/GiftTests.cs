using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftTests
    {
        [TestMethod]
        public void Create_Gift_Success()
        {

            //arrange
            const int id = 01101;
            const string title = "Bike";
            const string description = "Wrapped like a boat, but its a bike";
            const string url = "www.something.com";
            User user = new User(id, "02", "03", new List<Gift>());
            //const User user;

            //act
            Gift gift = new Gift(id, title, description, url, user);
            //assert 
            Assert.AreEqual<int>(id, gift.Id, "Id value is unexpected");
            Assert.AreEqual<string>(title, gift.Title, "Title value is unexpected");
            Assert.AreEqual<string>(description, gift.Description, "Description value is unexpected");
            Assert.AreEqual<string>(url, gift.Url, "Url value is unexpected");
            Assert.AreEqual<User>(user, gift.User, "User is unexpected");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_VerifyPropertiesAreNotNull_NotNull()
        {
            Gift gift = new Gift(0, "","","", new User(0, "02", "03", new List<Gift>()));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Properties_AssignNull_ThrowArgumentNullException()
        {
            Gift gift = new Gift(0, "1", "1", "1", new User(0, "02", "03", new List<Gift>()));
            gift.Title = "";
            gift.Description = null;
            gift.Url = null;
        }
    }
}
