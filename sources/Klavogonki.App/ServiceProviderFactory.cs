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
    /// <summary>
    /// <see cref="IServiceProvider"/> factory
    /// </summary>
    internal static class ServiceProviderFactory
    {
        /// <summary>
        /// Create <see cref="IServiceProvider"/>
        /// </summary>
        internal static IServiceProvider Create(IConfiguration configuration)
        {
            return new ServiceCollection()
                .AddLogging(builder => builder.AddConsole())
                .AddOptions()
                .Configure<GameOptions>(configuration.GetSection(nameof(GameOptions)).Bind)
                .Configure<CustomGameOptions>(configuration.GetSection(nameof(CustomGameOptions)).Bind)
                .Configure<AuthenticationOptions>(configuration.GetSection(nameof(AuthenticationOptions)).Bind)
                .Configure<GameRunnerOptions>(configuration.GetSection(nameof(GameRunnerOptions)).Bind)
                .AddSingleton<IGameRunner, GameRunner>()
                .AddSingleton<ITextExtractor, TextExtractor>()
                .AddSingleton<IDelayCalculator, AverageDelayCalculator>()
                .AddScoped<IInputMapBuilder, InputMapBuilder>()
                .AddGameStrategy(configuration)
                .AddScoped<IAuthenticationService, LoginPasswordAuthenticationService>()
                .AddScoped<IWebDriver>(provider => new ChromeDriver())
                .BuildServiceProvider();
        }

        /// <summary>
        /// Register game strategy
        /// </summary>
        private static IServiceCollection AddGameStrategy(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var strategy = configuration.GetValue<string>("GameRunnerOptions:GameStrategy");
            switch (strategy)
            {
                case "Random":
                    serviceCollection.AddScoped<IGameStrategy, RandomGameStrategy>();
                    break;

                case "Competition":
                    serviceCollection.AddScoped<IGameStrategy, CompetitionGameStrategy>();
                    break;

                case "Custom":
                    serviceCollection.AddScoped<IGameStrategy, CustomGameStrategy>();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return serviceCollection;
        }
    }
}