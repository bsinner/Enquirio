using Enquirio.Models;

namespace Enquirio.Tests.TestData {
    class AuthData {

        public static LoginViewModel LoginNoEmail = new LoginViewModel {
            Username = "foo", Password = "tT%5"
        };
        public static LoginViewModel LoginNoUname = new LoginViewModel {
            Email = "foo@example.net", Password = "tT%5"
        };
        
        public static LoginViewModel BadLoginNoPW = new LoginViewModel {
            Password = "a"
        };
        public static LoginViewModel BadLoginOnlyPW = new LoginViewModel {
            Email = "a"
        };
    }
}
