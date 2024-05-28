// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

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