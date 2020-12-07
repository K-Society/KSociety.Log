using System;
using System.Threading;
using System.Threading.Tasks;
using KSociety.Base.EventBus.Abstractions.Handler;
using KSociety.Log.Biz.IntegrationEvent.Event;
using Microsoft.Extensions.Logging;

namespace KSociety.Log.Biz.IntegrationEvent.EventHandling
{
    public class LogEventHandler : IIntegrationEventHandler<WriteLogEvent>
    {
        private readonly ILoggerFactory _loggerFactory;
        private ILogger _logger;

        public LogEventHandler(
            ILoggerFactory loggerFactory
        )
        {
            _loggerFactory = loggerFactory;
        }
        public async ValueTask Handle(WriteLogEvent @event, CancellationToken cancellationToken = default)
        {

            await Task.Run(() =>
            {
                //var logger = LogManager.GetLogger();

                //var log = new LogEventInfo(LogLevel.FromOrdinal(@event.Level), @event.LoggerName, @event.Message)
                //{
                //    TimeStamp = @event.TimeStamp
                //};
                //logger.Log(log);
                try
                {

                    _logger = _loggerFactory.CreateLogger(@event.LoggerName);

                    _logger.Log((LogLevel)@event.Level, @event.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("LogEventHandler: " + ex.Message + " - " + ex.StackTrace);
                }

            }, cancellationToken).ConfigureAwait(false);
        }
    }
}
