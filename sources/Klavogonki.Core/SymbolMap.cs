using System.Collections.Generic;

namespace Klavogonki.Core
{
    /// <summary>
    /// Symbol maps for replacement
    /// </summary>
    internal static class SymbolMap
    {
        /// <summary>
        /// English -> Russian map
        /// </summary>
        internal static IDictionary<char, char> EnglishToRussianMap = new Dictionary<char, char>
        {
            {'e', 'е'},
            {'y', 'у'},
            {'o', 'о'},
            {'p', 'р'},
            {'a', 'а'},
            {'k', 'к'},
            {'c', 'с'},
            {'m', 'м'}
        };
    }
}