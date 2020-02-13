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
        protected abstract TEntity CreateEntity();

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

    }
}
