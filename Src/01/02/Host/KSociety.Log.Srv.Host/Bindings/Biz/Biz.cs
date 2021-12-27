using System;
using Autofac;
using KSociety.Log.Biz.Interface;

namespace KSociety.Log.Srv.Host.Bindings.Biz;

public class Biz : Module
{
    private readonly bool _debugFlag;

    public Biz(bool debugFlag)
    {
        _debugFlag = debugFlag;

    }

    protected override void Load(ContainerBuilder builder)
    {
        try
        {
            builder.RegisterType<KSociety.Log.Biz.Class.Biz>().As<IBiz>().SingleInstance();
            builder.RegisterType<KSociety.Log.Biz.Class.Startup>().As<IStartable>().SingleInstance();
        }
        catch (Exception ex)
        {
            if (_debugFlag)
            {
                Console.WriteLine("Transaction: " + ex.Message + " - " + ex.StackTrace);
            }
        }
    }
}