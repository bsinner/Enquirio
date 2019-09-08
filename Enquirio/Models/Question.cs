using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Enquirio.Data;
using Microsoft.AspNetCore.Identity;

namespace Enquirio.Models {
    public class Question : IPost {

        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Contents { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Edited { get; set; }

        public List<Answer> Answers { get; set; } = new List<Answer>();
        [JsonIgnore]
        public virtual IdentityUser User { get; set; }
    }
}
