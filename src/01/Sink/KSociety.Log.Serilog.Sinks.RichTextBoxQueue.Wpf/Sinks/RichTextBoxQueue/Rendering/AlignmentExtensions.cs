using Serilog.Parsing;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Rendering
{
    internal static class AlignmentExtensions
    {
        public static Alignment Widen(this Alignment alignment, int amount)
        {
            return new Alignment(alignment.Direction, alignment.Width + amount);
        }
    }
}
