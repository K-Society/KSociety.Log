// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Biz.Event
{
    using ProtoBuf;

    [ProtoContract]
    public class IntegrationLogEvent : KSociety.Base.EventBus.Events.IntegrationEvent
    {
        public IntegrationLogEvent()
        {

        }

        public IntegrationLogEvent(string routingKey)
            : base(routingKey)
        {

        }
    }
}