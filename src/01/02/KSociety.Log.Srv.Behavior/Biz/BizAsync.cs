namespace KSociety.Log.Srv.Behavior.Biz
{
    using System.Threading.Tasks;
    using Autofac;
    using KSociety.Base.Srv.Shared.Interface;
    using KSociety.Log.App.Dto.Res.Biz;
    using KSociety.Log.Srv.Contract.Biz;
    using Microsoft.Extensions.Logging;
    using ProtoBuf.Grpc;

    public class BizAsync : IBizAsync
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IComponentContext _componentContext;
        private readonly ICommandHandlerAsync _commandHandlerAsync;

        public BizAsync(
            ILoggerFactory loggerFactory,
            IComponentContext componentContext,
            ICommandHandlerAsync commandHandlerAsync
        )
        {
            this._loggerFactory = loggerFactory;

            this._componentContext = componentContext;
            this._commandHandlerAsync = commandHandlerAsync;
        }

        public async ValueTask<WriteLog> WriteLogAsync(KSociety.Log.App.Dto.Req.Biz.WriteLog request,
            CallContext context = default)
        {
            return await this._commandHandlerAsync
                .ExecuteWithResponseAsync<KSociety.Log.App.Dto.Req.Biz.WriteLog, WriteLog>(this._loggerFactory,
                    this._componentContext, request, context.CancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<WriteLog> WriteLogsAsync(KSociety.Log.App.Dto.Req.Biz.List.WriteLog request,
            CallContext context = default)
        {
            return await this._commandHandlerAsync
                .ExecuteListWithResponseAsync<KSociety.Log.App.Dto.Req.Biz.WriteLog,
                    KSociety.Log.App.Dto.Req.Biz.List.WriteLog, WriteLog>(this._loggerFactory, this._componentContext, request,
                    context.CancellationToken).ConfigureAwait(false);
        }
    }
}