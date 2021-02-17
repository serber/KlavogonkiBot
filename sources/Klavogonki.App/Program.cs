using System;
using Klavogonki.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Klavogonki.App
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            var configuration = ConfigurationFactory.Create();
            var serviceProvider = ServiceProviderFactory.Create(configuration);

            var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();

            using var scope = serviceScopeFactory.CreateScope();
            var runner = scope.ServiceProvider.GetService<IGameRunner>();
            
            runner.Run();

            Console.ReadKey();
        }
    }
}