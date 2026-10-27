@echo off
setlocal enabledelayedexpansion

echo =======================================================
echo    APPLY PERMANENT LICENSE BYPASS ON SERVER (10.20.110.7)
echo =======================================================
echo.

cd /d "%~dp0"

echo [*] Connecting via SSH to 10.20.110.7...
python backend\apply_license_patch.py

echo.
pause
