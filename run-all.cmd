@echo off
setlocal
set "scriptDir=%~dp0"
powershell -NoProfile -ExecutionPolicy Bypass -File "%scriptDir%run-all.ps1" %*
endlocal