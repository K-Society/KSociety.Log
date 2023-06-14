using System.Security;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Rendering
{
    internal static class SpecialCharsEscaping
    {
        public static string Apply(string value, ref int invisibleCharacterCount)
        {
            var escapedValue = SecurityElement.Escape(value) ?? string.Empty;
            invisibleCharacterCount += escapedValue.Length - value.Length;

            return escapedValue;
        }
    }
}
