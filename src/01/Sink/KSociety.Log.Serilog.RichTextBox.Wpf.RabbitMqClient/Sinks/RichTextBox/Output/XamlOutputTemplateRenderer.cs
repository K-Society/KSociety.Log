﻿using System;
using System.Collections.Generic;
using System.IO;
using KSociety.Log.Serilog.RichTextBox.Wpf.RabbitMqClient.Sinks.RichTextBox.Themes;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Parsing;

namespace KSociety.Log.Serilog.RichTextBox.Wpf.RabbitMqClient.Sinks.RichTextBox.Output
{
    internal class XamlOutputTemplateRenderer : ITextFormatter
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

            _renderers = renderers.ToArray();
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

            foreach (var renderer in _renderers)
            {
                renderer.Render(logEvent, output);
            }
        }
    }
}