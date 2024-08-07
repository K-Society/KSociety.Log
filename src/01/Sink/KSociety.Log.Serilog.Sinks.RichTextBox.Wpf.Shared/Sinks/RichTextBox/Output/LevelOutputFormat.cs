// Copyright � K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output
{
    using Rendering;
    using global::Serilog.Events;

    /// <summary>
    /// Implements the {Level} element.
    /// can now have a fixed width applied to it, as well as casing rules.
    /// Width is set through formats like "u3" (uppercase three chars),
    /// "w1" (one lowercase char), or "t4" (title case four chars).
    /// </summary>
    internal static class LevelOutputFormat
    {
        private static readonly string[][] _titleCaseLevelMap =
        {
            new[] { "V", "Vb", "Vrb", "Verb" },
            new[] { "D", "De", "Dbg", "Dbug" },
            new[] { "I", "In", "Inf", "Info" },
            new[] { "W", "Wn", "Wrn", "Warn" },
            new[] { "E", "Er", "Err", "Eror" },
            new[] { "F", "Fa", "Ftl", "Fatl" },
        };

        private static readonly string[][] _lowercaseLevelMap =
        {
            new[] { "v", "vb", "vrb", "verb" },
            new[] { "d", "de", "dbg", "dbug" },
            new[] { "i", "in", "inf", "info" },
            new[] { "w", "wn", "wrn", "warn" },
            new[] { "e", "er", "err", "eror" },
            new[] { "f", "fa", "ftl", "fatl" },
        };

        private static readonly string[][] _uppercaseLevelMap =
        {
            new[] { "V", "VB", "VRB", "VERB" },
            new[] { "D", "DE", "DBG", "DBUG" },
            new[] { "I", "IN", "INF", "INFO" },
            new[] { "W", "WN", "WRN", "WARN" },
            new[] { "E", "ER", "ERR", "EROR" },
            new[] { "F", "FA", "FTL", "FATL" },
        };

        public static string GetLevelMoniker(LogEventLevel value, string format = null)
        {
            if (format is null || format.Length != 2 && format.Length != 3)
            {
                return Casing.Format(value.ToString(), format);
            }

            // Using int.Parse() here requires allocating a string to exclude the first character prefix.
            // Junk like "wxy" will be accepted but produce benign results.
            var width = format[1] - '0';
            if (format.Length == 3)
            {
                width *= 10;
                width += format[2] - '0';
            }

            if (width < 1)
            {
                return System.String.Empty;
            }

            if (width > 4)
            {
                var stringValue = value.ToString();
                if (stringValue.Length > width)
                {
                    stringValue = stringValue.Substring(0, width);
                }

                return Casing.Format(stringValue);
            }

            var index = (int)value;
            if (index >= 0 && index <= (int)LogEventLevel.Fatal)
            {
                switch (format[0])
                {
                    case 'w':
                        return _lowercaseLevelMap[index][width - 1];
                    case 'u':
                        return _uppercaseLevelMap[index][width - 1];
                    case 't':
                        return _titleCaseLevelMap[index][width - 1];
                }
            }

            return Casing.Format(value.ToString(), format);
        }
    }
}
