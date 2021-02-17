using Klavogonki.Common;
using Klavogonki.Core.Options;
using Microsoft.Extensions.Options;

namespace Klavogonki.Core
{
    /// <inheritdoc cref="IInputMapBuilder"/>
    public class InputMapBuilder : IInputMapBuilder
    {
        private readonly IDelayCalculator _delayCalculator;

        private readonly GameOptions _options;

        public InputMapBuilder(IDelayCalculator delayCalculator, IOptions<GameOptions> options)
        {
            _delayCalculator = delayCalculator;
            _options = options.Value;
        }

        /// <inheritdoc cref="IInputMapBuilder.GetInputMaps"/>
        public InputMapEntry[] GetInputMaps(string text)
        {
            var delay = _delayCalculator.Calculate(_options.Speed);
            var maps = new InputMapEntry[text.Length];

            for (var i = 0; i < text.Length; i++)
            {
                var entry = new InputMapEntry
                {
                    Symbol = text[i],
                    ErrorSymbols = new char[0],
                    Delay = RandomHelper.AddRandomDeviation(delay, Constants.DeviationPercent)
                };

                var shouldTypo = RandomHelper.IsLucky(_options.ErrorPercent);
                if (shouldTypo)
                {
                    entry.ErrorSymbols = GetRandomChars(text);
                }

                maps[i] = entry;
            }

            return maps;
        }

        /// <summary>
        /// Returns random chars from string
        /// </summary>
        private char[] GetRandomChars(string value)
        {
            var random = RandomHelper.Random.Next(1, 3);
            var chars = new char[random];

            for (var i = 0; i < random; i++)
            {
                chars[i] = value[RandomHelper.Random.Next(0, value.Length - 1)];
            }

            return chars;
        }
    }
}