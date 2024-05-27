// Copyright © Mipot S.p.A. and contributors. All rights reserved. Licensed under the Mipot S.p.A. License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.EventBus
{
    using KSociety.Base.EventBus;

    public class QueueLogDeclareParameters : QueueDeclareParameters, IQueueLogDeclareParameters
    {
        public QueueLogDeclareParameters()
        {

        }

        public QueueLogDeclareParameters(bool queueDurable, bool queueExclusive, bool queueAutoDelete)
            : base(queueDurable, queueExclusive, queueAutoDelete)
        {

        }
    }
}
