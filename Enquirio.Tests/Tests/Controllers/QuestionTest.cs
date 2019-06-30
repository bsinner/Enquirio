using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Enquirio.Controllers;
using Enquirio.Data;
using Enquirio.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {
    public class QuestionTest {

        private readonly Question _testQuestion 
            = new Question { Id = 2, Title = "Q2", Contents = "...", Created = DateTime.Now
                , Answers = new List<Answer> {
                    new Answer { Id = 1, Title = "A1", Contents = "...", Created = DateTime.Now }
                    , new Answer { Id = 2, Title = "A2", Contents = "...", Created = DateTime.Now }}
                };

       
        [Fact]
        public void ViewQuestionTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();

            mockRepo.Setup(repo => repo.GetByIdAsync<Question>("1", new [] {"Answers"}, null))
                    .Returns(Task.FromResult(_testQuestion));

            QuestionController controller = new QuestionController(mockRepo.Object);

            // Act
            Task<ViewResult> result = controller.ViewQuestion("1");

            // Assert
            Assert.Null(result.Result.ViewName);
            Assert.IsType<Question>(result.Result.ViewData.Model);
            mockRepo.Verify(repo => repo.GetByIdAsync<Question>("1", new[] {"Answers"}, null), Times.Once);
        }

        [Fact]
        public void ViewQuestionNotFoundTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();

            mockRepo.Setup(repo => repo.GetByIdAsync<Question>("foo", new[] {"Answers"}, null))
                .Returns(Task.FromResult<Question>(null));

            var controller = new QuestionController(mockRepo.Object);

            // Act
            Task<ViewResult> result = controller.ViewQuestion("foo");

            // Assert
            Assert.Equal("NotFound", result.Result.ViewName);
            Assert.Null(result.Result.ViewData.Model);
            mockRepo.Verify(repo => repo.GetByIdAsync<Question>("foo", new [] { "Answers" }, null), Times.Once);
        }
    }
}
