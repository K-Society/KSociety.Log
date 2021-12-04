# Based on http://nuts4.net/post/automated-download-and-installation-of-visual-studio-extensions-via-powershell
#https://github.com/wixtoolset/wix3/releases/download/wix3112rtm/wix311.exe

param([String] $PackageName)
 
$ErrorActionPreference = "Stop"
 
$baseProtocol = "https:"
$baseHostName = "github.com/wixtoolset/wix3/releases/download/wix3112rtm"
 
$Uri = "$($baseProtocol)//$($baseHostName)/$($PackageName)"
$ExeLocation = "$($env:Temp)\$([guid]::NewGuid()).exe"
 
#$VSInstallDir = "C:\Program Files (x86)\Microsoft Visual Studio\Installer\resources\app\ServiceHub\Services\Microsoft.VisualStudio.Setup.Service"
 
#if (-Not $VSInstallDir) {
#  Write-Error "Visual Studio InstallDir registry key missing"
#  Exit 1
#}
 
Write-Host "Grabbing EXE at $($Uri)"
$HTML = Invoke-WebRequest -Uri $Uri -OutFile $ExeLocation -UseBasicParsing -SessionVariable session
 
Write-Host "Attempting to download $($PackageName)..."
 
if (-Not (Test-Path $ExeLocation)) {
  Write-Error "Downloaded EXE file could not be located"
  Exit 1
}
#Write-Host "VSInstallDir is $($VSInstallDir)"
Write-Host "ExeLocation is $($ExeLocation)"
Write-Host "Installing $($PackageName)..."
Start-Process -Filepath "$($ExeLocation)" -ArgumentList "/install /quiet /norestart" -Wait
 
Write-Host "Cleanup..."
rm $ExeLocation
 
Write-Host "Installation of $($PackageName) complete!"