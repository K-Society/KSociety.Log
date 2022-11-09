using System;
using WixSharp;
using WixSharp.Bootstrapper;

namespace KSociety.Log.Install
{
    internal static class Setup
    {
        private const string Product = "Log";
        private const string Manufacturer = "K-Society";
        private static string _logSystemVersion = "1.0.0.0";

        public static void Main()
        {
            //Compiler.WixLocation =  Environment.ExpandEnvironmentVariables(@"%HOMEPATH%\.nuget\packages\wixsharp.wix.bin\3.11.2\tools\bin");
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            _logSystemVersion = fileVersionInfo.FileVersion;

            var productMsiUninstall = BuildMsiUninstall();
            var productMsiLogPresenter6 = BuildMsiLogPresenter("net6.0");
            var productMsiLogServer6 = BuildMsiLogServer("net6.0");
            var productMsiLogPresenter7 = BuildMsiLogPresenter("net7.0");
            var productMsiLogServer7 = BuildMsiLogServer("net7.0");
            var productMsiRegistryX86 = BuildMsiRegistryX86();
            var productMsiRegistryX64 = BuildMsiRegistryX64();

            var bootstrapper6 =
                new Bundle(Product,
                        new MsiPackage(productMsiUninstall)
                        {
                            DisplayInternalUI = false,
                            Compressed = true,
                            Visible = false,
                            Cache = false,
                            MsiProperties = "UNINSTALLER_PATH=[UNINSTALLER_PATH]"
                        },
                        new MsiPackage(productMsiLogPresenter6)
                        {
                            DisplayInternalUI = false,
                            Compressed = true,
                            Visible = false
                        },
                        new MsiPackage(productMsiLogServer6)
                        {
                            DisplayInternalUI = false,
                            Compressed = true,
                            Visible = false
                        },
                        new MsiPackage(productMsiRegistryX86) { DisplayInternalUI = false, Compressed = true, InstallCondition = "NOT VersionNT64" },
                        new MsiPackage(productMsiRegistryX64) { DisplayInternalUI = false, Compressed = true, InstallCondition = "VersionNT64" }
                    )
                    {
                        UpgradeCode = new Guid("97C9A7F8-AFBB-4634-AD96-91EBC5F07419"),
                        Version = new Version(_logSystemVersion),
                        Manufacturer = Manufacturer,
                        AboutUrl = "https://github.com/K-Society/KSociety.Log",
                        Variables = new[]
                        {
                            new Variable("UNINSTALLER_PATH",
                                $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\{"Package Cache"}\{"[WixBundleProviderKey]"}\{Manufacturer + "." + Product}6.exe")
                        }

                    };

            bootstrapper6.Build(Manufacturer + "." + Product + "6.exe");

            //

            var bootstrapper7 =
                new Bundle(Product,
                        new MsiPackage(productMsiUninstall)
                        {
                            DisplayInternalUI = false,
                            Compressed = true,
                            Visible = false,
                            Cache = false,
                            MsiProperties = "UNINSTALLER_PATH=[UNINSTALLER_PATH]"
                        },
                        new MsiPackage(productMsiLogPresenter7)
                        {
                            DisplayInternalUI = false,
                            Compressed = true,
                            Visible = false
                        },
                        new MsiPackage(productMsiLogServer7)
                        {
                            DisplayInternalUI = false,
                            Compressed = true,
                            Visible = false
                        },
                        new MsiPackage(productMsiRegistryX86) { DisplayInternalUI = false, Compressed = true, InstallCondition = "NOT VersionNT64" },
                        new MsiPackage(productMsiRegistryX64) { DisplayInternalUI = false, Compressed = true, InstallCondition = "VersionNT64" }
                    )
                {
                    UpgradeCode = new Guid("97C9A7F8-AFBB-4634-AD96-91EBC5F07419"),
                    Version = new Version(_logSystemVersion),
                    Manufacturer = Manufacturer,
                    AboutUrl = "https://github.com/K-Society/KSociety.Log",
                    Variables = new[]
                        {
                            new Variable("UNINSTALLER_PATH",
                                $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\{"Package Cache"}\{"[WixBundleProviderKey]"}\{Manufacturer + "." + Product}7.exe")
                        }

                };

            bootstrapper7.Build(Manufacturer + "." + Product + "7.exe");

            if (System.IO.File.Exists(productMsiUninstall))
            {
                System.IO.File.Delete(productMsiUninstall);
            }

            if (System.IO.File.Exists(productMsiLogPresenter6))
            {
                System.IO.File.Delete(productMsiLogPresenter6);
            }

            if (System.IO.File.Exists(productMsiLogServer6))
            {
                System.IO.File.Delete(productMsiLogServer6);
            }

            if (System.IO.File.Exists(productMsiLogPresenter7))
            {
                System.IO.File.Delete(productMsiLogPresenter7);
            }

            if (System.IO.File.Exists(productMsiLogServer7))
            {
                System.IO.File.Delete(productMsiLogServer7);
            }

            if (System.IO.File.Exists(productMsiRegistryX86)) { System.IO.File.Delete(productMsiRegistryX86); }
            if (System.IO.File.Exists(productMsiRegistryX64)) { System.IO.File.Delete(productMsiRegistryX64); }
        }

        private static string BuildMsiUninstall()
        {
            var project =
                new Project(Product + " Uninstall",
                    new Dir(new Id("PROGRAMMENUUNINSTALL"), @"%ProgramMenu%\" + Manufacturer + @"\" + Product,
                        new ExeFileShortcut("Uninstall " + Product, "[UNINSTALLER_PATH]", "")
                    )
                )
                {
                    InstallScope = InstallScope.perMachine,
                    
                    Version = new Version("1.0.0.0"),
                    GUID = new Guid("92CE0201-B483-441A-89D2-7831A250D070"),
                    UI = WUI.WixUI_ProgressOnly,
                    ControlPanelInfo = new ProductInfo
                    {
                        Manufacturer = Manufacturer
                    }
                };
            
            return project.BuildMsi();
        }

        private static string BuildMsiLogPresenter(string version)
        {
            Environment.SetEnvironmentVariable("LogPresenter",
                @"..\..\..\build\KSociety.Log.Pre.Web.App\Release\" + version + @"\publish");

            #region [Firewall]

            var serviceLogPresenterFw = new FirewallException
            {
                Id = "LOG_PRESENTER_ID",
                Description = "LogPresenter",
                Name = "LogPresenter",
                IgnoreFailure = true,
                Profile = FirewallExceptionProfile.all,
                Scope = FirewallExceptionScope.any
            };

            #endregion

            #region [Feature]

            Feature binaries = new Feature("Binaries", "Binaries", true, false);
            Feature logPresenter = new Feature("LogPresenter", "LogPresenter", true);

            binaries.Children.Add(logPresenter);

            #endregion

            File logPresenterFile;

            Project project = new Project("LogPresenter",
                new Dir(new Id("INSTALLDIR"), @"%ProgramFiles%\" + Manufacturer + @"\" + Product,
                    new Dir(new Id("DIAPRESENTERDIR"), logPresenter, "LogPresenter",
                        new Files(logPresenter, @"%LogPresenter%\*.*", f => !f.Contains("KSociety.Log.Pre.Web.App.exe")),
                        logPresenterFile = new File(logPresenter, @"%LogPresenter%\KSociety.Log.Pre.Web.App.exe", serviceLogPresenterFw)
                    ) // DIAPRESENTERDIR.
                ) // INSTALLDIR.
            )
            {
                InstallScope = InstallScope.perMachine,
                Platform = Platform.x64,
                Version = new Version("1.0.0.0"),
                GUID = new Guid("10962664-C32B-4045-BA9F-59CF5909A2FB"),
                UI = WUI.WixUI_ProgressOnly,
                ControlPanelInfo = new ProductInfo
                {
                    Manufacturer = Manufacturer
                }
            };

            logPresenterFile.ServiceInstaller = new ServiceInstaller
            {
                Id = "LOGPRESENTER",
                Name = "LogPresenter",
                Description = Manufacturer + " Log Presenter",
                StartOn = null,
                StopOn = SvcEvent.InstallUninstall_Wait,
                RemoveOn = SvcEvent.Uninstall_Wait,
                DelayedAutoStart = false,
                ServiceSid = ServiceSid.none,
                RestartServiceDelayInSeconds = 30,
                ResetPeriodInDays = 1,
                PreShutdownDelay = 1000 * 60 * 3,
                RebootMessage = "Failure actions do not specify reboot"
            };

            return project.BuildMsi();
        }

        private static string BuildMsiLogServer(string version)
        {
            Environment.SetEnvironmentVariable("LogServer",
                @"..\..\..\build\KSociety.Log.Srv.Host\Release\" + version + @"\publish");

            #region [Firewall]

            var serviceLogServerFw = new FirewallException
            {
                Id = "LOG_SERVER_ID",
                Description = "LogServer",
                Name = "LogServer",
                IgnoreFailure = true,
                Profile = FirewallExceptionProfile.all,
                Scope = FirewallExceptionScope.any
            };

            #endregion

            #region [Feature]

            Feature binaries = new Feature("Binaries", "Binaries", true, false);
            Feature logServer = new Feature("LogServer", "LogServer", true);

            binaries.Children.Add(logServer);

            #endregion

            File logService;

            //File service = new File();
            Project project = new Project("LogServer",
                new Dir(new Id("INSTALLDIR"), @"%ProgramFiles%\" + Manufacturer + @"\" + Product,
                    new Dir(new Id("LOGSERVERDIR"), logServer, "LogServer",
                        new Files(logServer, @"%LogServer%\*.*", f => !f.Contains("KSociety.Log.Srv.Host.exe")),
                        logService = new File(logServer, @"%LogServer%\KSociety.Log.Srv.Host.exe", serviceLogServerFw)
                    ) // LOGSERVER.
                ) // INSTALLDIR.
                //new Dir(new Id("PROGRAMMENU"),@"%ProgramMenu%\K-Society\Log System\LogServer",
                //    new ExeFileShortcut(new Id("LOGSERVERUNINSTALL"), logServer, "Uninstall Log Server", "[SystemFolder]msiexec.exe",
                //        "/x [ProductCode]")
                //) //Shortcut
            )
            {
                //InstallPrivileges = InstallPrivileges.elevated,
                InstallScope = InstallScope.perMachine,
                Platform = Platform.x64,
                Version = new Version("1.0.0.0"),
                GUID = new Guid("7AF94E29-7447-48B7-B4CC-3AD186B43C81"),
                UI = WUI.WixUI_ProgressOnly,
                ControlPanelInfo = new ProductInfo
                {
                    Manufacturer = Manufacturer
                }
                //Actions = new WixSharp.Action[]
                //{
                //    new ElevatedManagedAction(ServiceManager.InstallService, Return.check, When.After, Step.InstallFiles, Condition.NOT_Installed)
                //    {
                //        UsesProperties = "InstallState=[INSTALLSTATEDIR], ServiceName=[INSTALLDIR]Std.Srv.Host.Log.exe",
                //        ActionAssembly = typeof(ServiceManager).Assembly.Location
                //    },
                //    new ElevatedManagedAction(ServiceManager.UnInstallService, Return.check, When.Before, Step.RemoveFiles, Condition.BeingUninstalled)
                //    {
                //        UsesProperties = "InstallState=[INSTALLSTATEDIR], ServiceName=[INSTALLDIR]Std.Srv.Host.Log.exe",
                //        ActionAssembly = typeof(ServiceManager).Assembly.Location
                //    }
                //}
            };


            logService.ServiceInstaller = new ServiceInstaller
            {
                Id = "LOGSERVER",
                Name = "LogServer",
                Description = Manufacturer + " Log Server",
                StartOn = null,
                StopOn = SvcEvent.InstallUninstall_Wait,
                RemoveOn = SvcEvent.Uninstall_Wait,
                DelayedAutoStart = true,
                ServiceSid = ServiceSid.none,
                RestartServiceDelayInSeconds = 30,
                ResetPeriodInDays = 1,
                PreShutdownDelay = 1000 * 60 * 3,
                RebootMessage = "Failure actions do not specify reboot"
            };

            project.PreserveTempFiles = false;
            project.PreserveDbgFiles = false;

            return project.BuildMsi();
        }

        private static string BuildMsiRegistryX86()
        {
            Compiler.AutoGeneration.InstallDirDefaultId = null;
            var registry = new Feature("RegistryX86");
            var project =
                new Project(Product + " RegistryX86",
                    new RegKey(registry, RegistryHive.LocalMachine, @"SOFTWARE\" + Manufacturer + @"\" + Product,

                        new RegValue("Version", _logSystemVersion)),

                    new RemoveRegistryValue(registry, @"SOFTWARE\" + Manufacturer + @"\" + Product)
                )
                {

                    InstallScope = InstallScope.perMachine,

                    Version = new Version("1.0.0.0"),
                    GUID = new Guid("A6B0ABD7-7443-48CA-A419-304C5EAF27F4"),
                    UI = WUI.WixUI_ProgressOnly,
                    ControlPanelInfo = new ProductInfo
                    {
                        Manufacturer = Manufacturer
                    }
                };

            return project.BuildMsi();
        }// BuildMsiRegistryX86.

        private static string BuildMsiRegistryX64()
        {
            Compiler.AutoGeneration.InstallDirDefaultId = null;
            var registry = new Feature("RegistryX64");
            var project =
                new Project(Product + " RegistryX64",
                    new RegKey(registry, RegistryHive.LocalMachine, @"SOFTWARE\" + Manufacturer + @"\" + Product,

                        new RegValue("Version", _logSystemVersion))
                    {
                        Win64 = true
                    },

                    new RemoveRegistryValue(registry, @"SOFTWARE\" + Manufacturer + @"\" + Product)
                )
                {

                    InstallScope = InstallScope.perMachine,
                    Platform = Platform.x64,
                    Version = new Version("1.0.0.0"),
                    GUID = new Guid("9223CB10-3B93-4516-9567-CBC281E3AC34"),
                    UI = WUI.WixUI_ProgressOnly,
                    ControlPanelInfo = new ProductInfo
                    {
                        Manufacturer = Manufacturer
                    }
                };

            return project.BuildMsi();
        }// BuildMsiRegistryX64.
    }
}