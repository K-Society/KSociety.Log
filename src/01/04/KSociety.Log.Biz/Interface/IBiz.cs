// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Biz.Interface
{
    using KSociety.Log.Biz.Event;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBiz
    {
        void LoadEventBus();
        bool WriteLog(WriteLogEvent logEvent);
        bool WriteLogs(IEnumerable<WriteLogEvent> logEvents);
        ValueTask<bool> WriteLogAsync(WriteLogEvent logEvent);
        ValueTask<bool> WriteLogsAsync(IEnumerable<WriteLogEvent> logEvents);
    }
}