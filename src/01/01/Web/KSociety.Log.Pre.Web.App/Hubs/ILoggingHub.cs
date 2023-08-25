namespace KSociety.Log.Pre.Web.App.Hubs
{
    using System.Threading.Tasks;
    using KSociety.Log.Srv.Dto;

    public interface ILoggingHub
    {
        Task ReceiveLog(LogEvent logEvent);

        //Task ReceiveLogMessage(string logMessage);
    }
}