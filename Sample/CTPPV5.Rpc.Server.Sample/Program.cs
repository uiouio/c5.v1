using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.CommandServer;
using CTPPV5.Models;
using CTPPV5.Repository;
using CTPPV5.Rpc.Net.Server;

namespace CTPPV5.Rpc.Server.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);
            var bootstrap = new ServerBootstrap(endpoint);
            if (args.Length == 0 || args[0] == "sample")
                bootstrap.StartUp(null, new CommandServerModule(), new RepositoryModule(), new SampleModule());
            else
            {
                bootstrap.AddFilterTypes(typeof(AuthenticationValidationFilter));
                bootstrap.StartUp(null, new CommandServerModule(), new RepositoryModule());
            }

            Console.ReadLine();
        }
    }
}
