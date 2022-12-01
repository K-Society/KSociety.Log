using KSociety.Log.App.Dto.Res.Biz;
using KSociety.Log.Srv.Contract.Biz;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            WriteLog output = new WriteLog();
            try
            {
                using (Channel)
                {
                    IBiz client = Channel.CreateGrpcService<IBiz>();

                    var result = client.WriteLog(request, ConnectionOptions(cancellationToken));

                    output = result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name +
                                " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }

            return output;
        }

        public WriteLog WriteLogs(App.Dto.Req.Biz.List.WriteLog request, CancellationToken cancellationToken = default)
        {
            WriteLog output = new WriteLog();
            try
            {
                using (Channel)
                {
                    IBiz client = Channel.CreateGrpcService<IBiz>();

                    var result = client.WriteLogs(request, ConnectionOptions(cancellationToken));

                    output = result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name +
                                " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }

            return output;
        }

        public async ValueTask<WriteLog> WriteLogAsync(App.Dto.Req.Biz.WriteLog request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using (Channel)
                {
                    IBizAsync client = Channel.CreateGrpcService<IBizAsync>();

                    return await client.WriteLogAsync(request, ConnectionOptions(cancellationToken))
                        .ConfigureAwait(false);

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name +
                                " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }

            return await new ValueTask<WriteLog>();
        }

        public async ValueTask<WriteLog> WriteLogsAsync(App.Dto.Req.Biz.List.WriteLog request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using (Channel)
                {
                    IBizAsync client = Channel.CreateGrpcService<IBizAsync>();

                    return await client.WriteLogsAsync(request, ConnectionOptions(cancellationToken))
                        .ConfigureAwait(false);

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name +
                                " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }

            return await new ValueTask<WriteLog>();
        }
    }
}