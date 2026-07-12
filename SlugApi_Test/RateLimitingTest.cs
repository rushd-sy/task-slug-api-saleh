using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SlugApi.DTOs;

namespace SlugApi.Test
{
    public class RateLimitingTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public RateLimitingTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Post_Generate_SendMoreThan5RequestPerSecond_Returns429TooManyRequests()
        {
            var request = new GenerateSlugRequest(Text: "Hello World", Separator: '-');

            HttpResponseMessage? lastResponse = null;
            for (int i = 0; i < 6; i++)
                lastResponse = await _client.PostAsJsonAsync("api/v1/slugs", request);

            Assert.Equal(HttpStatusCode.TooManyRequests, lastResponse!.StatusCode);
        }
    }
}
