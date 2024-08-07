﻿// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Rendering
{
    using System.IO;
    using global::Serilog.Parsing;

    internal static class Padding
    {
        private static readonly char[] _paddingChars = new string(' ', 80).ToCharArray();

        /// <summary>
        /// Writes the provided value to the output, applying direction-based padding when <paramref name="alignment"/> is provided.
        /// </summary>
        /// <param name="output">Output object to write result.</param>
        /// <param name="value">Provided value.</param>
        /// <param name="alignment">The alignment settings to apply when rendering <paramref name="value"/>.</param>
        public static void Apply(TextWriter output, string value, Alignment? alignment)
        {
            if (alignment is null || value.Length >= alignment.Value.Width)
            {
                output.Write(value);
                return;
            }

            var pad = alignment.Value.Width - value.Length;

            if (alignment.Value.Direction == AlignmentDirection.Left)
            {
                output.Write(value);
            }

            if (pad <= _paddingChars.Length)
            {
                output.Write(_paddingChars, 0, pad);
            }
            else
            {
                output.Write(new string(' ', pad));
            }

            if (alignment.Value.Direction == AlignmentDirection.Right)
            {
                output.Write(value);
            }
        }
    }
}