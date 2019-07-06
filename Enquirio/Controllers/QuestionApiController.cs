﻿using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enquirio.Controllers {

    [Route("/api/")]
    public class QuestionApiController : Controller {

        private readonly IRepositoryEnq _repo;

        public QuestionApiController(IRepositoryEnq repo) => _repo = repo;

        [HttpPost("createAnswer/{questionId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateAnswer([FromBody] Answer answer, int questionId) {
            answer.QuestionId = questionId;

            _repo.Create(answer);

            await _repo.SaveAsync();

            return Ok(answer.Id.ToString());
        }

    }
}
