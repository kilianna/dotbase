@echo off

cd /d %~dp0

set TOOLS_OK=n
set TOOLS_INSTALL=n
set OLD_PATH=%PATH%
call :check_tools_first
call :check_tools %ProgramFiles%\7-Zip
call :check_tools %ProgramFiles(x86)%\7-Zip
call :check_tools %~dp0\7-Zip
call :check_reg HKCU\SOFTWARE\7-Zip Path /reg:32
call :check_reg HKCU\SOFTWARE\7-Zip Path64 /reg:32
call :check_reg HKLM\SOFTWARE\7-Zip Path /reg:32
call :check_reg HKLM\SOFTWARE\7-Zip Path64 /reg:32
call :check_reg HKCU\SOFTWARE\7-Zip Path /reg:64
call :check_reg HKCU\SOFTWARE\7-Zip Path64 /reg:64
call :check_reg HKLM\SOFTWARE\7-Zip Path /reg:64
call :check_reg HKLM\SOFTWARE\7-Zip Path64 /reg:64
call :check_tools %TOOLS_INSTALL%
if '%TOOLS_OK%'=='y' goto download
set PATH=%OLD_PATH%
echo 7-Zip not found!
exit /b 50

:check_tools
if '%TOOLS_OK%'=='y' goto done
set PATH=%*;%OLD_PATH%
:check_tools_first
7z i > NUL 2> NUL
if errorlevel 1 goto done
set TOOLS_OK=y
goto done

:check_reg
if '%TOOLS_OK%'=='y' goto done
if not "%TOOLS_INSTALL%"=="n" goto done
FOR /F "tokens=1-2*" %%A IN ('REG QUERY %1 /v %2 %3 2^> nul ^| find /i "%2"') DO set TOOLS_INSTALL=%%C
goto done

:download

SET CURL=%~dp0\curl\bin\curl.exe
IF EXIST %CURL% GOTO found_curl
SET CURL=%~dp0\curl\curl.exe
IF EXIST %CURL% GOTO found_curl
SET CURL=%~dp0\curl.exe
IF EXIST %CURL% GOTO found_curl
SET CURL=curl
:found_curl

%CURL% -L -o xml2docx.zip https://github.com/kildom/xml2docx/releases/latest/download/xml2docx-win.zip
7z -y x xml2docx.zip
del /y /s xml2docx.zip

:done

