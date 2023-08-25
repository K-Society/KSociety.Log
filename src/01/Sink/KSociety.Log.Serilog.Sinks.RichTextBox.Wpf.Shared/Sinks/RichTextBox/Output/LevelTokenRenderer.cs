namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;
using System.Collections.Generic;
using System.IO;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Rendering;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using global::Serilog.Events;
using global::Serilog.Parsing;

internal class LevelTokenRenderer : OutputTemplateTokenRenderer
{
    private readonly RichTextBoxTheme _theme;
    private readonly PropertyToken _levelToken;

    private static readonly Dictionary<LogEventLevel, RichTextBoxThemeStyle> Levels = new Dictionary<LogEventLevel, RichTextBoxThemeStyle>
    {
        { LogEventLevel.Verbose, RichTextBoxThemeStyle.LevelVerbose },
        { LogEventLevel.Debug, RichTextBoxThemeStyle.LevelDebug },
        { LogEventLevel.Information, RichTextBoxThemeStyle.LevelInformation },
        { LogEventLevel.Warning, RichTextBoxThemeStyle.LevelWarning },
        { LogEventLevel.Error, RichTextBoxThemeStyle.LevelError },
        { LogEventLevel.Fatal, RichTextBoxThemeStyle.LevelFatal },
    };

    // ReSharper disable once UnusedMember.Global
    protected LevelTokenRenderer()
    {
    }

    public LevelTokenRenderer(RichTextBoxTheme theme, PropertyToken levelToken)
    {
        this._theme = theme;
        this._levelToken = levelToken;
    }

    public override void Render(LogEvent logEvent, TextWriter output)
    {
        var moniker = LevelOutputFormat.GetLevelMoniker(logEvent.Level, this._levelToken.Format);
        if (!Levels.TryGetValue(logEvent.Level, out var levelStyle))
        {
            levelStyle = RichTextBoxThemeStyle.Invalid;
        }

        var _ = 0;
        using (this._theme.Apply(output, levelStyle, ref _))
        {
            Padding.Apply(output, moniker, this._levelToken.Alignment);
        }
    }
}