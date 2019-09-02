using Enquirio.Models;

namespace Enquirio.Tests.TestData {
    class AuthData {

        public static LoginViewModel LoginWithUname = new LoginViewModel {
            Username = "foo", Password = "tT%5"
        };
        public static LoginViewModel LoginWithEmail = new LoginViewModel {
            Email = "foo@example.net", Password = "tT%5"
        };
        
        public static LoginViewModel BadLoginNoPw = new LoginViewModel {
            Password = "a"
        };
        public static LoginViewModel BadLoginOnlyPw = new LoginViewModel {
            Email = "a"
        };
    }
}
