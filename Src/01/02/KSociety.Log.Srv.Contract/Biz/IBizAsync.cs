using System.Threading.Tasks;
using KSociety.Log.App.Dto.Res.Biz;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace KSociety.Log.Srv.Contract.Biz
{
    [Service]
    public interface IBizAsync
    {
        [Operation]
        ValueTask<WriteLog> WriteLogAsync(KSociety.Log.App.Dto.Req.Biz.WriteLog request, CallContext context = default);

        [Operation]
        ValueTask<WriteLog> WriteLogsAsync(KSociety.Log.App.Dto.Req.Biz.List.WriteLog events,
            CallContext context = default);
    }
}