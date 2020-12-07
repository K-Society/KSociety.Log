@call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\vsdevcmd.bat" -no_ext -winsdk=none %*
@setlocal
@pushd %~dp0
@set _C=Release
@set _P=%~dp0build

msbuild -t:restore -p:Configuration=%_C% || exit /b

msbuild -p:Configuration=%_C% || exit /b

msbuild Src\01\01\Web\KSociety.Log.Pre.Web.App\KSociety.Log.Pre.Web.App.csproj -t:restore -p:Configuration=%_C% || exit /b

msbuild Src\01\01\Web\KSociety.Log.Pre.Web.App\KSociety.Log.Pre.Web.App.csproj -t:Publish -p:Configuration=%_C% -p:TargetFramework="net5.0" -p:OutputPath=%_P%\KSociety.Log.Pre.Web.App\%_C%\net5.0\ || exit /b

msbuild Src\01\02\Host\KSociety.Log.Srv.Host\KSociety.Log.Srv.Host.csproj -t:restore -p:Configuration=%_C% || exit /b

msbuild Src\01\02\Host\KSociety.Log.Srv.Host\KSociety.Log.Srv.Host.csproj -t:Publish -p:Configuration=%_C% -p:TargetFramework="net5.0" -p:OutputPath=%_P%\KSociety.Log.Srv.Host\%_C%\net5.0\ || exit /b

msbuild Src\00\KSociety.Log.Install\KSociety.Log.Install.csproj -t:restore -p:Configuration=%_C% || exit /b

msbuild Src\00\KSociety.Log.Install\KSociety.Log.Install.csproj -p:Configuration=%_C% -p:TargetFramework="net35" || exit /b

@popd
@endlocal