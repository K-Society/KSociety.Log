﻿using KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Sinks.RichTextBox.Abstraction;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace KSociety.Log.Serilog.Sinks.RichTextBox.Wpf.Sinks.RichTextBox
{
    internal sealed class RichTextBoxSink : IBatchedLogEventSink, IDisposable
    {
        private readonly IRichTextBox _richTextBox;
        private readonly ITextFormatter _formatter;
        private readonly DispatcherPriority _dispatcherPriority;
        private readonly object _syncRoot;

        private readonly RenderAction _renderAction;
       // private const int _defaultWriteBufferCapacity = 256;

        //private const int _batchSize = 200;
        //private Thread _consumerThread;
        //private ConcurrentQueue<LogEvent> _messageQueue;

        public RichTextBoxSink(IRichTextBox richTextBox, ITextFormatter formatter, DispatcherPriority dispatcherPriority, object syncRoot)
        {
            _richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

            if (!Enum.IsDefined(typeof(DispatcherPriority), dispatcherPriority))
            {
                throw new InvalidEnumArgumentException(nameof(dispatcherPriority), (int)dispatcherPriority,
                    typeof(DispatcherPriority));
            }

            _dispatcherPriority = dispatcherPriority;
            _syncRoot = syncRoot ?? throw new ArgumentNullException(nameof(syncRoot));

            _renderAction = Render;

            //_messageQueue = new ConcurrentQueue<LogEvent>();

            //_consumerThread = new Thread(new ThreadStart(ProcessMessages)) { IsBackground = true };
            //_consumerThread.Start();
        }

        //private enum States
        //{
        //    Init,
        //    Dequeue,
        //    Log,
        //}

        //private void ProcessMessages()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    Stopwatch sw = Stopwatch.StartNew();
        //    States state = States.Init;
        //    int msgCounter = 0;

        //    while (true)
        //    {
        //        switch (state)
        //        {
        //            //prepare the string builder and data
        //            case States.Init:
        //                sb.Clear();
        //                sb.Append($"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");
        //                msgCounter = 0;
        //                state = States.Dequeue;
        //                break;

        //            case States.Dequeue:
        //                if (sw.Elapsed.TotalMilliseconds >= 25 || msgCounter >= _batchSize)
        //                {
        //                    if (msgCounter == 0)
        //                    {
        //                        //no messages, retick
        //                        sw.Restart();
        //                    }
        //                    else
        //                    {
        //                        //valid log condition
        //                        state = States.Log;
        //                        break;
        //                    }
        //                }

        //                if (_messageQueue.TryDequeue(out LogEvent logEvent) == false)
        //                {
        //                    Thread.Sleep(1);
        //                    continue;
        //                }

        //                StringWriter writer = new StringWriter();
        //                _formatter.Format(logEvent, writer);

        //                //got a message from the queue, retick
        //                sw.Restart();

        //                msgCounter++;
        //                sb.Append(writer.ToString());
        //                break;

        //            case States.Log:
        //                sb.Append("</Paragraph>");
        //                string xamlParagraphText = sb.ToString();
        //                _richTextBox.BeginInvoke(_dispatcherPriority, _renderAction, xamlParagraphText);
        //                state = States.Init;
        //                break;
        //        }
        //    }
        //}

        //public void Emit(LogEvent logEvent)
        //{            
        //    _messageQueue.Enqueue(logEvent);
        //}

        private void Render(string xamlParagraphText)
        {
            var richTextBox = _richTextBox;

            lock (_syncRoot)
            {
                richTextBox.Write(xamlParagraphText);
            }
        }

        public void Dispose()
        {
        }

        internal delegate void RenderAction(string xamlParagraphText);

        public Task EmitBatchAsync(IEnumerable<LogEvent> batch)
        {
            if (batch.Any())
            {
                StringBuilder sb = new();
                
                foreach (var logEvent in batch)
                {
                    sb.Append($"<Paragraph xmlns =\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xml:space=\"preserve\">");
                    StringWriter writer = new();
                    _formatter.Format(logEvent, writer);
                    sb.Append(writer);

                    sb.Append("</Paragraph>");
                    string xamlParagraphText = sb.ToString();
                    _richTextBox.BeginInvoke(_dispatcherPriority, _renderAction, xamlParagraphText);
                    sb.Clear();
                }
            }

            return Task.CompletedTask;
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }
    }
}
