namespace KSociety.Log.Serilog.Sinks.Test.WpfApp.Bindings
{
    using Autofac;
    using global::Serilog.Extensions.Logging;
    using KSociety.Log.Serilog.Sinks.RichTextBoxQueue.Wpf.Sinks.RichTextBoxQueue;
    using Microsoft.Extensions.Logging;

    public class Log : Module
    {
        private readonly RichTextBoxQueueSink _richTextBoxQueueSink;

        public Log(RichTextBoxQueueSink richTextBoxQueueSink)
        {
            this._richTextBoxQueueSink = richTextBoxQueueSink;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new LoggerFactory(new ILoggerProvider[] {new SerilogLoggerProvider()}))
                .As<ILoggerFactory>();

            builder.RegisterInstance(this._richTextBoxQueueSink).As<IRichTextBoxQueueSink>();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>));

            
        }
    }
}
