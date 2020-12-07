using System;
using KSociety.Base.App.Shared;
using KSociety.Log.App.Dto.Req.Biz;
using KSociety.Log.Biz.IntegrationEvent.Event;

namespace KSociety.Log.Serilog.Sinks.RabbitMq.ProtoModel
{
    public static class Configuration
    {
        public static void ProtoBufConfiguration()
        {
            try
            {
                //IntegrationEvent
                ProtoBuf.Meta.RuntimeTypeModel.Default.Add(typeof(IntegrationLogEvent), true)
                    .AddSubType(6000, typeof(WriteLogEvent));

                ProtoBuf.Meta.RuntimeTypeModel.Default.Add(typeof(KbAppList<WriteLog>), true)
                    .AddSubType(6001, typeof(App.Dto.Req.Biz.List.WriteLog));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Configuration " + ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
