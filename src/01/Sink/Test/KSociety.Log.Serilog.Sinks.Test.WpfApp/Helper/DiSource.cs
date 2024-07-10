// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Serilog.Sinks.Test.WpfApp.Helper
{
    using System;

    public class DiSource
    {
        public static Func<Type?, object?, string?, object?>? Resolver { get; set; }

        public Type? Type { get; set; }
        public object? Key { get; set; }
        public string? Name { get; set; }

        public object? ProvideValue(IServiceProvider serviceProvider)
        {
            return Resolver?.Invoke(this.Type, this.Key, this.Name);
        }
    }
}
