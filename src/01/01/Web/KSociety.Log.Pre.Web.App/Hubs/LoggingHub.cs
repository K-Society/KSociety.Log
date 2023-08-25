namespace KSociety.Log.Pre.Web.App.Hubs
{
    using System.Threading.Tasks;
    using KSociety.Log.Srv.Dto;
    using Microsoft.AspNetCore.SignalR;

    public class LoggingHub : Hub<ILoggingHub>
    {
        [HubMethodName("SendLog")]
        public async Task SendLog(LogEvent logEvent)
        {
            await this.Clients.Others.ReceiveLog(logEvent).ConfigureAwait(false);
        }

        //[HubMethodName("SendLogMessage")]
        //public async Task SendLogMessage(string logMessage)
        //{
        //    await Clients.Others.ReceiveLogMessage(logMessage);
        //}
    }
}