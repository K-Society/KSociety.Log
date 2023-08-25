namespace KSociety.Log.App.Dto.Req.Biz
{
    using System;
    using KSociety.Base.App.Shared;
    using ProtoBuf;

    [ProtoContract]
    public class WriteLog : IRequest
    {
        [ProtoMember(1)] public string Message { get; set; }

        [ProtoMember(2), CompatibilityLevel(CompatibilityLevel.Level200)]
        public DateTime TimeStamp { get; set; }

        [ProtoMember(3)] public int SequenceId { get; set; }

        [ProtoMember(4)] public int Level { get; set; }

        [ProtoMember(5)] public string LoggerName { get; set; }

        [ProtoMember(6)] public string RoutingKey { get; set; } = "log";

        //[ProtoMember(6)]
        //public string CallerClassName { get; set; }

        //[ProtoMember(7)]
        //public string CallerFilePath { get; set; }

        //[ProtoMember(8)]
        //public int CallerLineNumber { get; set; }


        public WriteLog() { }

        public WriteLog(
            string message,
            DateTime timeStamp,
            int sequenceId,
            int level,
            string loggerName,
            string routingKey = "log"
            //string callerClassName,
            //string callerFilePath,
            //int callerLineNumber
        )
        {
            this.Message = message;
            this.TimeStamp = timeStamp;
            this.SequenceId = sequenceId;
            this.Level = level;
            this.LoggerName = loggerName;
            this.RoutingKey = routingKey;
            //CallerClassName = callerClassName;
            //CallerFilePath = callerFilePath;
            //CallerLineNumber = callerLineNumber;
        }
    }
}