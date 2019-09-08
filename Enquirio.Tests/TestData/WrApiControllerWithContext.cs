using Enquirio.Controllers;
using Enquirio.Data;
using Microsoft.AspNetCore.Http;

namespace Enquirio.Tests.TestData {
    class WrApiControllerWithContext : WriteApiController {

        public WrApiControllerWithContext(IRepositoryEnq repo) : base (
            repo, new HttpContextAccessor()
        ) { }
    }
}
