namespace KSociety.Log.Biz.Event
{
    using ProtoBuf;

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