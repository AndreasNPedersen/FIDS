using Fly.Models.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Fly.Tests.IntegrationTest
{
    [TestClass]
    public class FlyControllerTest
    {
        private HttpClient _httpClient;
        private WebApplicationFactory<Program> _applicationFactory;
        public FlyControllerTest() {
            _applicationFactory = new WebApplicationFactory<Program>();
            _httpClient = _applicationFactory.CreateClient();
        }

        [TestMethod]
        public void TestGetOneAirplane()
        {
            //this cannot be done because of the direct use of docker and environment variables
            AirplaneDto payload = new AirplaneDto();
            var response =  _httpClient.PostAsJsonAsync("/api/airplane", payload).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
