﻿// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes
{
    using System.IO;

    /// <summary>
    /// The base class for styled RichTextBox output.
    /// </summary>
    public abstract class RichTextBoxTheme
    {
        /// <summary>
        /// No styling applied.
        /// </summary>
        public static RichTextBoxTheme None { get; } = new EmptyRichTextBoxTheme();

        /// <summary>
        /// True if styling applied by the theme is written into the output, and can thus be
        /// buffered and measured.
        /// </summary>
        public abstract bool CanBuffer { get; }

        /// <summary>
        /// Begin a span of text in the specified <paramref name="style"/>.
        /// </summary>
        /// <param name="output">Output destination.</param>
        /// <param name="style">Style to apply.</param>
        /// <returns> The number of characters written to <paramref name="output"/>. </returns>
        public abstract int Set(TextWriter output, RichTextBoxThemeStyle style);

        /// <summary>
        /// Reset the output to un-styled colors.
        /// </summary>
        /// <param name="output">Output destination.</param>
        public abstract void Reset(TextWriter output);

        /// <summary>
        /// The number of characters written by the <see cref="Reset(TextWriter)"/> method.
        /// </summary>
        protected abstract int ResetCharCount { get; }

        internal StyleReset Apply(TextWriter output, RichTextBoxThemeStyle style, ref int invisibleCharacterCount)
        {
            invisibleCharacterCount += this.Set(output, style);
            invisibleCharacterCount += this.ResetCharCount;

            return new StyleReset(this, output);
        }
    }
}