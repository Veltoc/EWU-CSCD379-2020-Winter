using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class UserGroupTest : TestBase
    {
        [TestMethod]
        public async Task CreateUserWithManyGroups()
        {
            // Arrange
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "imontoya"));

            var user = new User
            {
                Id = 1,
                FirstName = "Buzz",
                LastName = "lightyear",
                Gifts = new List<Gift>()
            };
            var group1 = new Group
            {
                Name = "Andys toys"
            };
            var group2 = new Group
            {
                Name = "Classroom"
            };

            // Act

            user.UserGroups = new List<UserGroup>();
            user.UserGroups.Add(new UserGroup { User = user, Group = group1 });
            user.UserGroups.Add(new UserGroup { User = user, Group = group2 });

            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            // Assert
            using (ApplicationDbContext dbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var retrievedUser = await dbContext.Users.Where(u => u.Id == user.Id)
                    .Include(p => p.UserGroups).ThenInclude(ug => ug.Group).SingleOrDefaultAsync();

                Assert.IsNotNull(retrievedUser);
                Assert.AreEqual(2, retrievedUser.UserGroups.Count);
                Assert.IsNotNull(retrievedUser.UserGroups[0].Group);
                Assert.IsNotNull(retrievedUser.UserGroups[1].Group);
            }
        }
    }
}
