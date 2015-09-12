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
    public class JsonFormatter
    {
        private readonly CustomJsonSerializer _serializer;
        public JsonFormatter()
        {
            _serializer = new CustomJsonSerializer();
            _serializer.Formatting = Formatting.None;
        }

        public IEnumerable<string> Format(object retval)
        {
            yield return _serializer.Serialize(retval);
        }
    }
    
}
