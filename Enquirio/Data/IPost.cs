using System;
using Enquirio.Models;

// Interface to represent a text post, such as a Question or Answer
namespace Enquirio.Data {
    public interface IPost : IEntity {
        DateTime Created { get; set; }
        DateTime? Edited { get; set; }

        string Title { get; set; }
        string Contents { get; set; }

        ApplicationUser User { get; set; }
        string UserId { get; set; }
    }
}