using System.Collections.Generic;

namespace Isop.Client.Transfer
{
    public class Root
    {
        public IEnumerable<Param> GlobalParameters { get; set; }
        public IEnumerable<Controller> Controllers { get; set; }
    }
}