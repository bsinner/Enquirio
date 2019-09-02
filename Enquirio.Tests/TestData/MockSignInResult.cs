using Microsoft.AspNetCore.Identity;

namespace Enquirio.Tests.TestData {

    public class MockSignInResult : SignInResult {
        public new readonly bool Succeeded;

        public MockSignInResult(bool succeeded) {
            Succeeded = succeeded;
        }
    }
}
