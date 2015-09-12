﻿using System;
using System.Collections.Generic;
using System.Linq;
using Isop.Infrastructure;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace Isop.Server.Owin
{
    using Models;

    public class IsopServerFromBuild : IIsopServer
    {
        Func<Build> Build;

        public IsopServerFromBuild(Func<Build> build)
        {
            Build = build;
        }

        public Models.Controller GetController(string controller)
        {
            return GetModel().Controllers.Single(c => c.Name.EqualsIC(controller));
        }

        public Models.Method GetControllerMethod(string controller, string method)
        {
            return GetController(controller).Methods.Single(m => m.Name.EqualsIC(method));
        }

        public Models.MethodTreeModel GetModel()
        {
            using (var build = Build())
            {
                return Map.GetMethodTreeModel(build);
            }
        }

        public IEnumerable<string> InvokeMethod(Models.Method method, IDictionary<string, object> form)
        {
            using (var build = Build())
            {
                return build
                    .SetFormatter(new JsonFormatter().Format)
                    .Controller(method.ClassName)
                    .Action(method.Name)
                    .Parameters(form.ToDictionary(p => p.Key, p => p.Value != null ? p.Value.ToString() : (string)null))
                    .Invoke();
            }
        }

        public IEnumerable<Controller> GetControllers ()
        {
            throw new NotImplementedException ();
        }

        private class Map
        {
            public static MethodTreeModel GetMethodTreeModel(Build that)
            {
                return new MethodTreeModel(globalParameters: Parameters(that),
                                           controllers: Controllers(that));
            }

            private static Models.Controller[] Controllers(Build that)
            {
                return that.Recognizes
                    .Where(cmr => !cmr.IsHelp())
                    .Select(cmr => Controller(that, cmr)).ToArray();
            }

            private static Param[] Parameters(Build that)
            {
                return new List<Param>(that.GlobalParameters
                    .Select(p => new Param(typeof(string), p.Argument.ToString(), p.Required)))
                    .ToArray();
            }

            private static Controller Controller(Build that, Domain.Controller type)
            {
                return new Controller(type.Name, type.GetControllerActionMethods().Select(m => Method(that, type, m)).ToArray());
            }

            private static Method Method(Build that, Domain.Controller type, Domain.Method m)
            {
                var @params = m.GetArguments().Select(p => new Param(p.Type, p.Name, p.Required)).ToArray();

                var help = that.Controller(type.Name).Action(m.Name).Help();

                return new Method(m.Name, type.Name, help)
                {
                    Parameters = new List<Param>(@params.ToArray())
                };
            }

        }
    }
}
