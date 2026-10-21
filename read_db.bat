@echo off
setlocal enabledelayedexpansion

cd /d "%~dp0"

echo =======================================================
echo     EXTRACT 5G DEVICES & QUERIES FROM APP.DB
echo =======================================================
echo.

python backend\read_absolut_db.py

echo.
pause
