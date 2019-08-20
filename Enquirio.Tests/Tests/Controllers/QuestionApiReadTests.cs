
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            mockRepo.Setup(repo => repo.GetAllAsync<Question>(
                    null, It.IsAny<Expression<Func<Question, IComparable>>>()
                    , It.IsAny<bool>(), PageLength, null, null)
                ).ReturnsAsync(
                    QuestionData.TestQuestions().ToList().GetRange(0, PageLength)
                );

            var controller = new QuestionApiController(mockRepo.Object);

            // Act 
            var result = await controller.GetQuestions(1);
            await controller.GetQuestions(0);
            await controller.GetQuestions(-1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<List<Question>>(result.Value);
            Assert.Equal(PageLength
                , (result.Value as List<Question>).Count);

            mockRepo.Verify(repo => repo.GetAllAsync<Question>(
                    null, It.IsAny<Expression<Func<Question, IComparable>>>()
                    , It.IsAny<bool>(), PageLength, null, null), Times.Exactly(3)
                );
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
