$scriptDir = Split-Path -Path $PSCommandPath -Parent
$exePath = $scriptDir + "\mpv.net\bin\mpvnet.exe"
$version = [Diagnostics.FileVersionInfo]::GetVersionInfo($exePath).FileVersion
$desktopDir = [Environment]::GetFolderPath("Desktop")
$targetDir = $desktopDir + "\mpv.net-" + $version
Copy-Item $scriptDir\mpv.net\bin $targetDir -Recurse -Exclude System.Management.Automation.xml -Force
Copy-Item $scriptDir\README.md $targetDir\README.md -Force
& "C:\Program Files\7-Zip\7z.exe" a -t7z -mx9 "$targetDir.7z" -r "$targetDir\*"