using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public abstract class BaseApiControllerTests<TEntity, TDto,TInputDto>
        where TEntity : EntityBase
        where TDto : class, TInputDto
        where TInputDto : class
    {
#nullable disable
        protected SecretSantaWebApplicationFactory Factory { get; set; } 
        protected HttpClient Client { get; set; }
#nullable enable
        protected IMapper Mapper { get; set; } = AutomapperConfigurationProfile.CreateMapper();

        //protected abstract BaseApiController<TDto,TInputDto> CreateController(TService service);

        protected abstract TEntity CreateEntity();
        //protected abstract TInputDto CreateInput();

        [TestInitialize]
        public void TestSetup()
        {
            Factory = new SecretSantaWebApplicationFactory();

            using ApplicationDbContext context = Factory.GetDbContext();
            context.Database.EnsureCreated();

            Client = Factory.CreateClient();

            SeedData();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            Factory.Dispose();
        }
        public void SeedData()
        {
            using ApplicationDbContext context = Factory.GetDbContext();
            for (int i = 0; i < 9; i++)
            {
                TEntity entity = CreateEntity();
                context.Add(entity);
                context.SaveChanges();
                Console.WriteLine(context.Entry(entity).Entity.Id);
            }
        }
        /*   [TestMethod]
           public async Task Get_ReturnsGifts()
           {
               using ApplicationDbContext context = Factory.GetDbContext();
               context.Gifts.Add(SampleData)

               HttpClient client = Factory.CreateClient();

               HttpResponseMessage response = await client.GetAsync("api/Gift");//GetAsync(string) use the abstract for a string per to point for this

               response.EnsureSuccessStatusCode();
           }


           [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_RequiresService()
        {
            new ThrowingController();
        }

        [TestMethod]
        public async Task Get_FetchesAllItems()
        {
            TService service = new TService();
            service.Items.Add(CreateEntity());
            service.Items.Add(CreateEntity());
            service.Items.Add(CreateEntity());

            BaseApiController<TDto,TInputDto> controller = CreateController(service);

            IEnumerable<TEntity> items = await controller.Get();

            CollectionAssert.AreEqual(service.Items.ToList(), items.ToList());
        }

        [TestMethod]
        public async Task Get_WhenEntityDoesNotExist_ReturnsNotFound()
        {
            TService service = new TService();
            BaseApiController<TDto, TInputDto> controller = CreateController(service);

            IActionResult result = await controller.Get(1);

            Assert.IsTrue(result is NotFoundResult);
        }


        [TestMethod]
        public async Task Get_WhenEntityExists_ReturnsItem()
        {
            TService service = new TService();
            TEntity entity = CreateEntity();
            service.Items.Add(entity);
            BaseApiController<TDto, TInputDto> controller = CreateController(service);

            IActionResult result = await controller.Get(entity.Id);

            var okResult = result as OkObjectResult;
            
            Assert.AreEqual(entity, okResult?.Value);
        }

        [TestMethod]
        public async Task Put_UpdatesItem()
        {
            TService service = new TService();
            TEntity entity1 = CreateEntity();
            service.Items.Add(entity1);
            TEntity entity2 = CreateEntity();
            BaseApiController<TDto, TInputDto> controller = CreateController(service);

            TEntity? result = await controller.Put(entity1.Id, entity2);

            Assert.AreEqual(entity2, result);
            Assert.AreEqual(entity2, service.Items.Single());
        }

        [TestMethod]
        public async Task Post_InsertsItem()
        {
            TService service = new TService();
            TEntity entity = CreateEntity();
            BaseApiController<TDto, TInputDto> controller = CreateController(service);

            TEntity? result = await controller.Post(entity);

            Assert.AreEqual(entity, result);
            Assert.AreEqual(entity, service.Items.Single());
        }

        [TestMethod]
        public async Task Delete_WhenItemDoesNotExist_ReturnsNotFound()
        {
            TService service = new TService();
            BaseApiController<TDto, TInputDto> controller = CreateController(service);

            IActionResult result = await controller.Delete(1);

            Assert.IsTrue(result is NotFoundResult);
        }

        [TestMethod]
        public async Task Delete_WhenItemExists_ReturnsOk()
        {
            TService service = new TService();
            TEntity entity = CreateEntity();
            service.Items.Add(entity);
            BaseApiController<TDto, TInputDto> controller = CreateController(service);

            IActionResult result = await controller.Delete(entity.Id);

            Assert.IsTrue(result is OkResult);
        }

        private class ThrowingController : BaseApiController<TEntity>
        {
            public ThrowingController() : base(null!)
            { }
        }
           */

    }
}
