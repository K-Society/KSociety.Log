namespace KSociety.Log.Biz.Class
{
    using Autofac;
    using KSociety.Log.Biz.Interface;
    using Microsoft.Extensions.Logging;

    public class Startup : IStartable
    {
        private readonly ILogger<Startup> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IBiz _biz;

        public Startup(
            ILoggerFactory loggerFactory,
            IBiz biz
        )
        {
            this._loggerFactory = loggerFactory;
            this._logger = this._loggerFactory.CreateLogger<Startup>();
            this._biz = biz;
            this._logger.LogInformation("KSociety.Log startup! ");
        }

        public void Start()
        {
            this._logger.LogTrace("KSociety.Log staring...");
            this._biz.LoadEventBus();
        }
    }
}