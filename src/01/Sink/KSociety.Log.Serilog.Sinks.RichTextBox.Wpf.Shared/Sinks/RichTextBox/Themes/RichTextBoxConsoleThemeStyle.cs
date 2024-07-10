// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes
{
    /// <summary>
    /// Styling applied for Foreground and Background colors.
    /// </summary>
    public struct RichTextBoxConsoleThemeStyle
    {
        /// <summary>
        /// The background color to apply. e.g. #ffffff
        /// </summary>
        public string? Background;

        /// <summary>
        /// The foreground color to apply. e.g. #ff0000
        /// </summary>
        public string? Foreground;

        /// <summary>
        /// The font weight to apply. e.g. Bold
        /// </summary>
        public string? FontWeight;
    }
}