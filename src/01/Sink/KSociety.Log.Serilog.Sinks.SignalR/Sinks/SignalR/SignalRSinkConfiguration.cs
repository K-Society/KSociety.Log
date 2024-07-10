﻿// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.SignalR.Sinks.SignalR
{
    using System;
    using global::Serilog.Events;
    using global::Serilog.Formatting;
    using global::Serilog.Formatting.Compact;

    public class SignalRSinkConfiguration
    {
        public int BatchPostingLimit { get; set; }
        public TimeSpan Period { get; set; }
        public ITextFormatter TextFormatter { get; set; } = new CompactJsonFormatter();
        public LogEventLevel RestrictedToMinimumLevel { get; set; } = LogEventLevel.Verbose;
    }
}