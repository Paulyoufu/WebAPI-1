using Microsoft.Owin.Hosting;
using System;

namespace WebAPI.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("WebAPI SelfHost started at " + baseAddress);
                Console.WriteLine("Press enter to finish");
                Console.ReadLine();
            }

            
        }
    }
}
