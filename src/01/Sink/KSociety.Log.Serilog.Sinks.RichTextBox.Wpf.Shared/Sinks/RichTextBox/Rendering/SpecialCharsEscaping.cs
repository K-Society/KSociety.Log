// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Rendering
{
    using System.Security;

    internal static class SpecialCharsEscaping
    {
        public static string? Apply(string? value, ref int invisibleCharacterCount)
        {
            var escapedValue = SecurityElement.Escape(value) ?? System.String.Empty;
            invisibleCharacterCount += escapedValue.Length - value.Length;

            return escapedValue;
        }
    }
}