// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace KSociety.Log.Srv.Host.Bindings.Biz
{
    using System;
    using Autofac;
    using KSociety.Log.Biz.Interface;

    public class Biz : Module
    {
        private readonly bool _debugFlag;

        public Biz(bool debugFlag)
        {
            this._debugFlag = debugFlag;

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
                if (this._debugFlag)
                {
                    Console.WriteLine("Transaction: " + ex.Message + " - " + ex.StackTrace);
                }
            }
        }
    }
}