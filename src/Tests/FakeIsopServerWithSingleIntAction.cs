using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using System.Linq;
using With.Rubyfy;
using With;
using Nancy.Bootstrapper;
using Nancy.Helpers;
using Isop.Server;
using System.Collections.Generic;

namespace Isop.Tests.Server
{

    class FakeIsopServerWithSingleIntAction : IsopServerFromBuild
    {
        public FakeIsopServerWithSingleIntAction()
            : base( ()=> new Build { typeof(Isop.Tests.FakeControllers.SingleIntAction) })
        {
        }
    }
}
