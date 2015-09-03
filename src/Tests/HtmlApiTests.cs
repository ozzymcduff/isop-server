﻿using Nancy;
using Nancy.Testing;
using NUnit.Framework;
using System.Linq;
using With.Rubyfy;
using With;
using Nancy.Helpers;
using Isop.Server;
namespace Isop.Tests.Server
{
    [TestFixture]
    public class HtmlApiTests: BaseFixture
    {
        private Browser browser;
        [TestFixtureSetUp]
        public void BeforeEachTest()
        {
            browser = GetBrowser();
        }

        [Test]
        public void Should_return_global_parameters()
        {
            // When
            var result = browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(new[] { "Global" }, result.Body[".global_parameters input"]
                .Map(i => i.Attributes["name"]).ToA());
        }

        [Test]
        public void Should_return_available_controllers()
        {
            // When
            var result = browser.Get("/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(new[] { "My" }, result.Body[".controllers a"]
                .Map(i => i.InnerText).ToA());
        }

        [Test]
        public void When_get_controller_url_Should_return_header_and_available_actions()
        {
            // When
            var result = browser.Get("/My/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("My", result.Body["h1"].Single().InnerText.Trim('\n', '\r', ' '));
            Assert.AreEqual(new[] { "Action", "Fail", "ActionWithGlobalParameter", "ActionWithObjectArgument" },
                result.Body["a"].Map(i => i.InnerText.Trim('\n', '\r', ' ')).ToA());
        }

        [Test]
        public void Form_for_action_Should_contain_parameters()
        {
            // When
            var result = browser.Get("/My/Action/", with =>
            {
                with.HttpRequest();
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Action", result.Body["h1"].Single().InnerText.Trim('\n', '\r', ' '));
            var names = result.Body["form input"].Map(i => i.Attributes["name"]).ToA();
            Assert.AreEqual(new[] { "value" }, names);
        }


        [Test]
        public void Post_form_action()
        {
            var value = "value ' 3 ' \"_12 \"sdf";

            // When
            var result = browser.Post("/My/Action/", with =>
            {
                with.HttpRequest();
                with.FormValue("value", value);
            });

            // Then
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.That(result.Body["p"].Map(p => p.InnerText).Join("\n"), Is.StringContaining(HttpUtility.HtmlEncode("value=" + value)));
        }
    }
}
