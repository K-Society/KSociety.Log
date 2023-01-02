[![Build status](https://ci.appveyor.com/api/projects/status/30w1cj6jnexc0mai?svg=true)](https://ci.appveyor.com/project/maniglia/ksociety-log)

[KSociety.Log Home](https://github.com/K-Society/KSociety.Log)

# KSociety.Log.Serilog.Sinks.RabbitMq

KSociety.Log.Serilog.Sinks.RabbitMq is a library that allows the transmission of logs to the [log server](https://github.com/K-Society/KSociety.Log) through RabbitMQ.

## Introduction

KSociety.Log.Serilog.Sinks.RabbitMq is a Nuget package that extends Serilog to support logging to RabbitMq in [K-Society](https://github.com/K-Society) ecosystem.

### Example of use
Refer to the following [example repository](https://github.com/K-Society/KSociety.Example).
Refer to the following [readme](https://github.com/K-Society/KSociety.Example/tree/master/docs/KSociety.Example.Pre.Console.Log.SinksRabbitMq).

### KSociety.Log.Serilog.Sinks.RabbitMq
Serilog sink to transmit log messages to RabbitMQ.

| GitHub Repository | NuGet | Download |
| ------------- | ------------- | ------------- |
| [KSociety.Log.Serilog.Sinks.RabbitMq](https://github.com/K-Society/KSociety.Log/tree/master/Src/01/Sink/KSociety.Log.Serilog.Sinks.RabbitMq) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.Serilog.Sinks.RabbitMq)](https://www.nuget.org/packages/KSociety.Log.Serilog.Sinks.RabbitMq) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.Serilog.Sinks.RabbitMq) |

## License
The project is under Microsoft Reciprocal License [(MS-RL)](http://www.opensource.org/licenses/MS-RL)

## Dependencies

List of technologies, frameworks and libraries used for implementation:

- [.NETSTANDARD 2.1](https://dotnet.microsoft.com/en-us/download/dotnet/2.1).
- [KSociety.Base.EventBusRabbitMQ](https://www.nuget.org/packages/KSociety.Base.EventBusRabbitMQ) 
- [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging) 
- [Serilog](https://www.nuget.org/packages/Serilog) 
- [Serilog.Formatting.Compact](https://www.nuget.org/packages/Serilog.Formatting.Compact) 
- [Serilog.Sinks.PeriodicBatching](https://www.nuget.org/packages/Serilog.Sinks.PeriodicBatching) 