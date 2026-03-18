@echo off
setlocal
set "scriptDir=%~dp0"
powershell -NoProfile -ExecutionPolicy Bypass -File "%scriptDir%run-coverage.ps1" %*
endlocal