using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Services;
using UrlShortener.Models;

namespace UrlShortener.Testing.Services.UrlServiceTests
{
    public class DeleteTests
    {
        private Guid userId = new Guid("11111111-1111-1111-1111-111111111111");

        [Fact]
        public async Task Delete_WhenUrlExists()
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

            repository.Setup(x => x.Delete(existingUrl))
                      .Returns(Task.CompletedTask);

            UrlService service = new UrlService(repository.Object);

            await service.Delete("abc123", userId);

            repository.Verify(x => x.Delete(existingUrl), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenUrlDoesNotExist()
        {
            Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            repository.Setup(x => x.GetByShortUrl("abc123", userId))
                      .ReturnsAsync((Models.UrlEntity?)null);

            UrlService service = new UrlService(repository.Object);

            ErrorException exception = await Assert.ThrowsAsync<ErrorException>(
                () => service.Delete("abc123", userId));

            Assert.Equal(HttpStatusCode.NotFound, exception.Code);

            repository.Verify(
                x => x.Delete(It.IsAny<Models.UrlEntity>()),
                Times.Never);
        }

        [Fact]
        public async Task Delete_WhenRepositoryReturnsError()
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

            repository.Setup(x => x.Delete(existingUrl))
                      .ThrowsAsync(
                          new Exception("Something went wrong deleting a UrlEntity"));

            UrlService service = new UrlService(repository.Object);

            Exception exception = await Assert.ThrowsAsync<Exception>(
                () => service.Delete("abc123", userId));

            Assert.Equal(
                "Something went wrong deleting a UrlEntity",
                exception.Message);
        }
    }
}