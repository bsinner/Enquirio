using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Enquirio.Data;

namespace Enquirio.Models {
    public class Answer : IPost {

        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Contents { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Edited { get; set; }

        [JsonIgnore]
        public virtual Question Question { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
    }
}