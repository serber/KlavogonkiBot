using System.Linq;
using System.Text;
using Klavogonki.Common;

namespace Klavogonki.Core
{
    /// <inheritdoc cref="ITextExtractor"/>
    public class TextExtractor : ITextExtractor
    {
        /// <inheritdoc cref="ITextExtractor.Extract"/>
        public string Extract(string html)
        {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            var textBuilder = new StringBuilder(); ;

            var spanCollection = document.DocumentNode.SelectSingleNode("//*[@id=\"typetext\"]/span")
                .Descendants("span")
                .ToList();

            foreach (var node in spanCollection)
            {
                if (node.HasAttributes)
                {
                    if (node.Attributes["style"] != null && node.Attributes["style"].Value.Contains("none"))
                    {
                        continue;
                    }

                    if (node.Id == "afterfocus")
                    {
                        continue;
                    }
                }

                if (node.ChildNodes != null && node.ChildNodes.Count > 1)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(node.InnerText))
                {
                    continue;
                }

                textBuilder.Append(node.InnerText);
            }

            foreach (var map in SymbolMap.EnglishToRussianMap)
            {
                textBuilder.Replace(map.Key, map.Value);
                textBuilder.Replace(char.ToUpper(map.Key), char.ToUpper(map.Value));
            }

            return textBuilder.ToString();
        }
    }
}