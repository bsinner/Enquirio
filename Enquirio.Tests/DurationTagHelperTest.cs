using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Enquirio.Views.Shared.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace Enquirio.Tests {
    public class DurationTagHelperTest {

        private const string Tag = "span";

        [Theory]
        [InlineData(-.01, "seconds ago")]
        [InlineData(-.5, "30 minutes ago")]
        [InlineData(-1, "1 hour ago")]
        [InlineData(-25, "1 day ago")]
        [InlineData(-(24 * 7 * 3), "3 weeks ago")]
        [InlineData(-(24 * 7 * 4 + 1), "^on \\d{1,2}/\\d{1,2}/\\d{4}$", true)]
        public void TestDuration(double hours, string testString, bool regexTest = false) {

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
            string a = output.Content.GetContent();

            // Assert
            Assert.Equal(Tag, output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);

            if (!regexTest) {
                Assert.Equal(testString, output.Content.GetContent());
            } else {
                Assert.True(new Regex(testString)
                    .Match(output.Content.GetContent())
                    .Success
                );
            }
        }

    }
}
