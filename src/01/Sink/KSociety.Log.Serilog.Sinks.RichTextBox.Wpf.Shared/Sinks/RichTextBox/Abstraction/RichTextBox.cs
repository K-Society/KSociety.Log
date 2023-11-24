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
    private readonly object _syncRoot;

    public RichTextBox(System.Windows.Controls.RichTextBox richTextBox, object syncRoot)
    {
        this._richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
        this._syncRoot = syncRoot;
        this._richTextBox.TextChanged += this.RichTextBoxTextChanged;
    }

    private void RichTextBoxTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        lock (this._syncRoot)
        {
            if (sender is System.Windows.Controls.RichTextBox richTextBox)
            {
                richTextBox.ScrollToEnd();
            }
        }
    }

    public void LimitRows()
    {
        lock (this._syncRoot)
        {
            try
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
            catch (Exception)
            {
                ;
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
            return;
        }

        var inLines = parsedParagraph.Inlines.ToList();

        lock (this._syncRoot)
        {
            try
            {
                var flowDocument = this._richTextBox.Document ??= new FlowDocument();

                if (flowDocument.Blocks.LastBlock is not Paragraph paragraph)
                {
                    paragraph = new Paragraph();
                    flowDocument.Blocks.Add(paragraph);
                }

                paragraph.Inlines.AddRange(inLines);
            }
            catch (Exception)
            {
                ;
            }
        }
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
