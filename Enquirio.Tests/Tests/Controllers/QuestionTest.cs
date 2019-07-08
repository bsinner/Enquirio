using System.Threading.Tasks;
using Enquirio.Controllers;
using Enquirio.Data;
using Enquirio.Models;
using Enquirio.Tests.TestData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {
    public class QuestionTest {

        [Fact]
        public async void ViewQuestionTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();

            mockRepo.Setup(repo => repo.GetByIdAsync<Question>("1", new [] {"Answers"}, null))
                    .ReturnsAsync(QuestionData.TestQuestion);

            QuestionController controller = new QuestionController(mockRepo.Object);

            // Act
            ViewResult result = await controller.ViewQuestion("1");

            // Assert
            Assert.Null(result.ViewName);
            Assert.IsType<QuestionViewModel>(result.ViewData.Model);
            mockRepo.Verify(repo => repo.GetByIdAsync<Question>("1", new[] {"Answers"}, null), Times.Once);
        }

        [Fact]
        public async void ViewQuestionNotFoundTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();

            mockRepo.Setup(repo => repo.GetByIdAsync<Question>("foo", new[] {"Answers"}, null))
                .Returns(Task.FromResult<Question>(null));

            var controller = new QuestionController(mockRepo.Object);

            // Act
            ViewResult result = await controller.ViewQuestion("foo");

            // Assert
            Assert.Equal("NotFound", result.ViewName);
            Assert.Null(result.ViewData.Model);
            mockRepo.Verify(repo => repo.GetByIdAsync<Question>("foo", new [] { "Answers" }, null), Times.Once);
        }

        [Fact]
        public async void CreateNewQuestionTest() {
            // Arrange
            var question = QuestionData.TestQuestion;
            var mockRepo = new Mock<IRepositoryEnq>();

            mockRepo.Setup(repo => repo.SaveAsync()).Callback(() => question.Id = 99);

            var controller = new QuestionController(mockRepo.Object);

            // Act
            RedirectToRouteResult result = await controller.Create(question);

            // Assert
            Assert.Equal(99, question.Id);
            Assert.Equal(question.Id, result.RouteValues["id"]);
            Assert.Equal("question", result.RouteName);

            mockRepo.Verify(repo => repo.Create(question), Times.Once);
            mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async void DeleteQuestionTest() {
            // Arrange
            var question = QuestionData.TestQuestion;
            var mockRepo = new Mock<IRepositoryEnq>();
            var controller = new QuestionController(mockRepo.Object);

            // Act
            RedirectToActionResult result = await controller.Delete(question.Id);

            // Assert
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
            mockRepo.Verify(repo => repo.DeleteById<Question>(question.Id), Times.Once);
            mockRepo.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public void CreateViewTest() {
            // Arrange
            var mockRepo = new Mock<IRepositoryEnq>();
            var controller = new QuestionController(mockRepo.Object);

            // Act
            ViewResult result = controller.Create();

            // Assert
            Assert.Null(result.ViewName);
            Assert.Null(result.ViewData.Model);
        }
    }
}
