namespace Klavogonki.Common
{
    public interface IInputMapBuilder
    {
        /// <summary>
        /// Build <see cref="InputMapEntry"/> from text
        /// </summary>
        /// <param name="text">Text</param>
        InputMapEntry[] GetInputMaps(string text);
    }
}