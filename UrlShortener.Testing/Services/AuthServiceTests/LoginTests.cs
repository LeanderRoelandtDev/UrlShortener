using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Core.Services;
using UrlShortener.Dtos.Auth.Request;
using UrlShortener.Dtos.Auth.Response;
using UrlShortener.Models;

namespace UrlShortener.Testing.Services.AuthServiceTests
{
    public class LoginTests
    {
        [Fact]
        public async Task Login_ValidCredentials_ReturnsLoginResponse()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IAuthRepository> repository = new Mock<IAuthRepository>();
            Mock<IAuthenticationService> authenticationService = new Mock<IAuthenticationService>();


            User user = new User
            {
                ID = Guid.Parse("01a0f2bb-16fc-72b4-ab29-c7bf90403704"),
                UserName = "Testing",
                Email = "Testing@Testing.com"
            };

            repository.Setup(x => x.GetUserByUserName(user.UserName))
                      .ReturnsAsync(user);


            repository.Setup(x => x.CheckPassword(user.ID, "Test123!"))
                      .ReturnsAsync(true);

            authenticationService.Setup(x => x.GenerateJWTToken(It.IsAny<User>()))
                                 .ReturnsAsync("valid JWT");
            
            AuthService service = new AuthService(repository.Object, authenticationService.Object);



            //Create the dto
            LoginRequest request = new LoginRequest
            {
                UserName = "Testing",
                Password = "Test123!",
            };


            //Call the method inside the UrlService
            LoginResponse response = await service.Login(request);


            Assert.NotNull(response);
            Assert.Equal("valid JWT", response.JwtToken);
        }


        [Fact]
        public async Task Login_UserNotFound_ThrowsNotFound()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IAuthRepository> repository = new Mock<IAuthRepository>();
            Mock<IAuthenticationService> authenticationService = new Mock<IAuthenticationService>();


            repository.Setup(x => x.GetUserByUserName("Testing"))
                      .ReturnsAsync((User?) null);

            AuthService service = new AuthService(repository.Object, authenticationService.Object);



            //Create the dto
            LoginRequest request = new LoginRequest
            {
                UserName = "Testing",
                Password = "Test123!",
            };


            //Call the method inside the UrlService
            ErrorException exception = await Assert.ThrowsAsync<ErrorException>(
                () => service.Login(request));

            Assert.Equal(HttpStatusCode.NotFound, exception.Code);
        }


        [Fact]
        public async Task Login_InvalidPassword_ThrowsUnauthorized()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IAuthRepository> repository = new Mock<IAuthRepository>();
            Mock<IAuthenticationService> authenticationService = new Mock<IAuthenticationService>();


            User user = new User
            {
                ID = Guid.Parse("01a0f2bb-16fc-72b4-ab29-c7bf90403704"),
                UserName = "Testing",
                Email = "Testing@Testing.com"
            };

            repository.Setup(x => x.GetUserByUserName(user.UserName))
                      .ReturnsAsync(user);


            repository.Setup(x => x.CheckPassword(user.ID, "Test123!"))
                      .ReturnsAsync(false);

            AuthService service = new AuthService(repository.Object, authenticationService.Object);



            //Create the dto
            LoginRequest request = new LoginRequest
            {
                UserName = "Testing",
                Password = "Test123!",
            };


            //Call the method inside the UrlService
            ErrorException exception = await Assert.ThrowsAsync<ErrorException>(
                () => service.Login(request));

            Assert.Equal(HttpStatusCode.Unauthorized, exception.Code);
        }
    }
}