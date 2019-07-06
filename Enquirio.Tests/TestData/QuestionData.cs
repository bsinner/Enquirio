using System;
using System.Collections.Generic;
using System.Text;
using Enquirio.Models;

namespace Enquirio.Tests.TestData {
    class QuestionData {
        public static Question TestQuestion
            = new Question { Id = 0, Title = "Q1", Contents = "...", Created = DateTime.Now
                , Answers = new List<Answer> {
                    new Answer { Id = 1, Title = "A1", Contents = "...", Created = DateTime.Now }
                    , new Answer { Id = 2, Title = "A2", Contents = "...", Created = DateTime.Now }}
            };

        public static Question InvalidTestQuestion
            = new Question { Id = 0, Title = null, Contents = "", Created = DateTime.Now, Answers = new List<Answer>() };

        public static Answer TestAnswer
            = new Answer { Id = 0, Title = "A1", Contents = "...", Created = DateTime.Now, QuestionId = 1 };

        public static Answer InvalidTestAnswer
            = new Answer { Id = 10, Title = "", Contents = null, Created = DateTime.Now, QuestionId = 1 };
    }
}
