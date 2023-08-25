namespace KSociety.Log.App.ReqHdlr.Biz
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using KSociety.Base.App.Shared;
    using KSociety.Log.App.Dto.Req.Biz;
    using KSociety.Log.Biz.Event;
    using KSociety.Log.Biz.Interface;
    using Microsoft.Extensions.Logging;

    public class WriteLogReqHdlr :
        IRequestHandlerWithResponse<WriteLog, Dto.Res.Biz.WriteLog>,
        IRequestHandlerWithResponseAsync<WriteLog, Dto.Res.Biz.WriteLog>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<WriteLogReqHdlr> _logger;
        private readonly IBiz _biz;
        private readonly IMapper _mapper;

        public WriteLogReqHdlr(ILoggerFactory loggerFactory, IBiz biz, IMapper mapper)
        {
            this._loggerFactory = loggerFactory;
            this._logger = this._loggerFactory.CreateLogger<WriteLogReqHdlr>();
            this._biz = biz;
            this._mapper = mapper;
        }

        public Dto.Res.Biz.WriteLog Execute(WriteLog request)
        {
            var output = new Dto.Res.Biz.WriteLog(false);

            try
            {
                var message = this._mapper.Map<WriteLogEvent>(request);
                this._biz.WriteLog(message);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "WriteLogReqHdlr Execute: ");
            }

            return output;
        }

        public async ValueTask<Dto.Res.Biz.WriteLog> ExecuteAsync(WriteLog request,
            CancellationToken cancellationToken = default)
        {
            var output = new Dto.Res.Biz.WriteLog(false);

            try
            {
                var message = this._mapper.Map<WriteLogEvent>(request);
                await this._biz.WriteLogAsync(message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "WriteLogReqHdlr ExecuteAsync: ");
            }

            return output;
        }
    }
}