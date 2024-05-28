// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.App.Dto.Req.Biz.List
{
    using System.Collections.Generic;
    using KSociety.Base.App.Shared;
    using ProtoBuf;

    [ProtoContract]
    public class WriteLog : AppList<Biz.WriteLog>
    {
        public WriteLog() { }

        public WriteLog(List<Biz.WriteLog> logs)
        {
            this.List = logs;
        }
    }
}