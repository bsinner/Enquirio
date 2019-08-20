
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
                    null, desc(), true, PageLength, null, null)
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
            Assert.Equal(PageLength, (result.Value as List<Question>).Count);

            mockRepo.Verify(repo => repo.GetAllAsync<Question>(
                    null, desc()
                    , true, PageLength, null, null), Times.Exactly(3)
            );
            mockRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task TestGetQuestionsPastPageOne() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();
            var questions = QuestionData.TestQuestions().ToList();
            var maxPage = (int)Math.Floor(questions.Count / (double)PageLength);
            var midPage = 2;
            var midSkip = midPage * PageLength;
            var maxSkip = maxPage * PageLength;

            mockRepo.Setup(repo => repo.GetAllAsync(
                    null, desc(), true, PageLength, midSkip, null
            )).ReturnsAsync(questions.GetRange(midSkip, PageLength));

            mockRepo.Setup(repo => repo.GetAllAsync(
                    null, desc(), true, PageLength, maxSkip, null
            )).ReturnsAsync(new List<Question>());

            mockRepo.Setup(repo => repo.GetCountAsync<Question>(null))
                    .ReturnsAsync(questions.Count);

            var controller = new QuestionApiController(mockRepo.Object);

            // Act 
            var result = await controller.GetQuestions(midPage);
            await controller.GetQuestions(maxPage + 100);

            // Assert
            Assert.Equal(PageLength, (result.Value as List<Question>).Count);

            mockRepo.Verify(repo => repo.GetAllAsync(
                    null, desc(), true, PageLength
                    , It.IsIn(new [] { midSkip, maxSkip }), null
            ), Times.Exactly(2));
            mockRepo.Verify(repo => repo.GetCountAsync<Question>(null)
                    , Times.Exactly(2));
            mockRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public void HttpVerbTests() {
            Assert.True(HasAttribute(nameof(QuestionApiController.GetQuestions)
                    , typeof(HttpGetAttribute)));
        }

        private Expression<Func<Question, IComparable>> desc() {
            return It.IsAny<Expression<Func<Question, IComparable>>>();
        }
    }
}
