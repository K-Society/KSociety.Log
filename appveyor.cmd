@setlocal
@pushd %~dp0
@set _C=Release
@set _P=%~dp0build

msbuild -t:restore -p:Configuration=%_C% || exit /b

msbuild -p:Configuration=%_C% || exit /b

msbuild Src\01\01\Web\KSociety.Log.Pre.Web.App\KSociety.Log.Pre.Web.App.csproj -t:restore -p:Configuration=%_C% || exit /b

msbuild Src\01\01\Web\KSociety.Log.Pre.Web.App\KSociety.Log.Pre.Web.App.csproj -t:Publish -p:Configuration=%_C% -p:DeleteExistingFiles=true -p:TargetFramework="net6.0" -p:OutputPath=%_P%\KSociety.Log.Pre.Web.App\%_C%\net6.0\ || exit /b

msbuild Src\01\01\Web\KSociety.Log.Pre.Web.App\KSociety.Log.Pre.Web.App.csproj -t:Publish -p:Configuration=%_C% -p:DeleteExistingFiles=true -p:TargetFramework="net7.0" -p:OutputPath=%_P%\KSociety.Log.Pre.Web.App\%_C%\net7.0\ || exit /b

msbuild Src\01\02\Host\KSociety.Log.Srv.Host\KSociety.Log.Srv.Host.csproj -t:restore -p:Configuration=%_C% || exit /b

msbuild Src\01\02\Host\KSociety.Log.Srv.Host\KSociety.Log.Srv.Host.csproj -t:Publish -p:Configuration=%_C% -p:DeleteExistingFiles=true -p:TargetFramework="net6.0" -p:OutputPath=%_P%\KSociety.Log.Srv.Host\%_C%\net6.0\ || exit /b

msbuild Src\01\02\Host\KSociety.Log.Srv.Host\KSociety.Log.Srv.Host.csproj -t:Publish -p:Configuration=%_C% -p:DeleteExistingFiles=true -p:TargetFramework="net7.0" -p:OutputPath=%_P%\KSociety.Log.Srv.Host\%_C%\net7.0\ || exit /b

REM msbuild Src\00\KSociety.Log.Install\KSociety.Log.Install.csproj -t:restore -p:Configuration=%_C% || exit /b

REM msbuild Src\00\KSociety.Log.Install\KSociety.Log.Install.csproj -p:Configuration=%_C% -p:TargetFramework="net461" || exit /b

@popd
@endlocal