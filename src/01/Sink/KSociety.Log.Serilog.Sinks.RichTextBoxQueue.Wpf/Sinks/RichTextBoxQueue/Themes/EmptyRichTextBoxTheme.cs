using System.IO;

namespace KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue.Themes
{
    internal class EmptyRichTextBoxTheme : RichTextBoxTheme
    {
        public override bool CanBuffer => true;

        protected override int ResetCharCount { get; } = 0;

        public override int Set(TextWriter output, RichTextBoxThemeStyle style) => 0;

        public override void Reset(TextWriter output)
        {
        }
    }
}
