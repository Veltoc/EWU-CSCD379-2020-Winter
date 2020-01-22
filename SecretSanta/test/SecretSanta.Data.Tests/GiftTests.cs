using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class GiftTests : TestBase
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetTitleToNull_ThrowsArgumentNullException()
        {
            _ = new Gift {
                Title = null!,
                Description = "a cookbook for dummies",
                Url = "www.dummies.com",
                User = new User()
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetDescriptionToNull_ThrowsArgumentNullException()
        {
            _ = new Gift {
                Title = "Cooking For Dummies",
                Description = null!,
                Url = "www.dummies.com",
                User = new User()
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Gift_SetUrlToNull_ThrowsArgumentNullException()
        {
            _ = new Gift {
                Title = "Cooking For Dummies",
                Description = "a cookbook for dummies",
                Url = null!,
                User = new User()
            };
        }

        [TestMethod]
        public async Task CreateGift_ShouldSaveIntoDatabase()
        {
            int giftId = -1;
            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                var gift = new Gift {
                    Title = "Cooking For Dummies",
                    Description = "a cookbook for dummies",
                    Url = "www.dummies.com",
                    User = new User()     
                };
                applicationDbContext.Gifts.Add(gift);

                await applicationDbContext.SaveChangesAsync();
                giftId = gift.Id;
            }
            using (var applicationDbContext = new ApplicationDbContext(Options))
            {
                Gift gift = await applicationDbContext.Gifts.Where(g => g.Id == giftId).SingleOrDefaultAsync();

                    
                Assert.AreEqual("Cooking For Dummies", gift.Title);
                Assert.AreEqual("a cookbook for dummies", gift.Description);
                Assert.AreEqual("www.dummies.com", gift.Url);
            }
            //Gift gift = new Gift(1, null!, "Montoya", new List<Gift>());
        }

        [TestMethod]
        public async Task CreateGift_ShouldSetFingerPrintDataOnInitialSave()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "dummies"));

            int giftId = -1;
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = new Gift {
                    Title = "Cooking For Dummies",
                    Description = "a cookbook for dummies",
                    Url = "www.dummies.com",
                    User = new User()
                };
                applicationDbContext.Gifts.Add(gift);

                var gift2 = new Gift {
                    Title = "Cooking For Dummies",
                    Description = "a cookbook for dummies",
                    Url = "www.dummies.com",
                    User = new User()
                };
                applicationDbContext.Gifts.Add(gift2);

                await applicationDbContext.SaveChangesAsync();
                giftId = gift.Id;
            }

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Gift gift = await applicationDbContext.Gifts.Where(u => u.Id == giftId).SingleOrDefaultAsync();

                Assert.AreEqual("Cooking For Dummies", gift.Title);
                Assert.AreEqual("a cookbook for dummies", gift.Description);
                Assert.AreEqual("www.dummies.com", gift.Url);
            }
        }

        [TestMethod]
        public async Task CreateGift_ShouldSetFingerPrintDataOnUpdate()
        {
            IHttpContextAccessor httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "dummies"));

            int giftId = -1;
            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                var gift = new Gift {
                    Title = "Cooking For Dummies",
                    Description = "a cookbook for dummies",
                    Url = "www.dummies.com",
                    User = new User()
                };
                applicationDbContext.Gifts.Add(gift);

                var gift2 = new Gift { 
                    Title = "Cooking For Dummies",
                    Description = "a cookbook for dummies",
                    Url = "www.dummies.com",
                    User = new User()
                };
                applicationDbContext.Gifts.Add(gift2);

                await applicationDbContext.SaveChangesAsync();
                giftId = gift.Id;
            }

            // change the gift that is updating the record
            httpContextAccessor = Mock.Of<IHttpContextAccessor>(hta =>
                hta.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == new Claim(ClaimTypes.NameIdentifier, "boring"));

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                // Since we are pulling back the record from the database and making changes to it, we don't need to re-add it to the collection
                // thus no Gifts.Add call, that is only needed when new records are inserted
                Gift gift = await applicationDbContext.Gifts.Where(u => u.Id == giftId).SingleOrDefaultAsync();
                gift.Title = "The Most Boring Book Ever";
                   

                await applicationDbContext.SaveChangesAsync();
            }

            using (var applicationDbContext = new ApplicationDbContext(Options, httpContextAccessor))
            {
                Gift gift = await applicationDbContext.Gifts.Where(u => u.Id == giftId).SingleOrDefaultAsync();

                Assert.IsNotNull(gift);
                Assert.AreEqual("dummies", gift.CreatedBy);
                Assert.AreEqual("boring", gift.ModifiedBy);
            }
        }

        [TestMethod]
        public async Task AddGift_WithUser_ShouldCreateForeignRelationship()
        {
            var gift = new Gift {
                Title = "Cooking For Dummies",
                Description = "a cookbook for dummies",
                Url = "www.dummies.com",
                User = new User(),
                CreatedBy = "blightyear",
                ModifiedBy = "blightyear"
            };

            var user = new User {
               
                FirstName = "Buzz",
                LastName = "lightyear",
                Gifts = new List<Gift>(),
                CreatedBy = "blightyear",
                ModifiedBy = "blightyear"
            };

            using (var dbContext = new ApplicationDbContext(Options))
            {
                gift.User = user;
                dbContext.Gifts.Add(gift);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(Options))
            {
                List<Gift> gifts = await dbContext.Gifts.Include(g => g.User).ToListAsync();
                Assert.AreEqual(1, gifts.Count);    
                Assert.AreEqual(gift.Title, gifts[0].Title);
                Assert.AreEqual(gift.Description, gifts[0].Description);
                Assert.AreNotEqual(0, gifts[0].Id); 
            }
        }
    }
}