using Isop.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Isop.Tests.Server
{

    class FakeIsopServerWithSingleIntAction : IsopServerFromBuild
    {
        public FakeIsopServerWithSingleIntAction()
            : base( ()=> Builder.Create(new ServiceCollection()).Recognize(typeof(Isop.Tests.FakeControllers.SingleIntAction)).BuildAppHost())
        {
        }
    }
}
