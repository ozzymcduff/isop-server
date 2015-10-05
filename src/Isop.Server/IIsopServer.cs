using System.Collections.Generic;

namespace Isop.Server
{
    public interface IIsopServer
    {
        Models.MethodTreeModel GetModel();
        Models.Controller GetController(string controller);
        Models.Method GetControllerMethod(string controller, string method);
        IEnumerable<string> InvokeMethod(Models.Method method, IDictionary<string,object> form);
    }
}
