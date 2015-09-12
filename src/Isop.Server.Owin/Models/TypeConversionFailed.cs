﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Isop.Server.Owin.Models
{
    public class TypeConversionFailed
    {
        public string ErrorType { get { return "TypeConversionFailed"; } }
        public string Message;
        public string Argument;
        public string Value;
        public string TargetType;
    }
}
