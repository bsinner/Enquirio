
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enquirio.Controllers;
using Enquirio.Data;
using Enquirio.Models;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {
    public class QuestionApiReadTests : ApiTestUtil {

        [Fact]
        public void TestGetQuestionsPageOne() {
            var mockRepo = new Mock<IRepositoryEnq>();
            mockRepo.Setup(repo => repo.GetAllAsync<Question>
                (null, null, true, 10, null, null))
                .ReturnsAsync(QuestionData.TestQuestions().ToList().GetRange(0, 10));

        }

        [Fact]
        public void TestGetQuestionsOverPageOne() {
            var mockRepo = new Mock<IRepositoryEnq>();
        }

        [Fact]
        public void HttpVerbTests() {
            Assert.True(HasAttribute(nameof(QuestionApiController.GetQuestions)
                , typeof(HttpGetAttribute)));
        }
    }
}
