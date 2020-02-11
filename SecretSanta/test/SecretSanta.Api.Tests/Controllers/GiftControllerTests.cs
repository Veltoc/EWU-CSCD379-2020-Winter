using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
//using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllerTests : BaseApiControllerTests<Data.Gift, Business.Dto.Gift, Business.Dto.GiftInput>
    {
        protected override Data.Gift CreateEntity()
            => new Data.Gift(Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),  
                Guid.NewGuid().ToString(),
                new Data.User(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));

        [TestMethod]
        public async Task Get_ReturnsGifts()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Data.Gift um = CreateEntity();
            context.Gifts.Add(um);
            context.SaveChanges();

            // Act
            var uri = new Uri("api/Gift", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            string jsonData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift[] gifts =
                JsonSerializer.Deserialize<Business.Dto.Gift[]>(jsonData, options);
            Assert.AreEqual(10, gifts.Length);

            Assert.AreEqual(um.Id, gifts[9].Id);
            Assert.AreEqual(um.Title, gifts[9].Title);
            Assert.AreEqual(um.Description, gifts[9].Description);
            Assert.AreEqual(um.Url, gifts[9].Url);
        }

        [TestMethod]
        public async Task Put_WithMissingId_NotFound()
        {
            // Arrange
            Business.Dto.GiftInput um = Mapper.Map<Data.Gift, Business.Dto.Gift>(CreateEntity());
            string jsonData = JsonSerializer.Serialize(um);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri("api/Gift/42", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_WithId_UpdatesEntity()
        {
            // Arrange
            var entity = CreateEntity();
            using ApplicationDbContext context = Factory.GetDbContext();
            context.Gifts.Add(entity);
            context.SaveChanges();

            Business.Dto.GiftInput giftInput = Mapper.Map<Gift, Business.Dto.Gift>(entity);
            giftInput.Title = entity.Title += " changed";
            giftInput.Description = entity.Description += " changed";
            giftInput.Url = entity.Url += " changed";
      

            string jsonData = JsonSerializer.Serialize(giftInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Gift/{entity.Id}", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            string retunedJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift returnedGift = JsonSerializer.Deserialize<Business.Dto.Gift>(retunedJson, options);

            // Assert that returnedGift matches um values
            Assert.AreEqual(giftInput.Title, returnedGift.Title);
            Assert.AreEqual(giftInput.Description, returnedGift.Description);
            Assert.AreEqual(giftInput.Url, returnedGift.Url);
            Assert.AreEqual(giftInput.UserId, returnedGift.UserId);

            // Assert that returnedGift matches database value
            Data.Gift dataGift = await context.Gifts.FindAsync(returnedGift.Id);
            Assert.AreEqual(giftInput.Title, dataGift.Title);
            Assert.AreEqual(giftInput.Description, dataGift.Description);
            Assert.AreEqual(giftInput.Url, dataGift.Url);
            Assert.AreEqual(giftInput.UserId, dataGift.UserId);
        }

        [TestMethod]
        public async Task Put_WithInvalid_Fails()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            Business.Dto.GiftInput giftInput = Mapper.Map<Gift, Business.Dto.Gift>(CreateEntity());
            giftInput.Title = null;

            string jsonData = JsonSerializer.Serialize(giftInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Gift/1", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [DataRow(nameof(Business.Dto.GiftInput.Title))]
        [DataRow(nameof(Business.Dto.GiftInput.UserId))]
        public async Task Post_WithoutData_BadResult(string propertyName)
        {
            Data.Gift entity = CreateEntity();
            Business.Dto.GiftInput um = Mapper.Map<Gift, Business.Dto.Gift>(entity);
            System.Type inputType = typeof(Business.Dto.GiftInput);
            System.Reflection.PropertyInfo? propInfo = inputType.GetProperty(propertyName);
            propInfo?.SetValue(um, null);

            string jsonData = JsonSerializer.Serialize(um);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Gift/{entity.Id}", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [TestMethod]
        public async Task Post_Valid_AddsGift()
        {
            // Arrange
            var entity = CreateEntity();
            using ApplicationDbContext context = Factory.GetDbContext();
            context.Gifts.Add(entity);
            context.SaveChanges();

            Business.Dto.GiftInput giftInput = Mapper.Map<Gift, Business.Dto.Gift>(entity);


            string jsonData = JsonSerializer.Serialize(giftInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Gift/", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PostAsync(uri, stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            string retunedJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Gift returnedGift = JsonSerializer.Deserialize<Business.Dto.Gift>(retunedJson, options);

            // Assert that returnedGift matches um values
            Assert.AreEqual(giftInput.Title, returnedGift.Title);
            Assert.AreEqual(giftInput.Description, returnedGift.Description);
            Assert.AreEqual(giftInput.Url, returnedGift.Url);
            Assert.AreEqual(giftInput.UserId, returnedGift.UserId);

            // Assert that returnedGift matches database value
            Data.Gift dataGift = await context.Gifts.FindAsync(returnedGift.Id);
            Assert.AreEqual(giftInput.Title, dataGift.Title);
            Assert.AreEqual(giftInput.Description, dataGift.Description);
            Assert.AreEqual(giftInput.Url, dataGift.Url);
            Assert.AreEqual(giftInput.UserId, dataGift.UserId);

        }


    }
}
