// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.XUnit.Sinks.XUnit
{
    using Xunit.Abstractions;

    internal class DiagnosticMessage : IMessageSinkMessage
    {
        internal string Message { get; }

        internal DiagnosticMessage(string message)
        {
            this.Message = message;
        }
    }
}
