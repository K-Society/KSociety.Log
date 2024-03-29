﻿namespace KSociety.Log.Biz.Interface
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