namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Threading;
using global::Serilog.Events;
using global::Serilog.Formatting;

public class RichTextBox : IRichTextBox
{
    private readonly System.Windows.Controls.RichTextBox? _richTextBox;
    private readonly object _syncRoot;
    private readonly ITextFormatter? _formatter;
    private readonly DispatcherPriority _dispatcherPriority;
    private readonly BackgroundWorker _backgroundWorker;

    private const string Paragraph1 = "<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">";
    private const string Paragraph2 = "</Paragraph>";
    private static readonly Thickness Thickness = new(0D);
    private static readonly StringBuilder StringBuilder = new();

    private delegate void RichTextBoxAppendTextDelegate(System.Windows.Controls.RichTextBox wpfRichTextBox, string xamlParagraphText, object syncRoot);

    private delegate void RichTextBoxLimiterDelegate(System.Windows.Controls.RichTextBox wpfRichTextBox, object? args, object syncRoot);

    public RichTextBox(System.Windows.Controls.RichTextBox? richTextBox, ITextFormatter? formatter, DispatcherPriority dispatcherPriority, object syncRoot)
    {
        this._richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
        this._formatter = formatter;
        this._syncRoot = syncRoot;
        this._dispatcherPriority = dispatcherPriority;

        this._backgroundWorker = new BackgroundWorker();

        this._backgroundWorker.DoWork += this.BackgroundWorkerOnDoWork;
        this._backgroundWorker.RunWorkerCompleted += this.BackgroundWorkerOnRunWorkerCompleted;
    }

    private void BackgroundWorkerOnRunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        //if (e.Cancelled)
        //{
        //    return;
        //}
        ;
        //this._backgroundWorker.RunWorkerAsync();
    }

    public void StartRichTextBoxLimiter()
    {
        if (!this._backgroundWorker.IsBusy)
        {
            this._backgroundWorker.RunWorkerAsync();
        }
    }

    public void StopRichTextBoxLimiter()
    {
        //if (!this._backgroundWorker.IsBusy)
        //{
            this._backgroundWorker.CancelAsync();
        //}
    }

    private async void BackgroundWorkerOnDoWork(object? sender, DoWorkEventArgs e)
    {
        var helperBackgroundWorker = sender as BackgroundWorker;

        e.Result = await this.BackgroundProcessLogicMethod(helperBackgroundWorker, e.Argument).ConfigureAwait(false);

        if (helperBackgroundWorker.CancellationPending)
        {
            e.Cancel = true;
        }
    }

    private async ValueTask<bool> BackgroundProcessLogicMethod(BackgroundWorker helperBackgroundWorker, object? arg)
    {
        try
        {
            var delegateArray = new object[3];

            delegateArray[0] = this._richTextBox;
            delegateArray[1] = arg;
            delegateArray[2] = this._syncRoot;

            await this._richTextBox.Dispatcher.BeginInvoke(new RichTextBoxLimiterDelegate(RichTextBoxLimiterDelegateMethod), this._dispatcherPriority, delegateArray);
            return true;
        }
        catch (Exception ex)
        {
            ;
        }
        return false;
    }

    public bool CheckAccess()
    {
        return this._richTextBox.CheckAccess();
    }

    public async ValueTask BeginInvoke(string? xamlParagraphText)
    {
        try
        {
            var delegateArray = new object[3];
            
            delegateArray[0] = this._richTextBox;
            delegateArray[1] = xamlParagraphText;
            delegateArray[2] = this._syncRoot;

            await this._richTextBox.Dispatcher.BeginInvoke(new RichTextBoxAppendTextDelegate(this.RichTextBoxAppendTextDelegateMethod), this._dispatcherPriority, delegateArray);
        }
        catch (Exception ex)
        {
            ;
        }
    }

    private void RichTextBoxAppendTextDelegateMethod(System.Windows.Controls.RichTextBox wpfRichTextBox, string xamlParagraphText, object syncRoot)
    {
        try
        {
            var parsedParagraph = (Paragraph)XamlReader.Parse(xamlParagraphText);
            parsedParagraph.Margin = Thickness;

            var flowDocument = wpfRichTextBox.Document ??= new FlowDocument();

            if (flowDocument.Blocks.LastBlock is Paragraph {Inlines.Count: 0} paragraph)
            {
                paragraph.Inlines.AddRange(parsedParagraph.Inlines.ToList());
                paragraph.Margin = Thickness;
            }
            else
            {
                flowDocument.Blocks.Add(parsedParagraph);
            }
                
            wpfRichTextBox.ScrollToEnd();

            if (flowDocument.Blocks.Count > 1000)
            {
                this.StartRichTextBoxLimiter();
            }
        }
        catch (Exception)
        {
            ;
        }
    }

    private static void RichTextBoxLimiterDelegateMethod(System.Windows.Controls.RichTextBox wpfRichTextBox, object? args, object syncRoot)
    {
        try
        {
            if (wpfRichTextBox.Document.Blocks.Count > 1000)
            {
                var blocksToRemove = wpfRichTextBox.Document.Blocks.Count - 1000;
                for (var i = 0; i < blocksToRemove; i++)
                {
                    wpfRichTextBox.Document.Blocks.Remove(wpfRichTextBox.Document.Blocks.FirstBlock);
                }
            }
        }
        catch (Exception)
        {
            ;
        }
    }

    public void OnCompleted()
    {
        ;
    }

    public void OnError(Exception error)
    {
        ;
    }

    public async void OnNext(LogEvent value)
    {
        StringWriter writer = new();
        
        try
        {
            StringBuilder.Clear();
            StringBuilder.Append(Paragraph1);

            this._formatter?.Format(value, writer);
            StringBuilder.Append(writer);

            StringBuilder.Append(Paragraph2);
            await this.BeginInvoke(StringBuilder.ToString()).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            ;
        }
        finally
        {
            await writer.DisposeAsync().ConfigureAwait(false);
        }
    }
}
