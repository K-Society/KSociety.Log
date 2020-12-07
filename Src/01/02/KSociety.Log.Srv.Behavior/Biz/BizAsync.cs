using System.Threading.Tasks;
using Autofac;
using KSociety.Base.Srv.Shared.Interface;
using KSociety.Log.App.Dto.Res.Biz;
using KSociety.Log.Srv.Contract.Biz;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;

namespace KSociety.Log.Srv.Behavior.Biz
{
    public class BizAsync : IBizAsync
    {
        private readonly ILoggerFactory _loggerFactory;
        private static ILogger<BizAsync> _logger;
        private readonly IComponentContext _componentContext;
        private readonly ICommandHandlerAsync _commandHandlerAsync;

        public BizAsync(
            ILoggerFactory loggerFactory,
            IComponentContext componentContext,
            ICommandHandlerAsync commandHandlerAsync
        )
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<BizAsync>();

            _componentContext = componentContext;
            _commandHandlerAsync = commandHandlerAsync;
        }

        public ValueTask<WriteLog> WriteLogAsync(KSociety.Log.App.Dto.Req.Biz.WriteLog request, CallContext context = default)
        {
            return _commandHandlerAsync.ExecuteWithResponseAsync<KSociety.Log.App.Dto.Req.Biz.WriteLog, WriteLog>(_loggerFactory, _componentContext, request);
        }

        public ValueTask<WriteLog> WriteLogsAsync(KSociety.Log.App.Dto.Req.Biz.List.WriteLog request, CallContext context = default)
        {
            return _commandHandlerAsync.ExecuteListWithResponseAsync<KSociety.Log.App.Dto.Req.Biz.WriteLog, KSociety.Log.App.Dto.Req.Biz.List.WriteLog, WriteLog>(_loggerFactory, _componentContext, request);
        }
    }
}
