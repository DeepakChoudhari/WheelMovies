using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
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
    }
}
