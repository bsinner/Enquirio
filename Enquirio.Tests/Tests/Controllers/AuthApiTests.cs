using System.Linq;
using System.Threading.Tasks;
using Enquirio.Controllers;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Enquirio.Tests.Tests.Controllers {

    public class AuthApiTests : ApiTestUtil {

        // Test logging in with username or email
        [Fact]
        public async Task TestLogin() {
            // Arrange
            var userManager = new Mock<StubUserManager>();
            var signinManager = new Mock<StubSignInManager>();

            var user = new IdentityUser();
            var models = new [] { AuthData.LoginWithUname, AuthData.LoginWithEmail };
            var pws = new[] { models[0].Password, models[1].Password };

            userManager.Setup(u => u.FindByEmailAsync(models[1].Email))
                .ReturnsAsync(user);
            userManager.Setup(u => u.FindByNameAsync(models[0].Username))
                .ReturnsAsync(user);
            signinManager.Setup
                (s => s.PasswordSignInAsync(user, It.IsIn(pws), true, false))
                .ReturnsAsync(SignInResult.Success);

            var controller = new AuthApiController(
                userManager.Object, signinManager.Object);

            // Act
            var result = await controller.Login(models[0]);
            await controller.Login(models[1]);

            // Assert
            Assert.IsType<OkResult>(result);

            signinManager.Verify(s => s.SignOutAsync(), Times.Exactly(2));
            signinManager.Verify
                (s => s.PasswordSignInAsync(user, It.IsIn(pws), true, false)
                , Times.Exactly(2));
            userManager.Verify(u => u.FindByEmailAsync(models[1].Email), Times.Once);
            userManager.Verify(u => u.FindByNameAsync(models[0].Username), Times.Once);
            
            VerifyLoggers(signinManager, userManager);
            userManager.VerifyNoOtherCalls();
            signinManager.VerifyNoOtherCalls();
        }

        // Test logging in without name, without password, test logging in
        // with name not found, test with password not found
        [Fact]
        public async Task TestLoginError() {
            // Arrange
            var userManager = new Mock<StubUserManager>();
            var signInManager = new Mock<StubSignInManager>();

            var user = new IdentityUser();
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
                (user, model2.Password, true, false))
                .ReturnsAsync(SignInResult.Failed);

            var controller = new AuthApiController(userManager.Object
                , signInManager.Object);

            // Act
            var unAuth = new [] { await controller.Login(model)
                , await controller.Login(model2)
            };
            var badReq = new [] {await controller.Login(invalidModels[0])
                , await controller.Login(invalidModels[1])
            };

            // Assert
            Assert.All(unAuth, r => Assert.IsType<UnauthorizedResult>(r));
            Assert.All(badReq, r => Assert.IsType<BadRequestResult>(r));

            userManager.Verify(u => u.FindByNameAsync(model.Username), Times.Once);
            userManager.Verify(u => u.FindByEmailAsync(model2.Email), Times.Once);
            signInManager.Verify(s => s.SignOutAsync(), Times.Exactly(4));
            signInManager.Verify(s => s.PasswordSignInAsync
                (user, model2.Password, true, false), Times.Once);

            VerifyLoggers(signInManager, userManager);
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
            VerifyLoggers(signInManager);
            signInManager.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task TestSignUp() {
            // Arrange
            var signInManager = new Mock<StubSignInManager>();
            var userManager = new Mock<StubUserManager>();
            var userModel = AuthData.LoginAllProps;

            userManager.Setup(u => u.CreateAsync
                (It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new AuthApiController(userManager.Object, signInManager.Object);

            // Act
            var result = await controller.SignUp(userModel);

            // Assert
            Assert.IsType<OkResult>(result);

            signInManager.Verify(s => s.SignOutAsync(), Times.Once);
            signInManager.Verify(s => s.PasswordSignInAsync
                (It.IsAny<IdentityUser>(), userModel.Password, true, false), Times.Once);
            userManager.Verify(u => u.CreateAsync
                (It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);

            VerifyLoggers(signInManager, userManager);
            signInManager.VerifyNoOtherCalls();
            userManager.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task TestSignUpError() {
            // Arrange
            var signInManager = new Mock<StubSignInManager>();
            var userManager = new Mock<StubUserManager>();
            var models = new[] {AuthData.LoginWithUname, AuthData.LoginWithEmail
                    , AuthData.BadLoginNoPw, AuthData.BadLoginOnlyPw };

            userManager.Setup(u => u.CreateAsync
                (It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var controller 
                = new AuthApiController(userManager.Object, signInManager.Object);

            // Act
            var unauthorized = await controller.SignUp(AuthData.LoginAllProps);
            var badResults = Task
                .WhenAll(from m in models select controller.SignUp(m)).Result;

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(unauthorized);
            Assert.All(badResults, a => Assert.IsType<BadRequestResult>(a));
            
            signInManager.Verify(s => s.SignOutAsync()
                , Times.Exactly(models.Length + 1));
            userManager.Verify(u => u.CreateAsync
                    (It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);

            VerifyLoggers(signInManager, userManager);
            signInManager.VerifyNoOtherCalls();
            userManager.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task TestSignUpAnonymous() {
            // Arrange
            var userManager = new Mock<StubUserManager>();
            var signInManager = new Mock<StubSignInManager>();

            var controller 
                = new AuthApiController(userManager.Object, signInManager.Object);

            // Act
            var result = await controller.SignUpAnonymous();

            // Assert
            Assert.IsType<OkResult>(result);

            signInManager.Verify(s => s.SignOutAsync(), Times.Once());
            signInManager.Verify(s => s.PasswordSignInAsync
                (It.IsAny<IdentityUser>(), It.IsAny<string>(), false, false)
                , Times.Once);
            userManager.Verify(u => u.CreateAsync
                (It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);

            VerifyLoggers(signInManager, userManager);
            signInManager.VerifyNoOtherCalls();
            userManager.VerifyNoOtherCalls();
        }

        // SignInManager and UserManager use set Logger internally 
        private void VerifyLoggers(Mock<StubSignInManager> sm = null
                , Mock<StubUserManager> um = null) {

            sm?.VerifySet(s => s.Logger = It.IsAny<ILogger<SignInManager<IdentityUser>>>());
            um?.VerifySet(u => u.Logger = It.IsAny<ILogger<UserManager<IdentityUser>>>());
        }

        [Fact]
        public void HttpVerbTests() {
            var t = typeof(AuthApiController);
            var post = typeof(HttpPostAttribute);
            var names = new [] {
                nameof(AuthApiController.Login)
                , nameof(AuthApiController.Logout)
                , nameof(AuthApiController.SignUp)
                , nameof(AuthApiController.SignUpAnonymous)
            };

            Assert.All(names, method => Assert.True(HasAttribute(method, post, t)));
        }
    }
}
