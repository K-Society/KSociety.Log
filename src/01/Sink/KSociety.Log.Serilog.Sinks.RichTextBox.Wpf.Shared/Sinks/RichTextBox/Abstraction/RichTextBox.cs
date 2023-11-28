namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    private delegate void RichTextBoxAppendTextDelegate(System.Windows.Controls.RichTextBox wpfRichTextBox,
        string xamlParagraphText, /*Table table, TableRowGroup tableRowGroup,*/ object syncRoot);

    //#region [Fields]

    //private readonly Table _table = new();
    //private readonly TableRowGroup _tableRowGroup = new();

    //#endregion

    public RichTextBox(System.Windows.Controls.RichTextBox? richTextBox, ITextFormatter? formatter, DispatcherPriority dispatcherPriority, object syncRoot)
    {
        this._richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
        this._formatter = formatter;
        this._syncRoot = syncRoot;
        this._dispatcherPriority = dispatcherPriority;


        //this._table.RowGroups.Add(this._tableRowGroup);

        //var column1 = new TableColumn() { Width = new GridLength(140, GridUnitType.Pixel) };
        //var column2 = new TableColumn() { Width = new GridLength(80, GridUnitType.Pixel) };
        //var column3 = new TableColumn() { Width = GridLength.Auto };

        //var column1 = new TableColumn() { Width = GridLength.Auto };

        //this._table.Columns.Add(column1);
        //this._table.Columns.Add(column2);
        //this._table.Columns.Add(column3);

        //this._richTextBox.Document.Blocks.Add(this._table);

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

    //public void Write(string xamlParagraphText)
    //{
    //    Paragraph parsedParagraph;

    //    try
    //    {
    //        parsedParagraph = (Paragraph)XamlReader.Parse(xamlParagraphText);

    //    }
    //    catch (XamlParseException ex)
    //    {

    //        SelfLog.WriteLine($"Error parsing `{xamlParagraphText}` to XAML: {ex.Message}");
    //        throw;
    //    }

    //    var inLines = parsedParagraph.Inlines.ToList();

    //    var flowDocument = this._richTextBox.Document ??= new FlowDocument();

    //    if (flowDocument.Blocks.LastBlock is not Paragraph paragraph)
    //    {
    //        paragraph = new Paragraph();
    //        flowDocument.Blocks.Add(paragraph);
    //    }

    //    paragraph.Inlines.AddRange(inLines);
    //}

    public bool CheckAccess()
    {
        return this._richTextBox.CheckAccess();
    }

    //public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg)
    //{
    //    return this._richTextBox.Dispatcher.BeginInvoke(priority, method, arg);
    //}

    public async ValueTask BeginInvoke(DispatcherPriority priority, string? xamlParagraphText)
    {
        try
        {
            //Paragraph parsedParagraph;

            //var parsedParagraph = (Paragraph)XamlReader.Parse(xamlParagraphText);

            //var inLines = parsedParagraph.Inlines.ToList();
            //;
            //var _0 = inLines[0];
            //var _1 = inLines[1];
            //var _2 = inLines[2];
            //var _3 = inLines[3];
            //var _4 = inLines[4];
            //var _5 = inLines[5];
            //var _6 = inLines[6];
            //var _7 = inLines[7];

            //inLines.RemoveRange(0, 5);

            //var parsedParagraph2 = new Paragraph() {Margin = new Thickness(0.0), Foreground = System.Windows.Media.Brushes.LawnGreen };
            //parsedParagraph2.Inlines.AddRange(inLines);

            object?[] delegateArray = new object[3];
            
            delegateArray[0] = this._richTextBox;
            delegateArray[1] = xamlParagraphText;
            //delegateArray[2] = this._table;
            //delegateArray[3] = this._tableRowGroup;
            delegateArray[2] = this._syncRoot;

            await this._richTextBox.Dispatcher.BeginInvoke(
                new RichTextBoxAppendTextDelegate(RichTextBoxAppendTextDelegateMethod), priority, delegateArray);

            //await result;
        }
        catch (Exception )
        {
            ;
        }
        //await this._richTextBox?.Dispatcher.BeginInvoke(p, riority, method, arg);
    }

    private static void RichTextBoxAppendTextDelegateMethod(
        System.Windows.Controls.RichTextBox wpfRichTextBox,
        string xamlParagraphText, /*Table table, TableRowGroup tableRowGroup,*/ object syncRoot)
    {
        try
        {
            //var inline1 = new Run(cell1);
            //var inline2 = new Run(cell2);
            //var inline3 = new Run(cell3);
            lock (syncRoot)
            {


                var parsedParagraph = (Paragraph)XamlReader.Parse(xamlParagraphText);

                var inLines = parsedParagraph.Inlines.ToList();

                var flowDocument = wpfRichTextBox.Document ??= new FlowDocument();

                if (flowDocument.Blocks.LastBlock is not Paragraph paragraph)
                {
                    paragraph = new Paragraph();
                    flowDocument.Blocks.Add(paragraph);
                }

                paragraph.Inlines.AddRange(inLines);
            }

            //var paragraph1 = new Paragraph(cell1) {Margin = new Thickness(0.0), Foreground = color};

            //var paragraph2 = new Paragraph(cell2) {Margin = new Thickness(0.0), Foreground = color};

            //var paragraph3 = new Paragraph(cell3) {Margin = new Thickness(0.0), Foreground = color};

            //var tableRow = new TableRow();
            //var tableCell1 = new TableCell() { };
            //var tableCell2 = new TableCell() { };
            //var tableCell3 = new TableCell() { };

            //tableCell1.Blocks.Add(parsedParagraph);
            //tableCell2.Blocks.Add(paragraph2);
            //tableCell3.Blocks.Add(cell3);



            //tableRow.Cells.Add(tableCell1);
            //tableRow.Cells.Add(tableCell2);
            //tableRow.Cells.Add(tableCell3);

            //tableRowGroup.Rows.Add(tableRow);

            //lock (syncRoot)
            //{
            //    wpfRichTextBox.Document.Blocks.Add(table);
            //}
        }
        catch (Exception )
        {
            //var m = ex.Message;
            ;
        }
    }

    public void OnCompleted()
    {

    }

    public void OnError(Exception error)
    {

    }

    public async void OnNext(LogEvent value)
    {
        StringBuilder sb = new();

        try
        {
            sb.Append($"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");
            StringWriter writer = new();
            this._formatter.Format(value, writer);
            sb.Append(writer);

            sb.Append("</Paragraph>");
            var xamlParagraphText = sb.ToString();

            await this.BeginInvoke(this._dispatcherPriority, xamlParagraphText).ConfigureAwait(false);
        }
        catch (Exception)
        {
            ;
        }
        finally
        {
            sb.Clear();
        }
    }
}
