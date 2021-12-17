using KSociety.Log.App.Dto.Res.Biz;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace KSociety.Log.Srv.Contract.Biz;

[Service]
public interface IBiz
{
    [Operation]
    WriteLog WriteLog(KSociety.Log.App.Dto.Req.Biz.WriteLog request, CallContext context = default);

    [Operation]
    WriteLog WriteLogs(KSociety.Log.App.Dto.Req.Biz.List.WriteLog request, CallContext context = default);
}