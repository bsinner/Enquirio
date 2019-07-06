﻿using System;

// Interface to represent a text post, such as a Question or Answer
namespace Enquirio.Data {
    public interface IPost : IEntity {
        DateTime Created { get; set; }
        DateTime? Edited { get; set; }

        String Title { get; set; }
        String Contents { get; set; }
    }
}