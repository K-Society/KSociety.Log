﻿// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RabbitMq.ProtoModel
{
    using System;
    using KSociety.Base.App.Shared;
    using KSociety.Log.App.Dto.Req.Biz;
    using KSociety.Log.Biz.Event;

    public static class Configuration
    {
        public static void ProtoBufConfiguration()
        {
            try
            {
                //IntegrationEvent
                ProtoBuf.Meta.RuntimeTypeModel.Default.Add(typeof(IntegrationLogEvent), true)
                    .AddSubType(6000, typeof(WriteLogEvent));

                ProtoBuf.Meta.RuntimeTypeModel.Default.Add(typeof(AppList<WriteLog>), true)
                    .AddSubType(6001, typeof(App.Dto.Req.Biz.List.WriteLog));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Configuration " + ex.Message + " " + ex.StackTrace);
            }
        }
    }
}