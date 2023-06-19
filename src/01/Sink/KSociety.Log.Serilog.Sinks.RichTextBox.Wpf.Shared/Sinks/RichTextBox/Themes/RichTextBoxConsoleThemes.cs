using System.Collections.Generic;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;

public static class RichTextBoxConsoleThemes
{
    public static RichTextBoxConsoleTheme Literate { get; } = new RichTextBoxConsoleTheme
    (
        new Dictionary<RichTextBoxThemeStyle, RichTextBoxConsoleThemeStyle>
        {
            [RichTextBoxThemeStyle.Text] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.SecondaryText] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Gray },
            [RichTextBoxThemeStyle.TertiaryText] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.DarkGray },
            [RichTextBoxThemeStyle.Invalid] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Yellow },
            [RichTextBoxThemeStyle.Null] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Blue },
            [RichTextBoxThemeStyle.Name] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Gray },
            [RichTextBoxThemeStyle.String] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Cyan },
            [RichTextBoxThemeStyle.Number] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Magenta },
            [RichTextBoxThemeStyle.Boolean] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Blue },
            [RichTextBoxThemeStyle.Scalar] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Green },
            [RichTextBoxThemeStyle.LevelVerbose] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Gray },
            [RichTextBoxThemeStyle.LevelDebug] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Gray },
            [RichTextBoxThemeStyle.LevelInformation] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.LevelWarning] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Yellow },
            [RichTextBoxThemeStyle.LevelError] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White, Background = ConsoleHtmlColor.Red },
            [RichTextBoxThemeStyle.LevelFatal] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White, Background = ConsoleHtmlColor.Red },
        }
    );

    public static RichTextBoxConsoleTheme Grayscale { get; } = new RichTextBoxConsoleTheme
    (
        new Dictionary<RichTextBoxThemeStyle, RichTextBoxConsoleThemeStyle>
        {
            [RichTextBoxThemeStyle.Text] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.SecondaryText] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Gray },
            [RichTextBoxThemeStyle.TertiaryText] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.DarkGray },
            [RichTextBoxThemeStyle.Invalid] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White, Background = ConsoleHtmlColor.DarkGray },
            [RichTextBoxThemeStyle.Null] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.Name] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Gray },
            [RichTextBoxThemeStyle.String] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.Number] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.Boolean] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.Scalar] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.LevelVerbose] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.DarkGray },
            [RichTextBoxThemeStyle.LevelDebug] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.DarkGray },
            [RichTextBoxThemeStyle.LevelInformation] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.LevelWarning] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White, Background = ConsoleHtmlColor.DarkGray },
            [RichTextBoxThemeStyle.LevelError] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Black, Background = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.LevelFatal] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Black, Background = ConsoleHtmlColor.White },
        }
    );

    public static RichTextBoxConsoleTheme Colored { get; } = new RichTextBoxConsoleTheme
    (
        new Dictionary<RichTextBoxThemeStyle, RichTextBoxConsoleThemeStyle>
        {
            [RichTextBoxThemeStyle.Text] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Gray },
            [RichTextBoxThemeStyle.SecondaryText] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.DarkGray },
            [RichTextBoxThemeStyle.TertiaryText] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.DarkGray },
            [RichTextBoxThemeStyle.Invalid] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Yellow },
            [RichTextBoxThemeStyle.Null] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.Name] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.String] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.Number] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.Boolean] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.Scalar] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White },
            [RichTextBoxThemeStyle.LevelVerbose] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.Gray, Background = ConsoleHtmlColor.DarkGray },
            [RichTextBoxThemeStyle.LevelDebug] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White, Background = ConsoleHtmlColor.DarkGray },
            [RichTextBoxThemeStyle.LevelInformation] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White, Background = ConsoleHtmlColor.Blue },
            [RichTextBoxThemeStyle.LevelWarning] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.DarkGray, Background = ConsoleHtmlColor.Yellow },
            [RichTextBoxThemeStyle.LevelError] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White, Background = ConsoleHtmlColor.Red },
            [RichTextBoxThemeStyle.LevelFatal] = new RichTextBoxConsoleThemeStyle { Foreground = ConsoleHtmlColor.White, Background = ConsoleHtmlColor.Red },
        }
    );
}