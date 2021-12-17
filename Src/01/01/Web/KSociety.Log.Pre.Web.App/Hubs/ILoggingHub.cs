using System.Threading.Tasks;
using KSociety.Log.Srv.Dto;

namespace KSociety.Log.Pre.Web.App.Hubs;

public interface ILoggingHub
{
    Task ReceiveLog(LogEvent logEvent);

    //Task ReceiveLogMessage(string logMessage);
}