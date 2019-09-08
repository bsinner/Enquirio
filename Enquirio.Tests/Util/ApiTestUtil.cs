using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Enquirio.Tests.Tests.Controllers {
    public class ApiTestUtil {
        protected bool HasAttribute(string method
                , Type attribute, Type clazz) {

            return clazz.GetMethod(method)
                       ?.GetCustomAttribute(attribute) != null;
        }
    }
}
