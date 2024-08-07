// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.EventBus
{
    using KSociety.Base.EventBus;

    public class ExchangeLogDeclareParameters : ExchangeDeclareParameters, IExchangeLogDeclareParameters
    {
        public ExchangeLogDeclareParameters()
            : base()
        {

        }

        public ExchangeLogDeclareParameters(string brokerName, ExchangeType exchangeType, bool exchangeDurable = false,
            bool exchangeAutoDelete = false)
            : base(brokerName, exchangeType, exchangeDurable, exchangeAutoDelete)
        {

        }
    }
}
