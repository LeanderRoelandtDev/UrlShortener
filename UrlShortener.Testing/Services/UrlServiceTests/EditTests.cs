using System.Net;
using Moq;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Services;
using UrlShortener.Dtos.Url.Request;
using UrlShortener.Infrastructure.DomainModels;

namespace UrlShortener.Testing.Services.UrlServiceTests
{
    public class EditTests
    {
        private Guid userId = new Guid("11111111-1111-1111-1111-111111111111");

        [Fact]
        public async Task Edit_WhenUrlExistsAndBelongsToUser()
        {
            Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            Models.UrlEntity existingUrl = new Models.UrlEntity
            {
                Id = Guid.NewGuid(),
                ShortUrl = "abc123",
                OriginalUrl = "https://google.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = userId
            };

            repository.Setup(x => x.GetByShortUrl("abc123", userId))
                      .ReturnsAsync(existingUrl);

            repository.Setup(x => x.Update(It.IsAny<Models.UrlEntity>()))
                      .Returns(Task.CompletedTask);

            UrlService service = new UrlService(repository.Object);

            EditUrlEntityRequest request = new EditUrlEntityRequest
            {
                ShortUrl = "abc123",
                NewUrl = "https://youtube.com"
            };

            await service.Edit(request, userId);

            Assert.Equal("https://youtube.com", existingUrl.OriginalUrl);

            repository.Verify(x => x.Update(existingUrl), Times.Once);
        }


        [Fact]
        public async Task Edit_WhenUrlDoesNotExist()
        {
            Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            repository.Setup(x => x.GetByShortUrl("abc123", userId))
                      .ReturnsAsync((Models.UrlEntity?)null);

            UrlService service = new UrlService(repository.Object);

            EditUrlEntityRequest request = new EditUrlEntityRequest
            {
                ShortUrl = "abc123",
                NewUrl = "https://youtube.com"
            };

            ErrorException exception = await Assert.ThrowsAsync<ErrorException>(
                () => service.Edit(request, userId));

            Assert.Equal(HttpStatusCode.NotFound, exception.Code);

            repository.Verify(
                x => x.Update(It.IsAny<Models.UrlEntity>()),
                Times.Never);
        }


        [Fact]
        public async Task Edit_WhenRepositoryReturnsError()
        {
            Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            Models.UrlEntity existingUrl = new Models.UrlEntity
            {
                Id = Guid.NewGuid(),
                ShortUrl = "abc123",
                OriginalUrl = "https://google.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = userId
            };

            repository.Setup(x => x.GetByShortUrl("abc123", userId))
                      .ReturnsAsync(existingUrl);

            repository.Setup(x => x.Update(It.IsAny<Models.UrlEntity>()))
                      .ThrowsAsync(new Exception(
                          "Something went wrong updating a UrlEntity"));

            UrlService service = new UrlService(repository.Object);

            EditUrlEntityRequest request = new EditUrlEntityRequest
            {
                ShortUrl = "abc123",
                NewUrl = "https://youtube.com"
            };

            Exception exception = await Assert.ThrowsAsync<Exception>(
                () => service.Edit(request, userId));

            Assert.Equal(
                "Something went wrong updating a UrlEntity",
                exception.Message);
        }
    }
}