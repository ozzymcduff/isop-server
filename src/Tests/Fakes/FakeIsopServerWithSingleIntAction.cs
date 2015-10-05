using Isop.Server;

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
