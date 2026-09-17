using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Core.Services;
using UrlShortener.Dtos.Url.Request;
using static System.Net.WebRequestMethods;

namespace UrlShortener.Testing.Services.UrlServiceTests
{
    public class CreateTests()
    {
        [Fact]
        public async Task Create_WhenCodeIsUnique()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            repository.Setup(x => x.IsDuplicate(It.IsAny<string>()))
                                   .ReturnsAsync(false);

            repository.Setup(x => x.Create("https://google.com", It.IsAny<string>()))
                      .ReturnsAsync((string url, string shortUrl) => shortUrl);


            UrlService service = new UrlService(repository.Object);

            //Create the dto
            CreateShortUrlRequest request = new CreateShortUrlRequest
            {
                Url = "https://google.com"
            };


            //Call the method inside the UrlService and store the return
            string result = await service.Create(request);


            //Check if the returning url matches what i expect
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_WhenCodeIsADuplicateOnce()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            repository.SetupSequence(x => x.IsDuplicate(It.IsAny<string>()))
                                   .ReturnsAsync(true)
                                   .ReturnsAsync(false);

            repository.Setup(x => x.Create("https://google.com", It.IsAny<string>()))
                      .ReturnsAsync((string url, string shortUrl) => shortUrl);


            UrlService service = new UrlService(repository.Object);

            //Create the dto
            CreateShortUrlRequest request = new CreateShortUrlRequest
            {
                Url = "https://google.com"
            };


            //Call the method inside the UrlService and store the return
            string result = await service.Create(request);

            //Check if the returning url matches what i expect
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_WhenCodeIsADuplicateTwice()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            repository.SetupSequence(x => x.IsDuplicate(It.IsAny<string>()))
                                   .ReturnsAsync(true)
                                   .ReturnsAsync(true);


            UrlService service = new UrlService(repository.Object);

            //Create the dto
            CreateShortUrlRequest request = new CreateShortUrlRequest
            {
                Url = "https://google.com"
            };


            //Call the method inside the UrlService and store the return
            ErrorException exception = await Assert.ThrowsAsync<ErrorException>(() => service.Create(request));

            Assert.Equal(HttpStatusCode.Conflict, exception.Code);
        }

        [Fact]
        public async Task Create_RepoReturnsError()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IUrlRepository> repository = new Mock<IUrlRepository>();

            repository.Setup(x => x.IsDuplicate(It.IsAny<string>()))
                                   .ReturnsAsync(false);

            repository.Setup(x => x.Create("https://google.com", It.IsAny<string>()))
                      .ThrowsAsync(new Exception("Something went wrong creating a UrlEntity"));


            UrlService service = new UrlService(repository.Object);

            //Create the dto
            CreateShortUrlRequest request = new CreateShortUrlRequest
            {
                Url = "https://google.com"
            };


            //Call the method inside the UrlService and store the return
            Exception exception = await Assert.ThrowsAsync<Exception>(() => service.Create(request));

            //Check if the returning url matches what i expect
            Assert.Equal("Something went wrong creating a UrlEntity", exception.Message);
        }
    }
}