[![Build status](https://ci.appveyor.com/api/projects/status/30w1cj6jnexc0mai?svg=true)](https://ci.appveyor.com/project/maniglia/ksociety-log)
![GitHub all releases](https://img.shields.io/github/downloads/K-Society/KSociety.Log/total)

# KSociety.Log - Log System

## Introduction

This is the README file for KSociety.Log.
KSociety.Log is a .NET 6.0 centralized log system, to work properly it needs [RabbitMQ](https://github.com/K-Society/KSociety.RabbitMQ.Install/releases).

## For Linux
Copy KSociety.Log.Srv.Host.service and KSociety.Log.Pre.Web.App.service files to /etc/systemd/system/ directory.

## Port numbers
- Log Server: 60500
- Log Presenter: 61000

### 0. Installer
The installer.

| GitHub Repository | NuGet | Download |
| ------------- | ------------- | ------------- |

### 1. Presentation
The presentation layer.

| GitHub Repository | NuGet | Download |
| ------------- | ------------- | ------------- |

### 1.1 Presentation - Web (KSociety.Log.Pre.Web.)
| GitHub Repository | NuGet | Download |
| ------------- | ------------- | ------------- |
| [App](https://github.com/K-Society/KSociety.Log/tree/master/Src/01/01/Web/KSociety.Log.Pre.Web.App) |  |  |

### 2. Service (KSociety.Log.Srv.)
The service layer.

| GitHub Repository | NuGet | Download |
| ------------- | ------------- | ------------- |
| [Agent](https://github.com/K-Society/KSociety.Log/tree/master/Src/01/02/KSociety.Log.Srv.Agent) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.Srv.Agent)](https://www.nuget.org/packages/KSociety.Log.Srv.Agent) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.Srv.Agent) |
| [Behavior](https://github.com/K-Society/KSociety.Log/tree/master/Src/01/02/KSociety.Log.Srv.Behavior) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.Srv.Behavior)](https://www.nuget.org/packages/KSociety.Log.Srv.Behavior) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.Srv.Behavior) |
| [Contract](https://github.com/K-Society/KSociety.Log/tree/master/Src/01/02/KSociety.Log.Srv.Contract) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.Srv.Contract)](https://www.nuget.org/packages/KSociety.Log.Srv.Contract) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.Srv.Contract) |
| [Dto](https://github.com/K-Society/KSociety.Log/tree/master/Src/01/02/KSociety.Log.Srv.Dto) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.Srv.Dto)](https://www.nuget.org/packages/KSociety.Log.Srv.Dto) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.Srv.Dto) |

### 2.1 Service - Host (KSociety.Log.Srv.Host)

| GitHub Repository | NuGet | Download |
| ------------- | ------------- | ------------- |
| [Host](https://github.com/K-Society/KSociety.Log/tree/master/Src/01/02/Host/KSociety.Log.Srv.Host) |  |  |

### 3. Application (KSociety.Log.App.)
The application layer.

| GitHub Repository | NuGet | Download |
| ------------- | ------------- | ------------- |
| [Dto](https://github.com/K-Society/KSociety.Log/tree/develop/Src/01/03/KSociety.Log.App.Dto) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.App.Dto)](https://www.nuget.org/packages/KSociety.Log.App.Dto) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.App.Dto) |
| [ReqHdlr](https://github.com/K-Society/KSociety.Log/tree/develop/Src/01/03/KSociety.Log.App.ReqHdlr) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.App.ReqHdlr)](https://www.nuget.org/packages/KSociety.Log.App.ReqHdlr) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.App.ReqHdlr) |

### 4. Business (KSociety.Log.Biz)
| GitHub Repository | NuGet | Download |
| ------------- | ------------- | ------------- |
| [Biz](https://github.com/K-Society/KSociety.Log/tree/develop/Src/01/04/KSociety.Log.Biz) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.Biz)](https://www.nuget.org/packages/KSociety.Log.Biz) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.Biz) |

### Sink (KSociety.Log.Serilog.Sinks.)
| GitHub Repository | NuGet | Download |
| ------------- | ------------- | ------------- |
| [Grpc](https://github.com/K-Society/KSociety.Log/tree/master/Src/01/Sink/KSociety.Log.Serilog.Sinks.Grpc) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.Serilog.Sinks.Grpc)](https://www.nuget.org/packages/KSociety.Log.Serilog.Sinks.Grpc) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.Serilog.Sinks.Grpc) |
| [RabbitMq](https://github.com/K-Society/KSociety.Log/tree/master/Src/01/Sink/KSociety.Log.Serilog.Sinks.RabbitMq) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.Serilog.Sinks.RabbitMq)](https://www.nuget.org/packages/KSociety.Log.Serilog.Sinks.RabbitMq) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.Serilog.Sinks.RabbitMq) |
| [SignalR](https://github.com/K-Society/KSociety.Log/tree/master/Src/01/Sink/KSociety.Log.Serilog.Sinks.SignalR) | [![NuGet](https://img.shields.io/nuget/v/KSociety.Log.Serilog.Sinks.SignalR)](https://www.nuget.org/packages/KSociety.Log.Serilog.Sinks.SignalR) | ![NuGet](https://img.shields.io/nuget/dt/KSociety.Log.Serilog.Sinks.SignalR) |

## License
The project is under Microsoft Reciprocal License [(MS-RL)](http://www.opensource.org/licenses/MS-RL)

## Dependencies

List of technologies, frameworks and libraries used for implementation:

- [.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0) (platform). Note for Visual Studio users: **VS 2022** is required.
- [Autofac](https://autofac.org/) (Inversion of Control Container)
- [protobuf-net](https://github.com/protobuf-net/protobuf-net) (protobuf-net is a contract based serializer for .NET code)
- [RabbitMQ.Client](https://www.rabbitmq.com/dotnet.html) (The RabbitMQ .NET client is an implementation of an AMQP 0-9-1 client library for C#)
- [Serilog](https://serilog.net/) (structured logging)
- [KSociety.Base.App.Shared](https://github.com/K-Society/KSociety.Base/tree/develop/Src/01/03/KSociety.Base.App.Shared) ()
- [KSociety.Base.EventBusRabbitMQ](https://github.com/K-Society/KSociety.Base/tree/master/Src/01/KSocietyBaseEventBus/KSociety.Base.EventBusRabbitMQ) ()
- [KSociety.Base.Srv.Agent](https://github.com/K-Society/KSociety.Base/tree/develop/Src/01/02/KSociety.Base.Srv.Agent) ()
- [KSociety.Base.Srv.Host.Shared](https://github.com/K-Society/KSociety.Base/tree/develop/Src/01/02/KSociety.Base.Srv.Shared) ()
