using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;

namespace Enquirio.Views.Shared.TagHelpers {

    [HtmlTargetElement("duration", Attributes = "date")]
    public class DurationTagHelper : TagHelper {

        public DateTime Date { get; set; }

        private readonly SortedDictionary<int, string> _timeSpans = new SortedDictionary<int, string>() {
            { 3600, "minute"}
            , { 216000, "hour"}
            , { 5184000, "day"}
            , { 20736000, "week"}
            , { 82944000, "month"}
        };

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.Content.SetContent(GetString());
        }

        private string GetString() {
            var duration = (long)(DateTime.Now - Date).TotalSeconds;

            if (duration < 60) return "seconds ago";
            
            foreach (var time in _timeSpans) {
                if (duration < time.Key) {
                    var result = duration / time.Key;

                    return $"{result} {time.Key + (result == 1 ? "" : "s")} ago";
                }
            }

            return $"on {Date.ToShortDateString()}";
        }
    }
}
