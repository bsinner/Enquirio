using System;
using System.Collections.Generic;
using System.Text;
using Enquirio.Models;

namespace Enquirio.Tests.TestData {
    class QuestionData {
        public static Question TestQuestion
            = new Question { Id = 99, Title = "Q2", Contents = "...", Created = DateTime.Now
                , Answers = new List<Answer> {
                    new Answer { Id = 1, Title = "A1", Contents = "...", Created = DateTime.Now }
                    , new Answer { Id = 2, Title = "A2", Contents = "...", Created = DateTime.Now }}
            };
    }
}
