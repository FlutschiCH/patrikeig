@echo off
setlocal enabledelayedexpansion
title AbsolutDashboard Multi-Device Unlock Tool
cd /d "%~dp0"

echo ========================================================
echo    AbsolutDashboard Multi-Device Unlock Tool
echo ========================================================
echo.

where python >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [!] Python is not found in PATH!
    echo Please install Python 3.10+ from python.org
    pause
    exit /b 1
)

python -c "import paramiko" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [*] Installing required paramiko package...
    pip install paramiko
)

echo [*] Launching Multi-Device Fix GUI...
python patch_gui.py
