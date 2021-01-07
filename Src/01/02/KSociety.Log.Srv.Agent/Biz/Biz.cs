using System;
using System.Threading;
using System.Threading.Tasks;
using KSociety.Log.App.Dto.Res.Biz;
using KSociety.Log.Srv.Contract.Biz;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;

namespace KSociety.Log.Srv.Agent.Biz
{
    public class Biz : Base.Srv.Agent.Connection
    {
        public Biz(Base.Srv.Agent.IAgentConfiguration agentConfiguration, ILoggerFactory loggerFactory)
            : base(agentConfiguration, loggerFactory)
        {

        }

        public WriteLog WriteLog(App.Dto.Req.Biz.WriteLog request, CancellationToken cancellationToken = default)
        {
            CallOptions = CallOptions.WithCancellationToken(cancellationToken);
            CallContext = new CallContext(CallOptions, CallContextFlags.IgnoreStreamTermination);
            WriteLog output = new WriteLog();
            try
            {
                using (Channel)
                {
                    IBiz client = Channel.CreateGrpcService<IBiz>();

                    var result = client.WriteLog(request, CallContext);

                    output = result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name + " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }
            return output;
        }

        public WriteLog WriteLogs(App.Dto.Req.Biz.List.WriteLog request, CancellationToken cancellationToken = default)
        {
            CallOptions = CallOptions.WithCancellationToken(cancellationToken);
            CallContext = new CallContext(CallOptions, CallContextFlags.IgnoreStreamTermination);
            WriteLog output = new WriteLog();
            try
            {
                using (Channel)
                {
                    IBiz client = Channel.CreateGrpcService<IBiz>();

                    var result = client.WriteLogs(request, CallContext);

                    output = result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name + " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }
            return output;
        }

        public async ValueTask<WriteLog> WriteLogAsync(App.Dto.Req.Biz.WriteLog request, CancellationToken cancellationToken = default)
        {
            CallOptions = CallOptions.WithCancellationToken(cancellationToken);
            CallContext = new CallContext(CallOptions, CallContextFlags.IgnoreStreamTermination);

            try
            {
                using (Channel)
                {
                    IBizAsync client = Channel.CreateGrpcService<IBizAsync>();

                    return await client.WriteLogAsync(request, CallContext);

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name + " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }

            return await new ValueTask<WriteLog>();
        }

        public async ValueTask<WriteLog> WriteLogsAsync(App.Dto.Req.Biz.List.WriteLog request, CancellationToken cancellationToken = default)
        {
            CallOptions = CallOptions.WithCancellationToken(cancellationToken);
            CallContext = new CallContext(CallOptions, CallContextFlags.IgnoreStreamTermination);

            try
            {
                using (Channel)
                {
                    IBizAsync client = Channel.CreateGrpcService<IBizAsync>();

                    return await client.WriteLogsAsync(request, CallContext);

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name + " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }

            return await new ValueTask<WriteLog>();
        }
    }
}
