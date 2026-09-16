using Moq;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Core.Services;

namespace UrlShortener.Testing.Services
{
    public class UrlServiceTests
    {
        [Fact]
        public async Task GetOriginalUrl_ReturnsUrlWhenAvaiable()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<UrlShortener.Core.Interfaces.Repositories.IUrlRepository> repository = new Mock<IUrlRepository>();

            repository.Setup(x => x.GetOriginalUrl("qwertyui"))
                      .ReturnsAsync("https://google.com");

            UrlService service = new UrlService(repository.Object);

            //Call the method inside the UrlService
            string result = await service.GetOriginalUrl("qwertyui");

            //Check if the returning url matches what i expect
            Assert.Equal("https://google.com", result);
        }
    }
}