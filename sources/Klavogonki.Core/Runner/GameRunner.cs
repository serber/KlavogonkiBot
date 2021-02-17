using System;
using System.Threading;
using Klavogonki.Common;
using Klavogonki.Core.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Klavogonki.Core.Runner
{
    /// <inheritdoc cref="IGameRunner"/>
    public class GameRunner : IGameRunner
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ILogger<GameRunner> _logger;

        private readonly GameRunnerOptions _options;
        
        public GameRunner(IServiceScopeFactory serviceScopeFactory, IOptions<GameRunnerOptions> options, ILogger<GameRunner> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _options = options.Value;
        }

        /// <inheritdoc cref="IGameRunner.Run"/>
        public void Run()
        {
            var cancelationToken = new CancellationTokenSource(_options.RunTime);
            for (var i = 0; i < _options.ParallelGameCount; i++)
            {
                //  The delay is needed so as not to enter the same game with different background threads
                Thread.Sleep(TimeSpan.FromSeconds(1));

                ThreadPool.QueueUserWorkItem(state => RunGame(cancelationToken.Token));
            }
        }

        /// <summary>
        /// Launches the game
        /// </summary>
        private void RunGame(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var gameStrategy = scope.ServiceProvider.GetService<IGameStrategy>();

                _logger.LogInformation($"Selected strategy: {gameStrategy.GetType().Name}");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var success = gameStrategy.Play();

                    _logger.LogInformation($"Game complete with success = {success}");
                }

                _logger.LogInformation("Game canceled");

                if (gameStrategy is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}