using System;
using System.Collections.Generic;
using System.Linq;
using Isop.Infrastructure;
using System.Reflection;

namespace Isop.Server.Owin
{
    using Models;
    public interface IIsopServer
    {
        Models.MethodTreeModel GetModel();
        IEnumerable<Models.Controller> GetControllers ();
        Models.Controller GetController(string controller);
        Models.Method GetControllerMethod(string controller, string method);
        IEnumerable<string> InvokeMethod(Models.Method method, IDictionary<string,object> form);
    }
    
}
