using System;
using Klavogonki.Common;
using Klavogonki.Common.Auth;
using Klavogonki.Core;
using Klavogonki.Core.Auth;
using Klavogonki.Core.Options;
using Klavogonki.Core.Runner;
using Klavogonki.Core.Strategies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Klavogonki.App
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddConsole())
                .AddOptions()
                .Configure<GameOptions>(configuration.GetSection(nameof(GameOptions)).Bind)
                .Configure<CustomGameOptions>(configuration.GetSection(nameof(CustomGameOptions)).Bind)
                .Configure<AuthenticationOptions>(configuration.GetSection(nameof(AuthenticationOptions)).Bind)
                .Configure<GameRunnerOptions>(configuration.GetSection(nameof(GameRunnerOptions)).Bind)
                .AddSingleton<IGameRunner, GameRunner>()
                .AddSingleton<ITextExtractor, TextExtractor>()
                //.AddScoped<IGameStrategy, RandomGameStrategy>()
                .AddScoped<IGameStrategy, CompetitionGameStrategy>()
                //.AddScoped<IGameStrategy, CustomGameStrategy>()
                .AddScoped<IAuthenticationService, LoginPasswordAuthenticationService>()
                .AddScoped<IWebDriver>(provider => new ChromeDriver())
                .BuildServiceProvider();

            var serviceScopeFactory = serviceProvider.GetService<IServiceScopeFactory>();

            using var scope = serviceScopeFactory.CreateScope();
            var runner = scope.ServiceProvider.GetService<IGameRunner>();

            runner.Run();

            Console.ReadKey();
        }
    }
}