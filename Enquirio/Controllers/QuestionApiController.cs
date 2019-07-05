using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    [Route("/api/")]
    public class QuestionApiController : Controller {

        private readonly IRepositoryEnq _repo;

        public QuestionApiController(IRepositoryEnq repo) => _repo = repo;

        [HttpPost("createAnswer/{id}")]
        [Consumes("application/json")]
        public async Task<string> CreateAnswer([FromBody] Answer answer, int id) {
            answer.QuestionId = id;

            _repo.Create(answer);

            await _repo.SaveAsync();

            return answer.Id.ToString();
        }

    }
}
