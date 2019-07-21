using Isop.Server;
using NUnit.Framework;
using System.Collections.Generic;
using With;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Isop.Tests.Server
{
    [TestFixture]
    public class IsopServerFromBuildTests
    {
        private IsopServerFromBuild _data;
        private MyController _controller;
        public class MyController
        {
			public string ActionValue;
            public string Action(string value)
            {
				ActionValue = value;
                return value;
            }
            public object Value;
            public object ReturnObject()
            {
                return Value;
            }
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new MyController();
            _data = new IsopServerFromBuild( ()=> Builder.Create(new ServiceCollection()
                .Tap(sc=>sc.AddSingleton(_controller))).Recognize(_controller.GetType()).BuildAppHost());
        }

        [Test]
        public void PassingValueShouldNotDistortParameter()
        {
            var method = _data.GetControllerMethod("My", "Action");
            var value = "value ' 3 ' \"_12 \"sdf";
            var result = String.Join("\n", _data.InvokeMethod(method, new Dictionary<string, object> { { "value", value } }));
            Assert.That(_controller.ActionValue, Is.EqualTo(value));
        }

        [Test]
        public void FormatObjectAsJson()
        {
            var method = _data.GetControllerMethod("My", "ReturnObject");
            var value = "{\"v\":1}";
            _controller.Value = new {v=1};
            var result = String.Join("\n",_data.InvokeMethod(method, Empty()));
            Assert.That(result, Is.EqualTo(value));
        }

        private Dictionary<string, object> Empty(){
            return new Dictionary<string, object>();
        }
    }
}
