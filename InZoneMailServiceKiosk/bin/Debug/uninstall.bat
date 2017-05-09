@echo off
installutil /u InZoneMailService.exe
if not %ERRORLEVEL% 0 pause