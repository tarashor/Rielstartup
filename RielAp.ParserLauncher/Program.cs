using Ninject;
using RielAp.AddressService.Services;
using RielAp.AddressService.YandexServices;
using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Domain.Repositories.Implementations;
using RielAp.Parser.Helper;
using RielAp.Parser.Loader;
using RielAp.Parser.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.ParserLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel();
            using (RealtyDBContext db = new RealtyDBContext())
            {
                kernel.Bind<IVashmagazinRepository>().To<VashmagazinRepository>().WithConstructorArgument("context", db);
                kernel.Bind<ILoader>().To<VashmagazinHttpClientLoader>();
                kernel.Bind<ILoger>().To<ConsoleLoger>();
                kernel.Bind<IAnnouncementsParser>().To<VashmagazinAnnouncementsParserParallel>();

                IAnnouncementsParser parser = kernel.Get<IAnnouncementsParser>();
                int count = parser.ParseApartments();
                Console.WriteLine(count);
                Console.WriteLine("Finished");

            }
            Console.ReadKey();
        }
    }
}
