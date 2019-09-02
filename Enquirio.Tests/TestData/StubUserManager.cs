using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using User = Microsoft.AspNetCore.Identity.IdentityUser;

namespace Enquirio.Tests.TestData {

    public class StubUserManager : UserManager<User> {

        public StubUserManager() : base (
            new Mock<IUserStore<User>>().Object
            , new Mock<IOptions<IdentityOptions>>().Object
            , new Mock<IPasswordHasher<User>>().Object
            , new IUserValidator<User>[0]
            , new IPasswordValidator<User>[0]
            , new Mock<ILookupNormalizer>().Object
            , new Mock<IdentityErrorDescriber>().Object
            , new Mock<IServiceProvider>().Object
            , new Mock<ILogger<UserManager<User>>>().Object
        ) {}
    }
}
