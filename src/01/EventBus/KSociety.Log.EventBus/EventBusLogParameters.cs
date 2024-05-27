// Copyright © Mipot S.p.A. and contributors. All rights reserved. Licensed under the Mipot S.p.A. License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.EventBus
{
    using KSociety.Base.EventBus;

    public class EventBusLogParameters : EventBusParameters, IEventBusLogParameters
    {
        public EventBusLogParameters()
        {
        }

        public EventBusLogParameters(IExchangeLogDeclareParameters exchangeDeclareParameters,
            IQueueLogDeclareParameters queueDeclareParameters, bool debug = false)
            : base(exchangeDeclareParameters, queueDeclareParameters, debug)
        {

        }
    }
}
