using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;
using Serilog.Debugging;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Shared.Sinks.RichTextBox.Abstraction
{
    public class RichTextBox : IRichTextBox
    {
        private readonly System.Windows.Controls.RichTextBox _richTextBox;

        public RichTextBox(System.Windows.Controls.RichTextBox richTextBox)
        {
            _richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
        }

        public void Write(string xamlParagraphText)
        {
            Paragraph parsedParagraph;

            try
            {
                //var parserContext = new ParserContext()
                //{

                //}
                var stream = new MemoryStream(Encoding.Unicode.GetBytes(xamlParagraphText));
                var xmlContext = new XmlParserContext(null, null, null, XmlSpace.Preserve, Encoding.Unicode);
                parsedParagraph = (Paragraph) XamlReader.Load(stream, new ParserContext(xmlContext), true);
                //parsedParagraph = (Paragraph) XamlReader.Parse(xamlParagraphText);

            }
            catch (XamlParseException ex)
            {
                Console.WriteLine(ex.Message);
                SelfLog.WriteLine($"Error parsing `{xamlParagraphText}` to XAML: {ex.Message}");
                throw;
            }
            catch (Exception exx)
            {
                Console.WriteLine(exx.Message);
                throw;
            }

            var inLines = parsedParagraph.Inlines.ToList();

            var flowDocument = _richTextBox.Document ??= new FlowDocument();

            if (flowDocument.Blocks.LastBlock is not Paragraph paragraph)
            {
                paragraph = new Paragraph();
                flowDocument.Blocks.Add(paragraph);
            }

            paragraph.Inlines.AddRange(inLines);
        }

        public bool CheckAccess()
        {
            return _richTextBox.CheckAccess();
        }

        public DispatcherOperation BeginInvoke(DispatcherPriority priority, Delegate method, object arg)
        {
            return _richTextBox.Dispatcher.BeginInvoke(priority, method, arg);
        }
    }
}
