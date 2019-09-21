using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Enquirio.Models {
    public class ApplicationUser : IdentityUser {
        [JsonIgnore]
        public virtual List<Answer> Answers { get; set; }
        [JsonIgnore]
        public virtual List<Question> Questions { get; set; }
    }
}
