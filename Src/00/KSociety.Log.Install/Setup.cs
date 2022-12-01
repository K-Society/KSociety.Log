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

            var productMsiUninstall6 = BuildMsiUninstall("net6.0");
            var productMsiUninstall7 = BuildMsiUninstall("net7.0");
            var productMsiLogPresenter6 = BuildMsiLogPresenter("net6.0");
            var productMsiLogServer6 = BuildMsiLogServer("net6.0");
            var productMsiLogPresenter7 = BuildMsiLogPresenter("net7.0");
            var productMsiLogServer7 = BuildMsiLogServer("net7.0");
            var productMsiRegistryX86_6 = BuildMsiRegistryX86("net6.0");
            var productMsiRegistryX64_6 = BuildMsiRegistryX64("net6.0");
            var productMsiRegistryX86_7 = BuildMsiRegistryX86("net7.0");
            var productMsiRegistryX64_7 = BuildMsiRegistryX64("net7.0");

            var bootstrapper6 =
                new Bundle(Product + @"-net6.0",
                        new MsiPackage(productMsiUninstall6)
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
                        new MsiPackage(productMsiRegistryX86_6) { DisplayInternalUI = false, Compressed = true, InstallCondition = "NOT VersionNT64" },
                        new MsiPackage(productMsiRegistryX64_6) { DisplayInternalUI = false, Compressed = true, InstallCondition = "VersionNT64" }
                    )
                    {
                        UpgradeCode = new Guid("97C9A7F8-AFBB-4634-AD96-91EBC5F07419"),
                        Version = new Version(_logSystemVersion),
                        Manufacturer = Manufacturer,
                        AboutUrl = "https://github.com/K-Society/KSociety.Log",
                        Variables = new[]
                        {
                            new Variable("UNINSTALLER_PATH",
                                $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\{"Package Cache"}\{"[WixBundleProviderKey]"}\{Manufacturer + "." + Product}-net6.0.exe")
                        }

                    };

            bootstrapper6.Build(Manufacturer + "." + Product + "-net6.0.exe");

            //

            var bootstrapper7 =
                new Bundle(Product + @"-net7.0",
                        new MsiPackage(productMsiUninstall7)
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
                        new MsiPackage(productMsiRegistryX86_7) { DisplayInternalUI = false, Compressed = true, InstallCondition = "NOT VersionNT64" },
                        new MsiPackage(productMsiRegistryX64_7) { DisplayInternalUI = false, Compressed = true, InstallCondition = "VersionNT64" }
                    )
                {
                    UpgradeCode = new Guid("ED601A62-1CA0-4AC1-A165-1627CCCC39F8"),
                    Version = new Version(_logSystemVersion),
                    Manufacturer = Manufacturer,
                    AboutUrl = "https://github.com/K-Society/KSociety.Log",
                    Variables = new[]
                        {
                            new Variable("UNINSTALLER_PATH",
                                $@"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\{"Package Cache"}\{"[WixBundleProviderKey]"}\{Manufacturer + "." + Product}-net7.0.exe")
                        }

                };

            bootstrapper7.Build(Manufacturer + "." + Product + "-net7.0.exe");

            if (System.IO.File.Exists(productMsiUninstall6))
            {
                System.IO.File.Delete(productMsiUninstall6);
            }

            if (System.IO.File.Exists(productMsiUninstall7))
            {
                System.IO.File.Delete(productMsiUninstall7);
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

            if (System.IO.File.Exists(productMsiRegistryX86_6)) { System.IO.File.Delete(productMsiRegistryX86_6); }
            if (System.IO.File.Exists(productMsiRegistryX64_6)) { System.IO.File.Delete(productMsiRegistryX64_6); }

            if (System.IO.File.Exists(productMsiRegistryX86_7)) { System.IO.File.Delete(productMsiRegistryX86_7); }
            if (System.IO.File.Exists(productMsiRegistryX64_7)) { System.IO.File.Delete(productMsiRegistryX64_7); }
        }

        private static string BuildMsiUninstall(string version)
        {
            var projectGuid = Guid.Empty;
            if (version.Equals("net5.0"))
            {
                projectGuid = new Guid("85C4ECA3-9383-467E-BD4D-9BC33989D51E");
            }
            else if (version.Equals("net6.0"))
            {
                projectGuid = new Guid("92CE0201-B483-441A-89D2-7831A250D070");
            }else if (version.Equals("net7.0"))
            {
                projectGuid = new Guid("88D42C2B-A1FA-4020-96A1-A4E521912F10");
            }
            var project =
                new Project(Product + " " + version + " Uninstall",
                    new Dir(new Id("PROGRAMMENUUNINSTALL"), @"%ProgramMenu%\" + Manufacturer + @"\" + Product + @"\" + version,
                        new ExeFileShortcut("Uninstall " + Product + " " + version, "[UNINSTALLER_PATH]", "")
                    )
                )
                {
                    InstallScope = InstallScope.perMachine,
                    
                    Version = new Version("1.0.0.0"),
                    GUID = projectGuid,
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
            var projectGuid = Guid.Empty;
            if (version.Equals("net5.0"))
            {
                projectGuid = new Guid("7FC0AF20-A36D-4149-A4CA-81AFDE099636");
            }
            else if (version.Equals("net6.0"))
            {
                projectGuid = new Guid("10962664-C32B-4045-BA9F-59CF5909A2FB");
            }
            else if (version.Equals("net7.0"))
            {
                projectGuid = new Guid("C313C734-1838-41E2-BD45-45E7779B3043");
            }

            Environment.SetEnvironmentVariable("LogPresenter",
                @"..\..\..\build\KSociety.Log.Pre.Web.App\Release\" + version + @"\publish");

            #region [Firewall]

            var serviceLogPresenterFw = new FirewallException
            {
                Id = "LOG_PRESENTER_ID",
                Description = "LogPresenter " + version,
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

            Project project = new Project("LogPresenter " + version,
                new Dir(new Id("INSTALLDIR"), @"%ProgramFiles%\" + Manufacturer + @"\" + Product + @"\" + version,
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
                GUID = projectGuid,
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
                Description = Manufacturer + " Log Presenter " + version,
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
            project.PreserveTempFiles = false;
            project.PreserveDbgFiles = false;
            return project.BuildMsi();
        }

        private static string BuildMsiLogServer(string version)
        {
            var projectGuid = Guid.Empty;
            if (version.Equals("net5.0"))
            {
                projectGuid = new Guid("BF0E98D3-C7EA-40FC-B48C-E02E2BB6B7C9");
            }
            else if (version.Equals("net6.0"))
            {
                projectGuid = new Guid("7AF94E29-7447-48B7-B4CC-3AD186B43C81");
            }
            else if (version.Equals("net7.0"))
            {
                projectGuid = new Guid("3547187C-1996-40B9-8DF5-53BF9093FEDF");
            }

            Environment.SetEnvironmentVariable("LogServer",
                @"..\..\..\build\KSociety.Log.Srv.Host\Release\" + version + @"\publish");

            #region [Firewall]

            var serviceLogServerFw = new FirewallException
            {
                Id = "LOG_SERVER_ID",
                Description = "LogServer " + version,
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
            Project project = new Project("LogServer " + version,
                new Dir(new Id("INSTALLDIR"), @"%ProgramFiles%\" + Manufacturer + @"\" + Product + @"\" + version,
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
                GUID = projectGuid,
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
                Description = Manufacturer + " Log Server " + version,
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

        private static string BuildMsiRegistryX86(string version)
        {
            var projectGuid = Guid.Empty;
            if (version.Equals("net5.0"))
            {
                projectGuid = new Guid("3603A315-AFE5-43BC-9AB2-C02097E98BD4");
            }
            else if (version.Equals("net6.0"))
            {
                projectGuid = new Guid("A6B0ABD7-7443-48CA-A419-304C5EAF27F4");
            }
            else if (version.Equals("net7.0"))
            {
                projectGuid = new Guid("B06D2D3D-64D7-4CE0-B868-C6299E9127A1");
            }

            Compiler.AutoGeneration.InstallDirDefaultId = null;
            var registry = new Feature("RegistryX86");
            var project =
                new Project(Product + " RegistryX86 " + version,
                    new RegKey(registry, RegistryHive.LocalMachine, @"SOFTWARE\" + Manufacturer + @"\" + Product + @"\" + version,

                        new RegValue("Version", _logSystemVersion)),

                    new RemoveRegistryValue(registry, @"SOFTWARE\" + Manufacturer + @"\" + Product + @"\" + version )
                )
                {

                    InstallScope = InstallScope.perMachine,

                    Version = new Version("1.0.0.0"),
                    GUID = projectGuid,
                    UI = WUI.WixUI_ProgressOnly,
                    ControlPanelInfo = new ProductInfo
                    {
                        Manufacturer = Manufacturer
                    }
                };

            return project.BuildMsi();
        }// BuildMsiRegistryX86.

        private static string BuildMsiRegistryX64(string version)
        {
            var projectGuid = Guid.Empty;
            if (version.Equals("net5.0"))
            {
                projectGuid = new Guid("9696B664-7C2B-4CF9-9DD1-D46FAA56D50C");
            }
            else if (version.Equals("net6.0"))
            {
                projectGuid = new Guid("A6B0ABD7-7443-48CA-A419-304C5EAF27F4");
            }
            else if (version.Equals("net7.0"))
            {
                projectGuid = new Guid("E35BBEFC-9345-4456-8137-96C2F057A61C");
            }
            Compiler.AutoGeneration.InstallDirDefaultId = null;
            var registry = new Feature("RegistryX64");
            var project =
                new Project(Product + " RegistryX64 " + version,
                    new RegKey(registry, RegistryHive.LocalMachine, @"SOFTWARE\" + Manufacturer + @"\" + Product + @"\" + version,

                        new RegValue("Version", _logSystemVersion))
                    {
                        Win64 = true
                    },

                    new RemoveRegistryValue(registry, @"SOFTWARE\" + Manufacturer + @"\" + Product + @"\" + version)
                )
                {

                    InstallScope = InstallScope.perMachine,
                    Platform = Platform.x64,
                    Version = new Version("1.0.0.0"),
                    GUID = projectGuid,
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