using Autofac;
using KSociety.Base.Srv.Shared.Interface;
using KSociety.Log.App.Dto.Res.Biz;
using KSociety.Log.Srv.Contract.Biz;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;

namespace KSociety.Log.Srv.Behavior.Biz
{
    public class Biz : IBiz
    {
        private readonly ILoggerFactory _loggerFactory;
        private static ILogger<Biz> _logger;
        private readonly IComponentContext _componentContext;
        private readonly ICommandHandler _commandHandler;

        public Biz(
            ILoggerFactory loggerFactory,
            IComponentContext componentContext,
            ICommandHandler commandHandler
        )
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<Biz>();

            _componentContext = componentContext;
            _commandHandler = commandHandler;
        }

        public WriteLog WriteLog(KSociety.Log.App.Dto.Req.Biz.WriteLog request, CallContext context = default)
        {
            return _commandHandler.ExecuteWithResponse<KSociety.Log.App.Dto.Req.Biz.WriteLog, WriteLog>(_loggerFactory, _componentContext, request);
        }

        public WriteLog WriteLogs(KSociety.Log.App.Dto.Req.Biz.List.WriteLog request, CallContext context = default)
        {
            return _commandHandler.ExecuteListWithResponse<KSociety.Log.App.Dto.Req.Biz.WriteLog, KSociety.Log.App.Dto.Req.Biz.List.WriteLog, WriteLog>(_loggerFactory, _componentContext, request);
        }
    }
}
