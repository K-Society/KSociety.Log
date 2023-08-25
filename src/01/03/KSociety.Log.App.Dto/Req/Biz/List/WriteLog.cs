namespace KSociety.Log.App.Dto.Req.Biz.List
{
    using System.Collections.Generic;
    using KSociety.Base.App.Shared;
    using ProtoBuf;

    [ProtoContract]
    public class WriteLog : AppList<Biz.WriteLog>
    {
        public WriteLog() { }

        public WriteLog(List<Biz.WriteLog> logs)
        {
            this.List = logs;
        }
    }
}