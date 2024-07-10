// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

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