@echo off
setlocal enabledelayedexpansion

echo =======================================================
echo    ABSOLUTDASHBOARD MULTI-DEVICE PATCH & DEPLOY GUI
echo =======================================================
echo.

cd /d "%~dp0"

REM Check for paramiko dependency, install silently if missing
python -c "import paramiko" 2>nul
if errorlevel 1 (
    echo [*] Installing required SSH library (paramiko)...
    python -m pip install paramiko --quiet
)

echo [*] Launching Multi-Device Patch GUI...
python patch_gui.py

if errorlevel 1 (
    echo.
    echo [!] An error occurred while running patch_gui.py
    pause
)
