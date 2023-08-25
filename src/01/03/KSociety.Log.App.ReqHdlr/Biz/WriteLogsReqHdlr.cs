namespace KSociety.Log.App.ReqHdlr.Biz
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using KSociety.Base.App.Shared;
    using KSociety.Log.App.Dto.Req.Biz;
    using KSociety.Log.Biz.Event;
    using KSociety.Log.Biz.Interface;
    using Microsoft.Extensions.Logging;

    public class WriteLogsReqHdlr :
        IRequestListHandlerWithResponse<WriteLog, Dto.Req.Biz.List.WriteLog, Dto.Res.Biz.WriteLog>,
        IRequestListHandlerWithResponseAsync<WriteLog, Dto.Req.Biz.List.WriteLog, Dto.Res.Biz.WriteLog>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<WriteLogsReqHdlr> _logger;
        private readonly IBiz _biz;
        private readonly IMapper _mapper;

        public WriteLogsReqHdlr(ILoggerFactory loggerFactory, IBiz biz, IMapper mapper)
        {
            this._loggerFactory = loggerFactory;
            this._logger = this._loggerFactory.CreateLogger<WriteLogsReqHdlr>();
            this._biz = biz;
            this._mapper = mapper;
        }

        public Dto.Res.Biz.WriteLog Execute(Dto.Req.Biz.List.WriteLog request)
        {
            var output = new Dto.Res.Biz.WriteLog(false);

            try
            {
                var list = request.List.Select(message => this._mapper.Map<WriteLogEvent>(message)).ToList();
                this._biz.WriteLogs(list);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "WriteLogsReqHdlr Execute: ");
            }

            return output;
        }

        public async ValueTask<Dto.Res.Biz.WriteLog> ExecuteAsync(Dto.Req.Biz.List.WriteLog request,
            CancellationToken cancellationToken = default)
        {
            var output = new Dto.Res.Biz.WriteLog(false);

            try
            {
                var list = request.List.Select(message => this._mapper.Map<WriteLogEvent>(message)).ToList();
                await this._biz.WriteLogsAsync(list).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "WriteLogsReqHdlr ExecuteAsync: ");
            }

            return output;
        }
    }
}