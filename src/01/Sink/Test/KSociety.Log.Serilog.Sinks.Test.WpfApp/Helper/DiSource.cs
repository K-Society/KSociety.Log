namespace KSociety.Log.Serilog.Sinks.Test.WpfApp.Helper;

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
