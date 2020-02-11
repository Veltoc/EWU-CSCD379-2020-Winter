using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
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
    public class UserControllerTests : BaseApiControllerTests<Data.User, Business.Dto.User, Business.Dto.UserInput>
    {


        protected override Data.User CreateEntity()
            => new Data.User(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString());



        [TestMethod]
        public async Task Get_ReturnsUsers()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Data.User im = CreateEntity();
            context.Users.Add(im);
            context.SaveChanges();

            // Act
            var uri = new Uri("api/User", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            string jsonData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.User[] users =
                JsonSerializer.Deserialize<Business.Dto.User[]>(jsonData, options);
            Assert.AreEqual(10, users.Length);

            Assert.AreEqual(im.Id, users[9].Id);
            Assert.AreEqual(im.FirstName, users[9].FirstName);
            Assert.AreEqual(im.LastName, users[9].LastName);
        }

        [TestMethod]
        public async Task Put_WithMissingId_NotFound()
        {
            // Arrange
            Business.Dto.UserInput im = Mapper.Map<Data.User, Business.Dto.User>(CreateEntity());

            string jsonData = JsonSerializer.Serialize(im);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri("api/User/42", UriKind.RelativeOrAbsolute);
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
            context.Users.Add(entity);
            context.SaveChanges();

            Business.Dto.UserInput userInput = Mapper.Map<User, Business.Dto.User>(entity);
            userInput.FirstName = entity.FirstName += " changed";
            userInput.LastName = entity.LastName += " changed";


            string jsonData = JsonSerializer.Serialize(userInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/User/{entity.Id}", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            string retunedJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.User returnedUser = JsonSerializer.Deserialize<Business.Dto.User>(retunedJson, options);

            // Assert that returnedUser matches im values
            Assert.AreEqual(userInput.FirstName, returnedUser.FirstName);
            Assert.AreEqual(userInput.LastName, returnedUser.LastName);

            // Assert that returnedUser matches database value
            Data.User dataUser = await context.Users.FindAsync(returnedUser.Id);
            Assert.AreEqual(userInput.FirstName, dataUser.FirstName);
            Assert.AreEqual(userInput.LastName, dataUser.LastName);

        }

        [TestMethod]
        public async Task Put_WithInvalid_Fails()
        {
            using ApplicationDbContext context = Factory.GetDbContext();

            Business.Dto.UserInput userInput = Mapper.Map<User, Business.Dto.User>(CreateEntity());
            userInput.FirstName = null;

            string jsonData = JsonSerializer.Serialize(userInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/User/1", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [DataRow(nameof(Business.Dto.UserInput.FirstName))]
        [DataRow(nameof(Business.Dto.UserInput.LastName))]
        public async Task Post_WithoutData_BadResult(string propertyName)
        {
            Data.User entity = CreateEntity();
            Business.Dto.UserInput im = Mapper.Map<User, Business.Dto.User>(entity);
            System.Type inputType = typeof(Business.Dto.UserInput);
            System.Reflection.PropertyInfo? propInfo = inputType.GetProperty(propertyName);
            propInfo?.SetValue(im, null);

            string jsonData = JsonSerializer.Serialize(im);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/User/{entity.Id}", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [TestMethod]
        public async Task Post_Valid_AddsUser()
        {
            // Arrange
            var entity = CreateEntity();
            using ApplicationDbContext context = Factory.GetDbContext();
            context.Users.Add(entity);
            context.SaveChanges();

            Business.Dto.UserInput userInput = Mapper.Map<User, Business.Dto.User>(entity);


            string jsonData = JsonSerializer.Serialize(userInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/User/", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PostAsync(uri, stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            string retunedJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.User returnedUser = JsonSerializer.Deserialize<Business.Dto.User>(retunedJson, options);

            // Assert that returnedUser matches im values
            Assert.AreEqual(userInput.FirstName, returnedUser.FirstName);
            Assert.AreEqual(userInput.LastName, returnedUser.LastName);

            // Assert that returnedUser matches database value
            Data.User dataUser = await context.Users.FindAsync(returnedUser.Id);
            Assert.AreEqual(userInput.FirstName, dataUser.FirstName);
            Assert.AreEqual(userInput.LastName, dataUser.LastName);

        }


    }
}

