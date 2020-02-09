using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Api;

namespace SecretSanta.Web.Controllers
{
    public class GiftController : Controller
    {
        public GiftController(IHttpClientFactory clientFactory)
        {
          if(clientFactory is null)
            {
                throw new System.ArgumentNullException(nameof(clientFactory));
            }
            ClientFactory = clientFactory;
        }

        public IHttpClientFactory ClientFactory { get; }

        public async Task<ActionResult> Index()
        {
            HttpClient httpClient = ClientFactory.CreateClient("SantaApi"); ;
            var client = new GiftClient(httpClient);
            ICollection<Gift> gifts = await client.GetAllAsync();
            return View(gifts);
        }
    }
}