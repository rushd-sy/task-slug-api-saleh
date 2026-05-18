using Microsoft.AspNetCore.Mvc.Testing;
using SlugApi.DTOs;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SlugApi.Test
{
    public class SlugsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SlugsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_Generate_ValideRequest_200Ok()
        {
            var request = new GenerateSlugRequest("Hello World", '_');
            var response = await _client.PostAsJsonAsync("api/v1/Slugs", request);
            var responseAsObject = await response.Content.ReadFromJsonAsync<GenerateSlugResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseAsObject);
            Assert.Equal("hello_world", responseAsObject.Slug);

        }
        [Fact]
        public async Task Post_Generate_ValideRequestWithOutSeparator_200Ok()
        {
            var request = new { Text = "Hello World" };
            var response = await _client.PostAsJsonAsync("api/v1/Slugs", request);
            var responseAsObject = await response.Content.ReadFromJsonAsync<GenerateSlugResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(responseAsObject);
            Assert.Equal("hello-world", responseAsObject.Slug);

        }
        [Fact]
        public async Task Post_Generate_EmptyString_400BadRequest()
        {
            var request = new GenerateSlugRequest("", '-');
            var response = await _client.PostAsJsonAsync("api/v1/Slugs", request);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseAsJson = JsonDocument.Parse(responseAsString).RootElement;

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(responseAsJson.TryGetProperty("errors", out var errors));
            Assert.True(errors.TryGetProperty("Text", out _));
        }

        [Fact]
        public async Task Post_Generate_InvalidSeparator_Returns400BadRequest()
        {
            var request = new { Text = "Hello World", Separator = '@' };
            var response = await _client.PostAsJsonAsync("/api/v1/slugs", request);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseAsJson = JsonDocument.Parse(responseAsString).RootElement;

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(responseAsJson.TryGetProperty("errors", out var errors));
            Assert.True(errors.TryGetProperty("Separator", out _));
        }
    }
}
