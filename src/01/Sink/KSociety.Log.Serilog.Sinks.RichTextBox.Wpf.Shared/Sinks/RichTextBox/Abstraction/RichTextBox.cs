namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using System;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Threading;
using global::Serilog.Debugging;

public class RichTextBox : IRichTextBox
{
    private readonly System.Windows.Controls.RichTextBox _richTextBox;

    public RichTextBox(System.Windows.Controls.RichTextBox richTextBox)
    {
        this._richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
        this._richTextBox.TextChanged += RichTextBoxTextChanged;
    }

    private static void RichTextBoxTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (sender is System.Windows.Controls.RichTextBox richTextBox)
        {
            richTextBox.ScrollToEnd();
        }
    }

    public void LimitRows()
    {
        var flowDocument = this._richTextBox.Document;

        if (flowDocument.Blocks.LastBlock is Paragraph paragraph)
        {
            while (paragraph.Inlines.Count > 1000)
            {
                paragraph.Inlines.Remove(paragraph.Inlines.FirstInline);
            }
        }
    }

    public void Write(string xamlParagraphText)
    {
        Paragraph parsedParagraph;

        try
        {
            parsedParagraph = (Paragraph) XamlReader.Parse(xamlParagraphText);
        }
        catch (XamlParseException ex)
        {
            SelfLog.WriteLine($"Error parsing `{xamlParagraphText}` to XAML: {ex.Message}");
            throw;
        }

        var inLines = parsedParagraph.Inlines.ToList();

        var flowDocument = this._richTextBox.Document ??= new FlowDocument();

        if (flowDocument.Blocks.LastBlock is not Paragraph paragraph)
        {
            paragraph = new Paragraph();
            flowDocument.Blocks.Add(paragraph);
        }

        paragraph.Inlines.AddRange(inLines);
    }

    public bool CheckAccess()
    {
        return this._richTextBox.CheckAccess();
    }

    public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg)
    {
        return this._richTextBox.Dispatcher.BeginInvoke(priority, method, arg);
    }
}
