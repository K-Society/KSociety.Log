using Serilog.Parsing;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Sinks.RichTextBox.Rendering
{
    internal static class AlignmentExtensions
    {
        public static Alignment Widen(this Alignment alignment, int amount)
        {
            return new Alignment(alignment.Direction, alignment.Width + amount);
        }
    }
}
