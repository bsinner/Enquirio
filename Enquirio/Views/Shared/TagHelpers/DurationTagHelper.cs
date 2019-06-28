using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;

// Get UI friendly date string

namespace Enquirio.Views.Shared.TagHelpers {

    [HtmlTargetElement("duration", Attributes = "date", TagStructure = TagStructure.WithoutEndTag)]
    public class DurationTagHelper : TagHelper {

        public DateTime Date { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.Content.SetContent(GetString());
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
        }

        private string GetString() {
            var times = new SortedDictionary<int, string>() {
                { 60, "minute"}
                , { 3600, "hour"}
                , { 86400, "day"}
                , { 604800, "week"}
                , { 2419200, "month"}
            };

            var duration = (long)(DateTime.Now - Date).TotalSeconds;

            if (duration < 60) return "seconds ago";

            return LoopTimes(times, duration) ?? $"on {Date.ToShortDateString()}";
        }

        private string LoopTimes(SortedDictionary<int, string> times, long duration) {
            var previous = new KeyValuePair<int, string>(60, "seconds");

            foreach (var time in times) {
                if (duration < time.Key) {
                    var result = duration / previous.Key;

                    return $"{result} {previous.Value + (result == 1 ? "" : "s")} ago";
                }

                previous = time;
            }

            return null;
        }
    }
}
