using Moq;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Interfaces.Repositories;
using UrlShortener.Core.Interfaces.Services;
using UrlShortener.Core.Services;
using UrlShortener.Dtos.Auth.Request;
using UrlShortener.Dtos.Url.Request;
using UrlShortener.Models;

namespace UrlShortener.Testing.Services.AuthServiceTests
{
    public class RegisterTests
    {
        [Fact]
        public async Task Register_ValidRequest_CreatesUser()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IAuthRepository> repository = new Mock<IAuthRepository>();
            Mock<IAuthenticationService> authenticationService = new Mock<IAuthenticationService>();

            repository.Setup(x => x.UserExistsByEmailOrUsername("NewAccount@Username.com", "NewAccountUsername"))
                      .ReturnsAsync(false);

            AuthService service = new AuthService(repository.Object, authenticationService.Object);



            //Create the dto
            RegisterRequest request = new RegisterRequest
            {
                Username = "NewAccountUsername",
                Email = "NewAccount@Username.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            };


            //Call the method inside the UrlService
            await service.Register(request);


            //Verify that the moq repo actually executes the method with these parameters
            repository.Verify(x =>
                        x.CreateUser(
                            It.Is<User>(u => u.UserName == "NewAccountUsername" && u.Email == "NewAccount@Username.com"),
                            "Test123!"),
                        Times.Once);
        }


        [Fact]
        public async Task Register_DuplicateUser_ThrowsAlreadyExists()
        {
            //Creates mock of the UrlRepository
            Moq.Mock<IAuthRepository> repository = new Mock<IAuthRepository>();
            Mock<IAuthenticationService> authenticationService = new Mock<IAuthenticationService>();

            repository.Setup(x => x.UserExistsByEmailOrUsername("Testing@Testing.com", "Testing"))
                      .ReturnsAsync(true);

            AuthService service = new AuthService(repository.Object, authenticationService.Object);



            //Create the dto
            RegisterRequest request = new RegisterRequest
            {
                Username = "Testing",
                Email = "Testing@Testing.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            };


            //Call the method inside the UrlService
            await Assert.ThrowsAsync<ErrorException>(() => service.Register(request));
        }
    }
}