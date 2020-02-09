using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class GroupControllerTests : BaseApiControllerTests<Data.Group, Business.Dto.Group, Business.Dto.Group>
    {

        protected override Data.Group CreateEntity()
            => new Data.Group(Guid.NewGuid().ToString());

        protected Business.Dto.GroupInput CreateInputDto()
        {
            return new Business.Dto.GroupInput
            {
                Title = Guid.NewGuid().ToString()
            };
        }

        [TestMethod]
        public async Task Get_ReturnsGroups()
        {
            // Arrange
            using ApplicationDbContext context = Factory.GetDbContext();
            Data.Group gm = CreateEntity();
            context.Groups.Add(gm);
            context.SaveChanges();

            // Act
            var uri = new Uri("api/Group", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            string jsonData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Group[] groups =
                JsonSerializer.Deserialize<Business.Dto.Group[]>(jsonData, options);
            Assert.AreEqual(10, groups.Length);

            Assert.AreEqual(gm.Id, groups[9].Id);
            Assert.AreEqual(gm.Title, groups[9].Title);

        }

        [TestMethod]
        public async Task Put_WithMissingId_NotFound()
        {
            // Arrange
            //Business.Dto.GroupInput gm = Mapper.Map<Data.Group, Business.Dto.Group>(CreateEntity());
            Business.Dto.GroupInput gm = CreateInputDto();
            string jsonData = JsonSerializer.Serialize(gm);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri("api/Group/42", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Put_WithId_UpdatesEntity()
        {
            // Arrange
            //var entity = CreateEntity();
            using ApplicationDbContext context = Factory.GetDbContext();
            //context.Groups.Add(entity);
            //context.SaveChanges();

            Group entity = await context.Groups.FirstOrDefaultAsync();
            Business.Dto.GroupInput groupInput = CreateInputDto();//Mapper.Map<Group, Business.Dto.Group>(entity);
            groupInput.Title = entity.Title += " changed";


            string jsonData = JsonSerializer.Serialize(groupInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Group/{entity.Id}", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            string retunedJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Group returnedGroup = JsonSerializer.Deserialize<Business.Dto.Group>(retunedJson, options);

            // Assert that returnedGroup matches gm values
            Assert.AreEqual(groupInput.Title, returnedGroup.Title);
            // Assert that returnedGroup matches database value
            Data.Group dataGroup = await context.Groups.FindAsync(entity.Id);
            Assert.AreEqual(groupInput.Title, dataGroup.Title);
        }

        [TestMethod]
        public async Task Put_WithInvalid_Fails()
        {
            using ApplicationDbContext context = Factory.GetDbContext();

            Business.Dto.GroupInput groupInput = CreateInputDto();//Mapper.Map<Group, Business.Dto.Group>(entity);
            groupInput.Title = null;

            string jsonData = JsonSerializer.Serialize(groupInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Group/1", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [DataRow(nameof(Business.Dto.GroupInput.Title))]
        public async Task Post_WithoutData_BadResult(string propertyName)
        {
            Data.Group entity = CreateEntity();
            Business.Dto.GroupInput gm = Mapper.Map<Group, Business.Dto.Group>(entity);
            System.Type inputType = typeof(Business.Dto.GroupInput);
            System.Reflection.PropertyInfo? propInfo = inputType.GetProperty(propertyName);
            propInfo?.SetValue(gm, null);

            string jsonData = JsonSerializer.Serialize(gm);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Group/{entity.Id}", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PutAsync(uri, stringContent);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [TestMethod]
        public async Task Post_Valid_AddsGroup()
        {
            // Arrange
            //var entity = CreateEntity();
            using ApplicationDbContext context = Factory.GetDbContext();
            //context.Groups.Add(entity);
            //context.SaveChanges();

            Business.Dto.GroupInput groupInput = CreateInputDto();//Mapper.Map<Group, Business.Dto.Group>(entity);


            string jsonData = JsonSerializer.Serialize(groupInput);

            using StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Act
            Uri uri = new Uri($"api/Group/", UriKind.RelativeOrAbsolute);
            HttpResponseMessage response = await Client.PostAsync(uri, stringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            string retunedJson = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Business.Dto.Group returnedGroup = JsonSerializer.Deserialize<Business.Dto.Group>(retunedJson, options);

            // Assert that returnedGroup matches gm values
            Assert.AreEqual(groupInput.Title, returnedGroup.Title);

            // Assert that returnedGroup matches database value
            Data.Group dataGroup = await context.Groups.FindAsync(returnedGroup.Id);
            Assert.AreEqual(groupInput.Title, dataGroup.Title);

        }


    }
}

