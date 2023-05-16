using ProtoBuf;

namespace KSociety.Log.Biz.Event
{
    [ProtoContract]
    public class IntegrationLogEvent : KSociety.Base.EventBus.Events.IntegrationEvent
    {
        public IntegrationLogEvent()
        {

        }

        public IntegrationLogEvent(string routingKey)
            : base(routingKey)
        {

        }
    }
}