@echo off
REM Lanzador de StellarMinds: levanta WebApi (7011) y WebApp (7104) en consolas separadas.
REM Doble clic para ejecutar. Cerrar las ventanas de consola detiene las apps.
cd /d "%~dp0"

echo Asegurando que LocalDB este iniciado...
sqllocaldb start MSSQLLocalDB

echo Iniciando WebApi (https://localhost:7011)...
start "StellarMinds - WebApi" cmd /k dotnet run --project "ObligatorioWebApi" --launch-profile https

echo Esperando a que la API arranque...
timeout /t 6 /nobreak >nul

echo Iniciando WebApp (https://localhost:7104)...
start "StellarMinds - WebApp" cmd /k dotnet run --project "StellarMinds.WebApp" --launch-profile https

echo Esperando a que la WebApp arranque...
timeout /t 8 /nobreak >nul

echo Abriendo el navegador...
start https://localhost:7104

echo.
echo Listo. WebApp: https://localhost:7104   WebApi/Swagger: https://localhost:7011/swagger
echo Usuarios (pass 1234!): fabricioferreira1169@stellar.com (Admin), maria@stellar.com (Coord), JuanPerez@stellar.com (Socio)
echo Para detener: cerra las dos ventanas de consola que se abrieron.
pause
