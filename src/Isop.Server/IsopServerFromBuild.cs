using System;
using System.Collections.Generic;
using System.Linq;
using Isop.Infrastructure;
using Isop.Server.Models;
using System.Reflection;
using Isop.Abstractions;

namespace Isop.Server
{
    public class IsopServerFromBuild : IIsopServer
    {
        Func<IAppHost> Build;

        public IsopServerFromBuild(Func<IAppHost> build)
        {
            Build = build;
        }

        public Models.Controller GetController(string controller)
        {
            return GetModel().Controllers.Single(c => c.Name.Equals(controller, StringComparison.OrdinalIgnoreCase));
        }

        public Models.Method GetControllerMethod(string controller, string method)
        {
            return GetController(controller).Methods.Single(m => m.Name.Equals(method, StringComparison.OrdinalIgnoreCase));
        }

        public Models.MethodTreeModel GetModel()
        {
            var build = Build();
            return Map.GetMethodTreeModel(build);
        }

        public IEnumerable<string> InvokeMethod(Models.Method method, IDictionary<string, object> form)
        {
            var build = Build();
            {
                throw new NotImplementedException();
#if false
                return build
                    //.SetFormatter(new JsonFormatter().Format)
                    .Controller(method.ClassName)
                    .Action(method.Name)
                    .Parameters(form.ToDictionary(p => p.Key, p => p.Value != null ? p.Value.ToString() : (string)null))
                    .Invoke();
#endif
            }
        }

        private class Map
        {
            public static MethodTreeModel GetMethodTreeModel(IAppHost that)
            {
                return new MethodTreeModel(globalParameters: Parameters(that),
                                           controllers: Controllers(that));
            }

            private static Models.Controller[] Controllers(IAppHost that)
            {
#if false
                return that.Recognizes
                    .Where(cmr => !cmr.IsHelp())
                    .Select(cmr => Controller(that, cmr)).ToArray();
#endif
                throw new NotImplementedException();
            }

            private static Param[] Parameters(IAppHost that)
            {
                throw new NotImplementedException();
#if false
                return new List<Param>(that.GlobalParameters
                    .Select(p => new Param(typeof(string), p.Argument.ToString(), p.Required)))
                    .ToArray();
#endif
            }

            private static Controller Controller(IAppHost that, Domain.Controller type)
            {
                throw new NotImplementedException();
#if false
                return new Controller(type.Name, type.GetControllerActionMethods().Select(m => Method(that, type, m)).ToArray());
#endif
            }

            private static Method Method(IAppHost that, Domain.Controller type, Domain.Method m)
            {
                throw new NotImplementedException();
#if false

                var @params = m.GetArguments().Select(p => new Param(p.Type, p.Name, p.Required)).ToArray();

                var help = that.Controller(type.Name).Action(m.Name).Help();

                return new Method(m.Name, type.Name, help)
                {
                    Parameters = new List<Param>(@params.ToArray())
                };
#endif
            }

        }
    }
}
