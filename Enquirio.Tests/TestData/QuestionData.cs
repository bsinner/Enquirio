﻿using System;
using System.Collections.Generic;
using System.Text;
using Enquirio.Models;

namespace Enquirio.Tests.TestData {
    class QuestionData {

        public static Question TestQuestionMinProps = new Question { Title = "Q1", Contents = "..." };
        public static Answer TestAnswerMinProps = new Answer { Title = "A1", Contents = "...", QuestionId = 1 };

        public static Answer TestAnswer
            = new Answer { Id = 1, Title = "A1", Contents = "...", Created = DateTime.Now, QuestionId = 1 };

        public static Question TestQuestion
            = new Question { Id = 1, Title = "Q1", Contents = "...", Created = DateTime.Now
                , Answers = new List<Answer> {
                    new Answer { Id = 1, Title = "A1", Contents = "...", Created = DateTime.Now }
                    , new Answer { Id = 2, Title = "A2", Contents = "...", Created = DateTime.Now }}
            };

        public static IEnumerable<Question> TestQuestions() {
            for (int i = 0; i <= 100; i++) {
                yield return new Question {
                    Id = i,
                    Title = $"Q{i}",
                    Contents = "..."
                    ,
                    Created = DateTime.Now,
                    Answers = new List<Answer>()
                };
            }
        }

        public static Question CreateTestInvalidQuestion = new Question { Id = 999, Title = "", Contents = null };

        public static Question EditTestInvalidTestQuestion
            = new Question { Id = 0, Title = null, Contents = "", Created = DateTime.Now, Answers = new List<Answer>() };

        public static Answer InvalidTestAnswer
            = new Answer { Id = 10, Title = "", Contents = null, Created = DateTime.Now, QuestionId = 1 };
    }
}
