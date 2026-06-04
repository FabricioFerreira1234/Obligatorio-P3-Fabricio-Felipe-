@echo off
REM Mata procesos zombie de las apps que dejan bloqueado el bin y cuelgan el build de Visual Studio.
REM Si VS "queda cargando" al ejecutar: corre este .bat y volve a apretar F5.
taskkill /F /IM ObligatorioWebApi.exe 2>nul
taskkill /F /IM StellarMinds.WebApp.exe 2>nul
echo Procesos de las apps detenidos. Ya podes ejecutar desde Visual Studio.
pause
