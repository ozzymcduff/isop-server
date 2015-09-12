using System;
using Microsoft.Owin.Hosting;

namespace Isop.Server.Owin
{
    public class ConsoleMain
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:8080/";

            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Started");
                Console.ReadKey();
                Console.WriteLine("Stopping");
            }
        }
    }
}

