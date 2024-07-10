﻿// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.RabbitMq.Sinks.RabbitMq
{
    using global::Serilog.Sinks.PeriodicBatching;

    public interface IRabbitMqBatchedSink : IBatchedLogEventSink
    {
        void Initialize();
    }
}