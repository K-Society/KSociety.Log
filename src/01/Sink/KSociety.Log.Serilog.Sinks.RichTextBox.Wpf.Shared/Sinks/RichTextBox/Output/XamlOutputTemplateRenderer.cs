// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Themes;
    using global::Serilog.Events;
    using global::Serilog.Formatting;
    using global::Serilog.Formatting.Display;
    using global::Serilog.Parsing;

    public class XamlOutputTemplateRenderer : ITextFormatter
    {
        private readonly OutputTemplateTokenRenderer[] _renderers;

        public XamlOutputTemplateRenderer(RichTextBoxTheme theme, string outputTemplate, IFormatProvider formatProvider)
        {
            if (outputTemplate is null)
            {
                throw new ArgumentNullException(nameof(outputTemplate));
            }

            var template = new MessageTemplateParser().Parse(outputTemplate);

            var renderers = new List<OutputTemplateTokenRenderer>();
            foreach (var token in template.Tokens)
            {
                if (token is TextToken tt)
                {
                    renderers.Add(new TextTokenRenderer(theme, tt.Text));
                    continue;
                }

                var pt = (PropertyToken) token;

                switch (pt.PropertyName)
                {
                    case OutputProperties.LevelPropertyName:
                        renderers.Add(new LevelTokenRenderer(theme, pt));
                        break;
                    case OutputProperties.NewLinePropertyName:
                        renderers.Add(new NewLineTokenRenderer(pt.Alignment));
                        break;
                    case OutputProperties.ExceptionPropertyName:
                        renderers.Add(new ExceptionTokenRenderer(theme));
                        break;
                    case OutputProperties.MessagePropertyName:
                        renderers.Add(new MessageTemplateOutputTokenRenderer(theme, pt, formatProvider));
                        break;
                    case OutputProperties.TimestampPropertyName:
                        renderers.Add(new TimestampTokenRenderer(theme, pt, formatProvider));
                        break;
                    case "Properties":
                        renderers.Add(new PropertiesTokenRenderer(theme, pt, template, formatProvider));
                        break;
                    default:
                        renderers.Add(new EventPropertyTokenRenderer(theme, pt, formatProvider));
                        break;
                }
            }

            this._renderers = renderers.ToArray();
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            if (logEvent is null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            foreach (var renderer in this._renderers)
            {
                renderer.Render(logEvent, output);
            }
        }
    }
}
