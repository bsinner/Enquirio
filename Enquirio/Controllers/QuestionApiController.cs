using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    [Route("api/[controller]")]
    public class QuestionApiController : Controller {

        private readonly IRepositoryEnq _repo;

        public QuestionApiController(IRepositoryEnq repo) => _repo = repo;

        [HttpPost]
        public async Task<string> CreateAnswer([FromBody] Answer answer) {
            _repo.Create(answer);

            await _repo.SaveAsync();

            return answer.Id.ToString();
        }

    }
}
