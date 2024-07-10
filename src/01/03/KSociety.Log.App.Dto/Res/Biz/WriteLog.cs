// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.App.Dto.Res.Biz
{
    using KSociety.Base.App.Shared;
    using ProtoBuf;

    [ProtoContract]
    public class WriteLog : IResponse
    {
        [ProtoMember(1)] public bool Result { get; set; }

        public WriteLog()
        {

        }

        public WriteLog(bool result)
        {
            this.Result = result;
        }
    }
}