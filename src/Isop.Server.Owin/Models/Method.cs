﻿using System.Collections.Generic;

namespace Isop.Server.Owin.Models
{
    public class Method
    {
        public Method()
        {
            Parameters = new Param[0];
        }

        public Method(string name, string className, string help)
            :this()
        {
            Name = name;
            ClassName = className;
            Help = help;
            Url = "/" + className + "/" + name;
        }

        public string Name { get; set; }
        public string ClassName { get; set; }

        public IEnumerable<Param> Parameters { get; set; }
        public string Help { get; private set; }
        public string Url { get; set; }
    }
}