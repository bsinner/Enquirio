using System;
using System.Reflection;
using Castle.Core.Internal;
using Enquirio.Controllers;

namespace Enquirio.Tests.Tests.Controllers {
    public class ApiTestUtil {
        protected bool HasAttribute(String method, Type attribute) {
            return !typeof(QuestionApiController)
                .GetMethod(method)
                .GetCustomAttributes(attribute)
                .IsNullOrEmpty();
        }
    }
}
