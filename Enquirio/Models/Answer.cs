using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Enquirio.Data;

namespace Enquirio.Models {
    public class Answer : IEntity, IPost {

        public int Id { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Contents { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Edited { get; set; }

        public virtual Question Question { get; set; }
    }
}
