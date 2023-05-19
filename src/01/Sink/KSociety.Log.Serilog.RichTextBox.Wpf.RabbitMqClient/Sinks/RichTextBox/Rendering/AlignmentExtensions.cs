using Serilog.Parsing;

namespace KSociety.Log.Serilog.RichTextBox.Wpf.RabbitMqClient.Sinks.RichTextBox.Rendering
{
    internal static class AlignmentExtensions
    {
        public static Alignment Widen(this Alignment alignment, int amount)
        {
            return new Alignment(alignment.Direction, alignment.Width + amount);
        }
    }
}
