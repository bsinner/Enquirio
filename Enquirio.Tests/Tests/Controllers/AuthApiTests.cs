using System.Linq;
using System.Threading.Tasks;
using Enquirio.Controllers;
using Enquirio.Models;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {

    public class AuthApiTests : ApiTestUtil {

        // Test logging in with username or email
        [Fact]
        public async Task TestLogin() {
            // Arrange
            var userManager = new Mock<StubUserManager>();
            var signinManager = new Mock<StubSignInManager>();
            var signInResult = new MockSignInResult(true);

            var user = new IdentityUser();
            var models = new [] { AuthData.LoginWithUname, AuthData.LoginWithEmail };
            var passwords = It.IsIn(models.Select(m => m.Password));

            userManager.Setup(u => u.FindByEmailAsync(models[1].Email))
                .ReturnsAsync(user);
            userManager.Setup(u => u.FindByNameAsync(models[0].Username))
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
            userManager.Verify(u => u.FindByEmailAsync(models[1].Email), Times.Once);
            userManager.Verify(u => u.FindByNameAsync(models[0].Username), Times.Once);

            signinManager.VerifyNoOtherCalls();
            userManager.VerifyNoOtherCalls();
        }

        // Test logging in without name, without password, test logging in
        // with name not found, test with password not found
        [Fact]
        public async Task TestLoginError() {
            // Arrange
            var userManager = new Mock<StubUserManager>();
            var signInManager = new Mock<StubSignInManager>();

            var user = new IdentityUser();
            var signInResult = new MockSignInResult(false);

            var model = AuthData.LoginWithUname;
            var model2 = AuthData.LoginWithEmail;
            var invalidModels = new[] {
                AuthData.BadLoginNoPw, AuthData.BadLoginOnlyPw
            };

            userManager.Setup(u => u.FindByNameAsync(model.Username))
                .Returns(Task.FromResult<IdentityUser>(null));
            userManager.Setup(u => u.FindByEmailAsync(model2.Email))
                .ReturnsAsync(user);
            signInManager.Setup(s => s.PasswordSignInAsync
                (user, model2.Password, false, false))
                .ReturnsAsync(signInResult);

            var controller = new AuthApiController(userManager.Object
                , signInManager.Object);

            // Act
            var results = new [] { await controller.Login(model)
                , await controller.Login(model2)
                , await controller.Login(invalidModels[0])
                , await controller.Login(invalidModels[1])
            };

            // Assert
            Assert.All(results, r => Assert.IsType<UnauthorizedResult>(r));

            userManager.Verify(u => u.FindByNameAsync(model.Username)
                , Times.Once);
            userManager.Verify(u => u.FindByEmailAsync(model2.Email)
                , Times.Once);
            signInManager.Verify(s => s.SignOutAsync(), Times.Exactly(4));
            signInManager.Verify(s => s.PasswordSignInAsync
                (user, model2.Password, false, false), Times.Once);

            userManager.VerifyNoOtherCalls();
            signInManager.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task TestLogout() {
            // Arrange
            var signInManager = new Mock<StubSignInManager>();
            var controller = new AuthApiController(
                new Mock<StubUserManager>().Object
                , signInManager.Object
            );

            // Act
            var result = await controller.Logout();

            // Assert
            Assert.IsType<OkResult>(result);

            signInManager.Verify(s => s.SignOutAsync(), Times.Once);
            signInManager.VerifyNoOtherCalls();
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
