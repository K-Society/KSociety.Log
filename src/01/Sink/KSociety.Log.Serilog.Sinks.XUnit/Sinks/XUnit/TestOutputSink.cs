namespace KSociety.Log.Serilog.Sinks.XUnit.Sinks.XUnit
{
    using System;
    using System.IO;
    using global::Serilog.Core;
    using global::Serilog.Events;
    using global::Serilog.Formatting;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// A sink to direct Serilog output to the XUnit test output
    /// </summary>
    public class TestOutputSink : ILogEventSink
    {
        private readonly IMessageSink? _messageSink;
        private readonly ITestOutputHelper? _testOutputHelper;
        private readonly ITextFormatter _textFormatter;

        /// <summary>
        /// Creates a new instance of <see cref="TestOutputSink"/>
        /// </summary>
        /// <param name="messageSink">An <see cref="IMessageSink"/> implementation that can be used to provide test output</param>
        /// <param name="textFormatter">The <see cref="ITextFormatter"/> used when rendering the message</param>
        public TestOutputSink(IMessageSink? messageSink, ITextFormatter textFormatter)
        {
            this._messageSink = messageSink ?? throw new ArgumentNullException(nameof(messageSink));
            this._textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        /// <summary>
        /// Creates a new instance of <see cref="TestOutputSink"/>
        /// </summary>
        /// <param name="testOutputHelper">An <see cref="ITestOutputHelper"/> implementation that can be used to provide test output</param>
        /// <param name="textFormatter">The <see cref="ITextFormatter"/> used when rendering the message</param>
        public TestOutputSink(ITestOutputHelper? testOutputHelper, ITextFormatter textFormatter)
        {
            this._testOutputHelper = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
            this._textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        }

        /// <summary>
        /// Emits the provided log event from a sink 
        /// </summary>
        /// <param name="logEvent">The event being logged</param>
        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) {throw new ArgumentNullException(nameof(logEvent));}

            var renderSpace = new StringWriter();
            this._textFormatter.Format(logEvent, renderSpace);
            var message = renderSpace.ToString().Trim();
            this._messageSink?.OnMessage(new DiagnosticMessage(message));
            this._testOutputHelper?.WriteLine(message);
        }
    }
}
