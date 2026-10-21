@echo off
setlocal enabledelayedexpansion

echo =======================================================
echo          PATRIKEIG SYSTEM AUTO-UPDATER & DUAL LAUNCHER
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
call npm install

echo.
echo [*] Installing / Updating Backend Dependencies...
if not exist "backend\requirements.txt" (
    (
        echo flask
        echo flask-cors
        echo gevent
        echo requests
        echo mysql-connector-python
        echo beautifulsoup4
        echo lxml
        echo pysnmp-lextudio
        echo paramiko
    ) > "backend\requirements.txt"
)
python -m pip install -r "backend\requirements.txt" --quiet

echo.
echo [*] Running AbsolutDashboard remote repair...
python backend\remote_fix.py

echo.
echo [*] Copying frontend URL to clipboard...
<nul set /p="http://localhost:5173" | clip

echo =======================================================
echo     [SUCCESS] URL COPIED TO CLIPBOARD: http://localhost:5173
echo =======================================================
echo.
echo [*] Launching Python Backend in a separate window...
start "Python Backend WSGI & Debug" cmd /k "cd /d %~dp0backend && python app.py"

echo [*] Launching React Frontend (npm run dev)...
echo.
call npm run dev
