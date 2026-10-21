@echo off
setlocal enabledelayedexpansion

cd /d "%~dp0"

echo =======================================================
echo     TESTING REMOTE DEVICE (10.20.110.7)
echo =======================================================
echo.

python backend\test_remote_device.py

echo.
pause
