using KSociety.Base.EventBus.Handlers;
using KSociety.Log.Biz.Event;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KSociety.Log.Biz.IntegrationEvent.EventHandling
{
    public class LogEventHandler : IntegrationEventHandler<WriteLogEvent>
    {
        private readonly ILoggerFactory _loggerFactory;

        public LogEventHandler(
            ILoggerFactory loggerFactory
        ) : base ( loggerFactory )
        {
            _loggerFactory = loggerFactory;
        }

        public override async ValueTask Handle(WriteLogEvent @event, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                try
                {
                    var logger = _loggerFactory.CreateLogger(@event.LoggerName);
                    logger.Log((LogLevel)@event.Level, @event.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("LogEventHandler: " + ex.Message + " - " + ex.StackTrace);
                }

            }, cancellationToken).ConfigureAwait(false);
        }
    }
}