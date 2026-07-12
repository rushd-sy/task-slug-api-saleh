using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using SlugApi.DTOs;

namespace SlugApi.Test
{
    public class CachTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CachTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task Post_Generate_SendSameRequest_SecondCallIsCacheHit()
        {
            var request = new GenerateSlugRequest("Hello World", '-');

            var firstResponse = await _client.PostAsJsonAsync("api/v1/Slugs", request);
            var secondResponse = await _client.PostAsJsonAsync("api/v1/Slugs", request);

            var firstBody = await firstResponse.Content.ReadFromJsonAsync<GenerateSlugResult>();
            var secondBody = await secondResponse.Content.ReadFromJsonAsync<GenerateSlugResult>();

            Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);

            Assert.Equal("MISS", firstResponse.Headers.GetValues("X-Cache").Single());
            Assert.Equal("HIT", secondResponse.Headers.GetValues("X-Cache").Single());

            Assert.Equal(firstBody!.Response.Slug, secondBody!.Response.Slug);
        }


    }
}
