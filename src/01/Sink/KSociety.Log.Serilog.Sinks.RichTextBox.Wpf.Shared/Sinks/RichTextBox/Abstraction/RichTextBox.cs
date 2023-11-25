namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using global::Serilog.Debugging;

public class RichTextBox : IRichTextBox
{
    private readonly System.Windows.Controls.RichTextBox _richTextBox;
    private readonly object _syncRoot;

    private delegate void RichTextBoxAppendTextDelegate(System.Windows.Controls.RichTextBox wpfRichTextBox,
        string cell1, string cell2, string cell3, Table table, TableRowGroup tableRowGroup, Brush color);

    #region [Fields]

    private readonly Table _table = new();
    private readonly TableRowGroup _tableRowGroup = new();

    #endregion

    public RichTextBox(System.Windows.Controls.RichTextBox richTextBox, object syncRoot)
    {
        this._richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
        this._syncRoot = syncRoot;

        this._table.RowGroups.Add(this._tableRowGroup);

        var column1 = new TableColumn() { Width = new GridLength(140, GridUnitType.Pixel) };
        var column2 = new TableColumn() { Width = new GridLength(80, GridUnitType.Pixel) };
        var column3 = new TableColumn() { Width = GridLength.Auto };

        this._table.Columns.Add(column1);
        this._table.Columns.Add(column2);
        this._table.Columns.Add(column3);

        this._richTextBox.Document.Blocks.Add(this._table);

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

    //public void Write(string xamlParagraphText)
    //{
    //    Paragraph parsedParagraph;

    //    try
    //    {
    //        parsedParagraph = (Paragraph) XamlReader.Parse(xamlParagraphText);
    //    }
    //    catch (XamlParseException ex)
    //    {
    //        SelfLog.WriteLine($"Error parsing `{xamlParagraphText}` to XAML: {ex.Message}");
    //        return;
    //    }

    //    var inLines = parsedParagraph.Inlines.ToList();

    //    lock (this._syncRoot)
    //    {
    //        try
    //        {
    //            var flowDocument = this._richTextBox.Document ??= new FlowDocument();

    //            if (flowDocument.Blocks.LastBlock is not Paragraph paragraph)
    //            {
    //                paragraph = new Paragraph();
    //                flowDocument.Blocks.Add(paragraph);
    //            }

    //            paragraph.Inlines.AddRange(inLines);
    //        }
    //        catch (Exception)
    //        {
    //            ;
    //        }
    //    }
    //}

    public bool CheckAccess()
    {
        return this._richTextBox.CheckAccess();
    }
    
    public async ValueTask BeginInvoke(DispatcherPriority priority, /*Delegate method,*/ string arg)
    {
        var delegateArray = new object[7];

        delegateArray[0] = this._richTextBox;
        delegateArray[1] = "1";
        delegateArray[2] = "1";
        delegateArray[3] = arg;
        delegateArray[4] = this._table;
        delegateArray[5] = this._tableRowGroup;
        delegateArray[6] = System.Windows.Media.Brushes.LawnGreen;

        await this._richTextBox.Dispatcher.BeginInvoke(priority, new RichTextBoxAppendTextDelegate(RichTextBoxAppendTextDelegateMethod), delegateArray);
        //await this._richTextBox?.Dispatcher.BeginInvoke(p, riority, method, arg);
    }

    private static void RichTextBoxAppendTextDelegateMethod(
        System.Windows.Controls.RichTextBox wpfRichTextBox,
        string cell1, string cell2, string cell3, Table table, TableRowGroup tableRowGroup, Brush color)
    {
        var inline1 = new Run(cell1);
        var inline2 = new Run(cell2);
        var inline3 = new Run(cell3);

        var paragraph1 = new Paragraph(inline1)
        {
            Margin = new Thickness(0.0),
            Foreground = color
        };

        var paragraph2 = new Paragraph(inline2)
        {
            Margin = new Thickness(0.0),
            Foreground = color
        };

        var paragraph3 = new Paragraph(inline3)
        {
            Margin = new Thickness(0.0),
            Foreground = color
        };

        var tableRow = new TableRow();
        var tableCell1 = new TableCell() { };
        var tableCell2 = new TableCell() { };
        var tableCell3 = new TableCell() { };

        tableCell1.Blocks.Add(paragraph1);
        tableCell2.Blocks.Add(paragraph2);
        tableCell3.Blocks.Add(paragraph3);



        tableRow.Cells.Add(tableCell1);
        tableRow.Cells.Add(tableCell2);
        tableRow.Cells.Add(tableCell3);

        tableRowGroup.Rows.Add(tableRow);

        wpfRichTextBox.Document.Blocks.Add(table);
    }
}
