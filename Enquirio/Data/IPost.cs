using System;

// Interface to represent a text post, such as a Question or Answer
namespace Enquirio.Data {
    public interface IPost {
        DateTime Created { get; set; }
        DateTime? Edited { get; set; }
    }
}