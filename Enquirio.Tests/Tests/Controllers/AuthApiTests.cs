using System.Linq;
using System.Threading.Tasks;
using Enquirio.Controllers;
using Enquirio.Models;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Enquirio.Tests.Tests.Controllers {

    // SignInResult's succeeded field defaults to false 
    // and can't be overriden by Moq
    public class TestSignInResult : SignInResult {
        public new bool Succeeded = true;
    }

    public class AuthApiTests : ApiTestUtil {

        [Fact]
        public async Task TestLogin() {
            // Arrange
            var userManager = new Mock<UserManagerStub>();
            var signinManager = new Mock<SignInManagerStub>();
            var signInResult = new TestSignInResult();

            var user = new IdentityUser();
            var models = new [] { AuthData.LoginNoEmail, AuthData.LoginNoUname };
            var passwords = It.IsIn(models.Select(m => m.Password));

            userManager.Setup(s => s.FindByEmailAsync(models[1].Email))
                .ReturnsAsync(user);
            userManager.Setup(s => s.FindByNameAsync(models[0].Username))
                .ReturnsAsync(user);
            signinManager.Setup
                (s => s.PasswordSignInAsync(user, passwords, false, false))
                .ReturnsAsync(signInResult);

            var controller = new AuthApiController(
                    userManager.Object, signinManager.Object);

            // Act
            var result = await controller.Login(models[0]);
            await controller.Login(models[1]);

            // Assert
            Assert.IsType<OkResult>(result);

            signinManager.Verify(s => s.SignOutAsync(), Times.Exactly(2));
            signinManager.Verify(s => s.PasswordSignInAsync(user, passwords, false, false)
                , Times.Exactly(2));
            userManager.Verify(s => s.FindByEmailAsync(models[1].Email), Times.Once);
            userManager.Verify(s => s.FindByNameAsync(models[0].Username), Times.Once);

            signinManager.VerifyNoOtherCalls();
            userManager.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task TestLoginError() {
            // Arrange
            // Act
            // Assert
        }

        [Fact]
        public async Task TestLogout() {
            // Arrange
            // Act
            // Assert
        }

        [Fact]
        public void HttpVerbTests() {
            Assert.True(HasAttribute(nameof(AuthApiController.Login)
                    , typeof(HttpPostAttribute)));
            Assert.True(HasAttribute(nameof(AuthApiController.Logout)
                    , typeof(HttpPostAttribute)));
        }
    }
}
