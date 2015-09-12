using System;
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
    using Infrastructure;

    public class IsopServerFromAssemblyLocation : IsopServerFromBuild
    {
        private static IEnumerable<Assembly> Assemblies;
        public IsopServerFromAssemblyLocation()
            : base(()=>GetBuild(Assemblies))
        {
            if (Assemblies == null)
            {
                Assemblies = new LoadAssemblies().LoadFrom(ExecutionAssembly.Path()).ToArray();
            }
        }
        private static Build GetBuild(IEnumerable<Assembly> assemblies){
            var build = new Build();
            foreach (var assembly in assemblies)
            {
                build.ConfigurationFrom(assembly);
            }
            return build;
        }
    }
    
}
