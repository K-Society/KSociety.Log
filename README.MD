[![Build status](https://ci.appveyor.com/api/projects/status/30w1cj6jnexc0mai?svg=true)](https://ci.appveyor.com/project/maniglia/ksociety-log)
![GitHub all releases](https://img.shields.io/github/downloads/K-Society/KSociety.Log/total)

# KSociety.Log - Log System

## Introduction

This is the README file for KSociety.Log.
KSociety.Log is a .NET 5.0 centralized log system, to work properly it needs [RabbitMQ](https://github.com/K-Society/KSociety.RabbitMQ.Install/releases).

## For Linux
Copy KSociety.Log.Srv.Host.service and KSociety.Log.Pre.Web.App.service files to /etc/systemd/system/ directory.

## Port numbers
- Log Server: 60500
- Log Presenter: 61000