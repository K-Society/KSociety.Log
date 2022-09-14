using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KSociety.Base.App.Shared;
using KSociety.Log.App.Dto.Req.Biz;
using KSociety.Log.Biz.IntegrationEvent.Event;
using KSociety.Log.Biz.Interface;
using Microsoft.Extensions.Logging;

namespace KSociety.Log.App.ReqHdlr.Biz;

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
        _loggerFactory = loggerFactory;
        _logger = _loggerFactory.CreateLogger<WriteLogReqHdlr>();
        _biz = biz;
        _mapper = mapper;
    }

    public Dto.Res.Biz.WriteLog Execute(WriteLog request)
    {
        var output = new Dto.Res.Biz.WriteLog(false);

        try
        {
            var message = _mapper.Map<WriteLogEvent>(request);
            _biz.WriteLog(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WriteLogReqHdlr Execute: ");
        }

        return output;
    }

    public async ValueTask<Dto.Res.Biz.WriteLog> ExecuteAsync(WriteLog request, CancellationToken cancellationToken = default)
    {
        var output = new Dto.Res.Biz.WriteLog(false);

        try
        {
            var message = _mapper.Map<WriteLogEvent>(request);
            await _biz.WriteLogAsync(message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WriteLogReqHdlr ExecuteAsync: ");
        }

        return output;
    }
}