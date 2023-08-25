namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Rendering;
using global::Serilog.Parsing;

internal static class AlignmentExtensions
{
    public static Alignment Widen(this Alignment alignment, int amount)
    {
        return new Alignment(alignment.Direction, alignment.Width + amount);
    }
}