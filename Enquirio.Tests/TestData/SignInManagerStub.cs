using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using User = Microsoft.AspNetCore.Identity.IdentityUser;

namespace Enquirio.Tests.TestData {

    public class SignInManagerStub : SignInManager<User> {

        public SignInManagerStub() : base (
            new Mock<UserManagerStub>().Object
            , new HttpContextAccessor()
            , new Mock<IUserClaimsPrincipalFactory<User>>().Object
            , new Mock<IOptions<IdentityOptions>>().Object
            , new Mock<ILogger<SignInManager<User>>>().Object
            , new Mock<IAuthenticationSchemeProvider>().Object
            , new Mock<IUserConfirmation<User>>().Object
        ) {}
    }
}
