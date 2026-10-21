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
echo [*] Checking Backend requirements.txt...
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

echo [*] Installing / Updating Backend Dependencies...
python -m pip install -r "backend\requirements.txt" --quiet

echo.
echo =======================================================
echo     [SUCCESS] Running Database Extraction...
echo =======================================================
echo.

python backend\read_absolut_db.py

echo.
echo Press any key to start the background WSGI and Debug GUI...
pause >nul

cd /d "%~dp0backend"
python app.py

if errorlevel 1 (
    echo [!] Application exited with error code %errorlevel%.
    pause
)
