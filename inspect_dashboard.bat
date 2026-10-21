@echo off
setlocal enabledelayedexpansion

cd /d "%~dp0"

echo =======================================================
echo     DEEP INSPECT ABSOLUTDASHBOARD (10.20.110.7)
echo =======================================================
echo.

python backend\inspect_dashboard.py

echo.
pause
