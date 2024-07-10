// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Srv.Agent.Biz
{
    using KSociety.Log.App.Dto.Res.Biz;
    using KSociety.Log.Srv.Contract.Biz;
    using Microsoft.Extensions.Logging;
    using ProtoBuf.Grpc.Client;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

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
                using (this.Channel)
                {
                    IBiz client = this.Channel.CreateGrpcService<IBiz>();

                    var result = client.WriteLog(request, this.ConnectionOptions(cancellationToken));

                    output = result;
                }
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name +
                                " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }

            return output;
        }

        public WriteLog WriteLogs(App.Dto.Req.Biz.List.WriteLog request, CancellationToken cancellationToken = default)
        {
            WriteLog output = new WriteLog();
            try
            {
                using (this.Channel)
                {
                    IBiz client = this.Channel.CreateGrpcService<IBiz>();

                    var result = client.WriteLogs(request, this.ConnectionOptions(cancellationToken));

                    output = result;
                }
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name +
                                " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }

            return output;
        }

        public async ValueTask<WriteLog> WriteLogAsync(App.Dto.Req.Biz.WriteLog request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using (this.Channel)
                {
                    IBizAsync client = this.Channel.CreateGrpcService<IBizAsync>();

                    return await client.WriteLogAsync(request, this.ConnectionOptions(cancellationToken))
                        .ConfigureAwait(false);

                }
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name +
                                " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }

            return await new ValueTask<WriteLog>();
        }

        public async ValueTask<WriteLog> WriteLogsAsync(App.Dto.Req.Biz.List.WriteLog request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using (this.Channel)
                {
                    IBizAsync client = this.Channel.CreateGrpcService<IBizAsync>();

                    return await client.WriteLogsAsync(request, this.ConnectionOptions(cancellationToken))
                        .ConfigureAwait(false);

                }
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(this.GetType().FullName + "." + System.Reflection.MethodBase.GetCurrentMethod()?.Name +
                                " - " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }

            return await new ValueTask<WriteLog>();
        }
    }
}
