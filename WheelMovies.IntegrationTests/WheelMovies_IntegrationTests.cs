using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WheelMovies.Business.DTO;
using Xunit;

namespace WheelMovies.IntegrationTests
{
    public class WheelMovies_IntegrationTests
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public WheelMovies_IntegrationTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task GetMovies_Returns_ValidMoviesResponse()
        {
            // 1. Arrange
            var client = factory.CreateClient();

            // 2. Act
            var response = await client.GetAsync("/api/movies");

            // 3. Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetMoviesForUser_Returns_NotFound_For_No_UserId()
        {
            // 1. Arrange
            var client = factory.CreateClient();

            // 2. Act
            var response = await client.GetAsync("/api/movies/user");

            // 3. Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetMoviesByCriteriaRequest_For_ValidRequest_Returns_MoviesList()
        {
            // 1. Arrange
            var client = factory.CreateClient();
            var request = new GetMoviesByCriteriaRequest
            {
                RunningTime = 120
            };
            var json = JsonConvert.SerializeObject(request);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            // 2. Act
            var response = await client.PostAsync("/api/movies", payload);

            // 3. Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
