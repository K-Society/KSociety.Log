using System.Collections.Generic;
using System.Threading.Tasks;
using KSociety.Base.EventBusRabbitMQ;
using KSociety.Log.Biz.IntegrationEvent.Event;

namespace KSociety.Log.Biz.Interface
{
    public interface IBiz
    {
        IRabbitMqPersistentConnection PersistentConnection { get; }
        void LoadEventBus();
        bool WriteLog(WriteLogEvent logEvent);
        bool WriteLogs(IEnumerable<WriteLogEvent> logEvents);
        ValueTask<bool> WriteLogAsync(WriteLogEvent logEvent);
        ValueTask<bool> WriteLogsAsync(IEnumerable<WriteLogEvent> logEvents);
    }
}
