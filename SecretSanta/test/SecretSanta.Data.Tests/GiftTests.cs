using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
        public class GiftTests : TestBase
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public async Task CreateGift_ShouldSaveIntoDatabase()
            {
                int giftId = -1;
                using (var applicationDbContext = new ApplicationDbContext(Options))
                {
                var gift = new Gift
                {
                    Id = 1,
                    Title = "Cooking For Dummies",
                    Description = "a cookbook for dummies",
                    Url = "www.dummies.com",
                    User = new User()
                        
                    };
                    applicationDbContext.Gifts.Add(gift);

                    await applicationDbContext.SaveChangesAsync();

                }
                using (var applicationDbContext = new ApplicationDbContext(Options))
                {
                    var gift = await applicationDbContext.Gifts.Where(g => g.Id == giftId).SingleOrDefaultAsync();

                    
                    Assert.AreEqual("Cooking For Dummies", gift.Title);
                    Assert.AreEqual("a cookbook for dummies", gift.Description);
                    Assert.AreEqual("www.dummies.com", gift.Url);
                    Assert.IsNotNull(gift.User);

                }


                //Gift gift = new Gift(1, null!, "Montoya", new List<Gift>());
            }

            [TestMethod]
            public async Task CreateGift_ShouldSetFingerPrintDataOnInitialSave()
            {
                IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                    hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "dummies"));

                int giftId = -1;
                // Arrange
                using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
                {
                    var gift = new Gift
                    {
                        Id = 1,
                        Title = "Cooking For Dummies",
                        Description = "a cookbook for dummies",
                        Url = "www.dummies.com"
                    };
                    applicationDbContext.Gifts.Add(gift);

                    var gift2 = new Gift
                    {
                        Id = 2,
                        Title = "Cooking For Dummies",
                        Description = "a cookbook for dummies",
                        Url = "www.dummies.com"
                    };
                    applicationDbContext.Gifts.Add(gift2);

                    await applicationDbContext.SaveChangesAsync();

                    giftId = gift.Id;
                }

                // Act
                // Assert
                using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
                {
                    var gift = await applicationDbContext.Gifts.Where(u => u.Id == giftId).SingleOrDefaultAsync();

                Assert.AreEqual("Cooking For Dummies", gift.Title);
                Assert.AreEqual("a cookbook for dummies", gift.Description);
                Assert.AreEqual("www.dummies.com", gift.Url);
                Assert.IsNotNull(gift.User);
            }
            }

            [TestMethod]
            public async Task CreateGift_ShouldSetFingerPrintDataOnUpdate()
            {
                IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                    hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "dummies"));

                int giftId = -1;
                // Arrange
                using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
                {
                    var gift = new Gift
                    {
                        Id = 1,
                        Title = "Cooking For Dummies",
                        Description = "a cookbook for dummies",
                        Url = "www.dummies.com"
                    };
                    applicationDbContext.Gifts.Add(gift);

                    var gift2 = new Gift
                    {
                        Id = 2,
                        Title = "Cooking For Dummies",
                        Description = "a cookbook for dummies",
                        Url = "www.dummies.com"
                    };
                    applicationDbContext.Gifts.Add(gift2);

                    await applicationDbContext.SaveChangesAsync();

                    giftId = gift.Id;
                }

                // Act
                // change the gift that is updating the record
                httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                    hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "boring"));
                using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
                {
                    // Since we are pulling back the record from the database and making changes to it, we don't need to re-add it to the collection
                    // thus no Gifts.Add call, that is only needed when new records are inserted
                    var gift = await applicationDbContext.Gifts.Where(u => u.Id == giftId).SingleOrDefaultAsync();
                    gift.Title = "The Most Boring Book Ever";
                   

                    await applicationDbContext.SaveChangesAsync();
                }
                // Assert
                using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
                {
                    var gift = await applicationDbContext.Gifts.Where(u => u.Id == giftId).SingleOrDefaultAsync();

                    Assert.IsNotNull(gift);
                    Assert.AreEqual("dummies", gift.CreatedBy);
                    Assert.AreEqual("boring", gift.ModifiedBy);
                }
         }
    }
}