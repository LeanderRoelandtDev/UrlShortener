using Moq;
using System.Net;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Services;
using UrlShortener.Dtos.Url.Request;
using UrlShortener.Dtos.Url.Response;

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
                ShortUrl = "abcd1234",
                OriginalUrl = "https://google.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = userId
            };

            Models.UrlEntity updatedFromDb = new Models.UrlEntity
            {
                Id = existingUrl.Id,
                ShortUrl = "abcd1234",
                OriginalUrl = "https://youtube.com",
                UserId = userId,
                CreatedAt = existingUrl.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                ClickCount = 0
            };

            repository.Setup(x => x.GetByShortUrl("abcd1234", userId))
                      .ReturnsAsync(existingUrl);

            repository.Setup(x => x.Update(It.IsAny<Models.UrlEntity>()))
                      .ReturnsAsync((Models.UrlEntity ToUpdate) => updatedFromDb);

            UrlService service = new UrlService(repository.Object);

            EditUrlEntityRequest request = new EditUrlEntityRequest
            {
                ShortUrl = "abcd1234",
                NewUrl = "https://youtube.com"
            };

            UrlResponse result = await service.Edit(request, userId);

            Assert.Equal("https://youtube.com", existingUrl.OriginalUrl);

            Assert.Equal(updatedFromDb.Id, result.Id);
            Assert.Equal("abcd1234", result.ShortUrl);
            Assert.Equal("https://youtube.com", result.OriginalUrl);
            Assert.Equal(updatedFromDb.CreatedAt, result.CreatedAt);
            Assert.Equal(updatedFromDb.UpdatedAt, result.UpdatedAt);
            Assert.Equal(0, result.ClickCount);

            repository.Verify(x => x.Update(existingUrl), Times.Once);
        }


        [Fact]
        public async Task Edit_WhenUrlDoesNotExist()
        {
            Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            repository.Setup(x => x.GetByShortUrl("abcd1234", userId))
                      .ReturnsAsync((Models.UrlEntity?)null);

            UrlService service = new UrlService(repository.Object);

            EditUrlEntityRequest request = new EditUrlEntityRequest
            {
                ShortUrl = "abcd1234",
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
                ShortUrl = "abcd1234",
                OriginalUrl = "https://google.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = userId
            };

            repository.Setup(x => x.GetByShortUrl("abcd1234", userId))
                      .ReturnsAsync(existingUrl);

            repository.Setup(x => x.Update(It.IsAny<Models.UrlEntity>()))
                      .ThrowsAsync(new Exception(
                          "Something went wrong updating a UrlEntity"));

            UrlService service = new UrlService(repository.Object);

            EditUrlEntityRequest request = new EditUrlEntityRequest
            {
                ShortUrl = "abcd1234",
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