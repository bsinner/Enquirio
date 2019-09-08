using Enquirio.Controllers;
using Enquirio.Data;
using Microsoft.AspNetCore.Http;

namespace Enquirio.Tests.TestData {
    class QApiControllerWithContext : QuestionApiController {

        public QApiControllerWithContext(IRepositoryEnq repo) : base (
            repo, new HttpContextAccessor()
        ) { }
    }
}
