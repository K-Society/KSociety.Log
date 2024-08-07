// Copyright � K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes
{
    public enum RichTextBoxThemeStyle
    {
        /// <summary>
        /// Prominent text, generally content within an event's message.
        /// </summary>
        Text,

        /// <summary>
        /// Boilerplate text, for example items specified in an output template.
        /// </summary>
        SecondaryText,

        /// <summary>
        /// De-emphasized text, for example literal text in output templates and
        /// punctuation used when writing structured data.
        /// </summary>
        TertiaryText,

        /// <summary>
        /// Output demonstrating some kind of configuration issue, e.g. an invalid
        /// message template token.
        /// </summary>
        Invalid,

        /// <summary>
        /// The built-in <see langword="null"/> value.
        /// </summary>
        Null,

        /// <summary>
        /// Property and type names.
        /// </summary>
        Name,

        /// <summary>
        /// Strings.
        /// </summary>
        String,

        /// <summary>
        /// Numbers.
        /// </summary>
        Number,

        /// <summary>
        /// <see cref="System.Boolean"/> values.
        /// </summary>
        Boolean,

        /// <summary>
        /// All other scalar values, e.g. <see cref="Guid"/> instances.
        /// </summary>
        Scalar,

        /// <summary>
        /// Level indicator.
        /// </summary>
        LevelVerbose,

        /// <summary>
        /// Level indicator.
        /// </summary>
        LevelDebug,

        /// <summary>
        /// Level indicator.
        /// </summary>
        LevelInformation,

        /// <summary>
        /// Level indicator.
        /// </summary>
        LevelWarning,

        /// <summary>
        /// Level indicator.
        /// </summary>
        LevelError,

        /// <summary>
        /// Level indicator.
        /// </summary>
        LevelFatal,
    }
}
