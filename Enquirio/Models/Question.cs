using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Enquirio.Data;

namespace Enquirio.Models {
    public class Question : IEntity, IPost {

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Contents { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Edited { get; set; }

        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
