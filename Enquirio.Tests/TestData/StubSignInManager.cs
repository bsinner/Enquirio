using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using User = Enquirio.Models.ApplicationUser;

namespace Enquirio.Tests.TestData {

    public class StubSignInManager : SignInManager<User> {

        public StubSignInManager() : base (
            new Mock<StubUserManager>().Object
            , new HttpContextAccessor()
            , new Mock<IUserClaimsPrincipalFactory<User>>().Object
            , new Mock<IOptions<IdentityOptions>>().Object
            , new Mock<ILogger<SignInManager<User>>>().Object
            , new Mock<IAuthenticationSchemeProvider>().Object
            , new Mock<IUserConfirmation<User>>().Object
        ) { }
    }
}
