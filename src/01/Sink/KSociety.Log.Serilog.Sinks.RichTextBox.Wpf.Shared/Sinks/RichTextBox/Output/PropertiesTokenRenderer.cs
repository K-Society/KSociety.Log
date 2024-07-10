// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Formatting;
    using Rendering;
    using Themes;
    using global::Serilog.Events;
    using global::Serilog.Parsing;

    internal class PropertiesTokenRenderer : OutputTemplateTokenRenderer
    {
        private readonly MessageTemplate _outputTemplate;
        private readonly RichTextBoxTheme _theme;
        private readonly PropertyToken _token;
        private readonly ThemedValueFormatter _valueFormatter;

        public PropertiesTokenRenderer(RichTextBoxTheme theme, PropertyToken token, MessageTemplate outputTemplate, IFormatProvider formatProvider)
        {
            this._outputTemplate = outputTemplate;
            this._theme = theme ?? throw new ArgumentNullException(nameof(theme));
            this._token = token ?? throw new ArgumentNullException(nameof(token));

            var isJson = false;

            if (token.Format != null)
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < token.Format.Length; ++i)
                {
                    if (token.Format[i] == 'j')
                    {
                        isJson = true;
                    }
                }
            }

            this._valueFormatter = isJson
                ? (ThemedValueFormatter)new ThemedJsonValueFormatter(theme, formatProvider)
                : new ThemedDisplayValueFormatter(theme, formatProvider);
        }

        public override void Render(LogEvent logEvent, TextWriter output)
        {
            var included = logEvent.Properties
                .Where(p => !TemplateContainsPropertyName(logEvent.MessageTemplate, p.Key) &&
                            !TemplateContainsPropertyName(this._outputTemplate, p.Key))
                .Select(p => new LogEventProperty(p.Key, p.Value));

            var value = new StructureValue(included);

            if (this._token.Alignment is null || !this._theme.CanBuffer)
            {
                this._valueFormatter.Format(value, output, null);
                return;
            }

            var buffer = new StringWriter(new StringBuilder(value.Properties.Count * 16));
            var invisible = this._valueFormatter.Format(value, buffer, null);
            var str = buffer.ToString();

            Padding.Apply(output, str, this._token.Alignment.Value.Widen(invisible));
        }

        private static bool TemplateContainsPropertyName(MessageTemplate template, string propertyName)
        {
            foreach (var token in template.Tokens)
            {
                if (token is PropertyToken namedProperty &&
                    namedProperty.PropertyName == propertyName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
