// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.App.ReqHdlr.Biz
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
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

        public WriteLogsReqHdlr(ILoggerFactory loggerFactory, IBiz biz)
        {
            this._loggerFactory = loggerFactory;
            this._logger = this._loggerFactory.CreateLogger<WriteLogsReqHdlr>();
            this._biz = biz;
        }

        public Dto.Res.Biz.WriteLog Execute(Dto.Req.Biz.List.WriteLog request)
        {
            var output = new Dto.Res.Biz.WriteLog(false);

            try
            {
                var list = request.List.Select(message => new WriteLogEvent(message.Message, message.TimeStamp, message.SequenceId, message.Level, message.LoggerName) /*this._mapper.Map<WriteLogEvent>(message)*/).ToList();
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
                var list = request.List.Select(message => new WriteLogEvent(message.Message, message.TimeStamp, message.SequenceId, message.Level, message.LoggerName)).ToList();
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
