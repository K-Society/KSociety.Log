using System;
using ProtoBuf;

namespace KSociety.Log.Srv.Dto
{
    [ProtoContract]
    public class LogEvent
    {
        [ProtoMember(1)]
        public string Message { get; set; }

        [ProtoMember(2), CompatibilityLevel(CompatibilityLevel.Level200)]
        public DateTime TimeStamp { get; set; }

        [ProtoMember(3)]
        public int SequenceId { get; set; }

        [ProtoMember(4)]
        public int Level { get; set; }

        [ProtoMember(5)]
        public string LoggerName { get; set; }

        //[ProtoMember(6)]
        //public string CallerClassName { get; set; }

        //[ProtoMember(7)]
        //public string CallerFilePath { get; set; }

        //[ProtoMember(8)]
        //public int CallerLineNumber { get; set; }


        public LogEvent()
           /* : base("log")*/ { }

        public LogEvent(
            string message,
            DateTime timeStamp,
            int sequenceId,
            int level,
            string loggerName
            //string callerClassName,
            //string callerFilePath,
            //int callerLineNumber
        )
            //: base("log")
        {
            Message = message;
            TimeStamp = timeStamp;
            SequenceId = sequenceId;
            Level = level;
            LoggerName = loggerName;
            //CallerClassName = callerClassName;
            //CallerFilePath = callerFilePath;
            //CallerLineNumber = callerLineNumber;
        }
    }
}
