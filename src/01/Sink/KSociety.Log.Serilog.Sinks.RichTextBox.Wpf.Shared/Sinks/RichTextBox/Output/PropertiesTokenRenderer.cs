using System;
using System.IO;
using System.Linq;
using System.Text;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Formatting;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Rendering;
using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Themes;
using Serilog.Events;
using Serilog.Parsing;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Output;

internal class PropertiesTokenRenderer : OutputTemplateTokenRenderer
{
    private readonly MessageTemplate _outputTemplate;
    private readonly RichTextBoxTheme _theme;
    private readonly PropertyToken _token;
    private readonly ThemedValueFormatter _valueFormatter;

    public PropertiesTokenRenderer(RichTextBoxTheme theme, PropertyToken token, MessageTemplate outputTemplate, IFormatProvider formatProvider)
    {
        _outputTemplate = outputTemplate;
        _theme = theme ?? throw new ArgumentNullException(nameof(theme));
        _token = token ?? throw new ArgumentNullException(nameof(token));

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

        _valueFormatter = isJson
            ? (ThemedValueFormatter)new ThemedJsonValueFormatter(theme, formatProvider)
            : new ThemedDisplayValueFormatter(theme, formatProvider);
    }

    public override void Render(LogEvent logEvent, TextWriter output)
    {
        var included = logEvent.Properties
            .Where(p => !TemplateContainsPropertyName(logEvent.MessageTemplate, p.Key) &&
                        !TemplateContainsPropertyName(_outputTemplate, p.Key))
            .Select(p => new LogEventProperty(p.Key, p.Value));

        var value = new StructureValue(included);

        if (_token.Alignment is null || !_theme.CanBuffer)
        {
            _valueFormatter.Format(value, output, null);
            return;
        }

        var buffer = new StringWriter(new StringBuilder(value.Properties.Count * 16));
        var invisible = _valueFormatter.Format(value, buffer, null);
        var str = buffer.ToString();

        Padding.Apply(output, str, _token.Alignment.Value.Widen(invisible));
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