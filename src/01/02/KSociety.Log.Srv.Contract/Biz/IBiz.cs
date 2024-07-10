// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Srv.Contract.Biz
{
    using KSociety.Log.App.Dto.Res.Biz;
    using ProtoBuf.Grpc;
    using ProtoBuf.Grpc.Configuration;

    [Service]
    public interface IBiz
    {
        [Operation]
        WriteLog WriteLog(KSociety.Log.App.Dto.Req.Biz.WriteLog request, CallContext context = default);

        [Operation]
        WriteLog WriteLogs(KSociety.Log.App.Dto.Req.Biz.List.WriteLog request, CallContext context = default);
    }
}
