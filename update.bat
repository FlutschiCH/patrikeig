@echo off
setlocal enabledelayedexpansion

echo =======================================================
echo          PATRIKEIG SYSTEM AUTO-UPDATER & LAUNCHER
echo =======================================================
echo.

cd /d "%~dp0"

echo [*] Discarding local changes to avoid merge conflicts...
git reset --hard HEAD
git clean -fd

echo [*] Fetching latest branches and tags from origin...
git fetch origin main

echo [*] Updating to origin/main...
git reset --hard origin/main

if errorlevel 1 (
    echo [!] Git pull failed. Checking network or repository status.
    pause
    exit /b 1
)

echo.
echo [*] Installing / Updating Frontend Dependencies...
call npm install --omit=dev

echo [*] Building Production Frontend Assets...
call npm run build

echo.
echo [*] Installing / Updating Backend Dependencies...
if exist "backend\requirements.txt" (
    python -m pip install -r backend\requirements.txt --quiet
)

echo.
echo =======================================================
echo     [SUCCESS] Update completed! Launching app.py...
echo =======================================================
echo.

cd /d "%~dp0backend"
python app.py

if errorlevel 1 (
    echo [!] Application exited with error code %errorlevel%.
    pause
)
