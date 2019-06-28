using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;

namespace Enquirio.Views.Shared.TagHelpers {

    [HtmlTargetElement("duration", Attributes = "date")]
    public class DurationTagHelper : TagHelper {

        public DateTime Date { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.Content.SetContent(GetString());
        }

        private string GetString() {
            var times = new SortedDictionary<int, string>() {
                { 3600, "minute"}
                , { 216000, "hour"}
                , { 5184000, "day"}
                , { 20736000, "week"}
                , { 82944000, "month"}
            };

            var duration = (long)(DateTime.Now - Date).TotalSeconds;
            var previous = 60;

            if (duration < 60) return "seconds ago";
            
            foreach (var time in times) {
                if (duration < time.Key) {
                    var result = duration / previous;

                    return $"{result} {time.Value + (result == 1 ? "" : "s")} ago";
                }

                previous = time.Key;
            }

            return $"on {Date.ToShortDateString()}";
        }

        private string FindMessage(SortedDictionary<int, string> times, long duration) {
//            if (duration < 60) return "seconds ago";
//
//            using (var enumerator = times.GetEnumerator()) {
//
//                while (enumerator.MoveNext()) {
//                    if (duration < enumerator.Current.Key) {
//                        enumerator.MoveNext();
//                        var result = duration / enumerator.Current.Key;
//                        return $"{result} {enumerator.Current.Value + (result == 1 ? "" : "s")} ago";
//                    }
//                }
//            }
//
            return null;
        }
    }
}
