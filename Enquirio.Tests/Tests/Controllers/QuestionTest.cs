using System;
using System.Collections.Generic;
using System.Text;
using Enquirio.Controllers;
using Enquirio.Data;
using Moq;
using Xunit;

namespace Enquirio.Tests.Tests.Controllers {
    public class QuestionTest {


        [Fact]
        public void ViewResultTests() {
            var mock = new Mock<IRepositoryEnq>();
//            mock.SetupGet()
//            QuestionController controller = new QuestionController();

            Assert.Equal(1, 1);
        }

    }
}
