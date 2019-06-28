using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enquirio.Views.Shared.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace Enquirio.Tests {
    public class DurationTagHelperTest {

        [Theory]
        [InlineData(-.01, "seconds ago")]
        [InlineData(-.5, "30 minutes ago")]
        [InlineData(-1, "1 hour ago")]
        [InlineData(-25, "2 days ago")]
        [InlineData(-(24 * 7 * 3), "3 weeks ago")]
        public void TestDuration(double hours, string expected) {

            // Arrange
            var context = new TagHelperContext(
                new TagHelperAttributeList()
                , new Dictionary<object, object>()
                ,  "unique-id"
            );

            var output = new TagHelperOutput("duration"
                , new TagHelperAttributeList()
                , (cache, encoder) => 
                    Task.FromResult<TagHelperContent>(new DefaultTagHelperContent())
            );

            // Act
            var helper = new DurationTagHelper { Date = DateTime.Now.AddHours(hours) };
            helper.Process(context, output);

            // Assert
            Assert.Equal(expected, output.Content.GetContent());
        }
//
//        public void TestDurationUpperBound() {
//
//        }
    }
}
