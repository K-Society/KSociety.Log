namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using global::Serilog.Debugging;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Threading;

public class RichTextBox : IRichTextBox
{
    private readonly System.Windows.Controls.RichTextBox _richTextBox;
    //private static readonly object DefaultSyncRoot = new();

    public RichTextBox(System.Windows.Controls.RichTextBox richTextBox)
    {
        this._richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
        this._richTextBox.DataContextChanged += this.RichTextBoxOnDataContextChanged;
    }

    private void RichTextBoxOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is System.Windows.Controls.RichTextBox richTextBox)
        {
            var flowDocument = this._richTextBox.Document;

            if (flowDocument.Blocks.LastBlock is Paragraph paragraph)
            {
                while (paragraph.Inlines.Count > 500)
                {
                    paragraph.Inlines.Remove(paragraph.Inlines.FirstInline);
                }
            }

            richTextBox.ScrollToEnd();
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
