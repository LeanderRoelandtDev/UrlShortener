using Moq;
using System.Net;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Services;

namespace UrlShortener.Testing.Services.UrlServiceTests
{
    public class GetOriginalUrlTests
    {
        [Fact]
        public async Task GetOriginalUrl_ReturnsUrlWhenAvaiable()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            repository.Setup(x => x.GetOriginalUrl("qwertyui"))
                      .ReturnsAsync("https://google.com");

            UrlService service = new UrlService(repository.Object);

            //Call the method inside the UrlService
            string result = await service.GetOriginalUrl("qwertyui");

            //Check if the returning url matches what i expect
            Assert.Equal("https://google.com", result);
        }

        [Fact]
        public async Task GetOriginalUrl_ThrowsNotFound_WhenUrlDoesNotExist()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            repository.Setup(x => x.GetOriginalUrl("qwertyui"))
                      .ReturnsAsync((string?)null);

            UrlService urlService = new UrlService(repository.Object);

            //Call the method inside the UrlService
            ErrorException exception = await Assert.ThrowsAsync<ErrorException>(
                () => urlService.GetOriginalUrl("qwertyui"));

            Assert.Equal(HttpStatusCode.NotFound, exception.Code);
        }
    }
}