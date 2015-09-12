using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Isop.Server.Owin.Models
{
    public class MissingArgument
    {
        public string ErrorType { get { return "MissingArgument"; } }
        public string Message;
        public string Argument;
    }
}
