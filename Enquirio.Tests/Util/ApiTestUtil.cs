using System;
using System.Diagnostics;
using System.Reflection;
using Castle.Core.Internal;
using Enquirio.Controllers;

namespace Enquirio.Tests.Tests.Controllers {
    public class ApiTestUtil {
        protected bool HasAttribute(string method
                , Type attribute, Type clazz) {

            return clazz.GetMethod(method)
                       ?.GetCustomAttribute(attribute) != null;
        }
    }
}
