@echo off
cd /d "%~dp0"

echo =======================================================
echo    ABSOLUTDASHBOARD MULTI-DEVICE PATCH AND DEPLOY GUI
echo =======================================================
echo.

echo [*] Pulling latest changes from git...
git pull

echo [*] Launching Multi-Device Patch GUI...
python patch_gui.py
pause
