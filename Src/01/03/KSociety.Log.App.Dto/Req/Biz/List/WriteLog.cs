using System.Collections.Generic;
using KSociety.Base.App.Shared;
using ProtoBuf;

namespace KSociety.Log.App.Dto.Req.Biz.List;

[ProtoContract]
public class WriteLog : AppList<Biz.WriteLog>
{
    public WriteLog(){}

    public WriteLog(List<Biz.WriteLog> logs)
    {
        List = logs;
    }
}