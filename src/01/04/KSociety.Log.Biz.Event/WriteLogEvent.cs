namespace KSociety.Log.Biz.Event
{
    using ProtoBuf;
    using System;

    [ProtoContract]
    public class WriteLogEvent : IntegrationLogEvent
    {
        [ProtoMember(1)] public string Message { get; set; }

        [ProtoMember(2), CompatibilityLevel(CompatibilityLevel.Level200)]
        public DateTime TimeStamp { get; set; }

        [ProtoMember(3)] public int SequenceId { get; set; }

        [ProtoMember(4)] public int Level { get; set; }

        [ProtoMember(5)] public string LoggerName { get; set; }

        public WriteLogEvent()
        {

        }

        public WriteLogEvent(
            string message,
            DateTime timeStamp,
            int sequenceId,
            int level,
            string loggerName,
            string routingKey = "log"
        )
            : base(routingKey)
        {
            this.Message = message;
            this.TimeStamp = timeStamp;
            this.SequenceId = sequenceId;
            this.Level = level;
            this.LoggerName = loggerName;
        }
    }
}