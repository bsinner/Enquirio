
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enquirio.Controllers;
using Enquirio.Data;
using Enquirio.Models;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {
    public class QuestionApiReadTests : ApiTestUtil {

        private const int PageLength = QuestionApiController.PageLength;

        [Fact]
        public async Task TestGetQuestionsPageOne() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();
            mockRepo.Setup(repo => repo.GetAllAsync<Question>
                (null, null, true, PageLength, null, null))
                .ReturnsAsync
                (QuestionData.TestQuestions().ToList().GetRange(0, 10));

            var controller = new QuestionApiController(mockRepo.Object);

            // Act 
            var result = await controller.GetQuestions();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Question>>(result.Value);
            Assert.Equal(PageLength
                , (result.Value as List<Question>).Count);

            mockRepo.Verify(repo => repo.GetAllAsync<Question>
                (null, null, true, PageLength, null, null), Times.Once);
            mockRepo.VerifyNoOtherCalls();
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
