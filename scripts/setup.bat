@echo off
pushd .
cd /d %~dp0

if exist ..\.venv goto skip_venv_setup

set PYTHON_EXE=python
if not [%1] == [] set PYTHON_EXE=%1

%PYTHON_EXE% -m venv ..\.venv && goto venv_ready
%PYTHON_EXE% -m virtualenv ..\.venv && goto venv_ready
echo.
echo Cannot create virtual environment
echo Try to give the python interpreter, e.x.:
echo     %0 C:\python36\python.exe
goto return

:venv_ready
call ..\.venv\Scripts\activate
pip install -r requirements.txt

:skip_venv_setup

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
goto done

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
IF EXIST %CURL% GOTO done
SET CURL=%~dp0\curl\curl.exe
IF EXIST %CURL% GOTO done
SET CURL=%~dp0\curl.exe
IF EXIST %CURL% GOTO done
SET CURL=curl

%CURL% -L -o xml2docx.zip https://github.com/kildom/xml2docx/releases/latest/download/xml2docx-win.zip
7z -y x xml2docx.zip
del /q xml2docx.zip

if exist gs\bin\gswin64c.exe goto skip_gs_download

%CURL% -L -o gs10021w64.exe https://github.com/ArtifexSoftware/ghostpdl-downloads/releases/download/gs10021/gs10021w64.exe
7z -y x -ogs gs10021w64.exe
del /q gs10021w64.exe

:skip_gs_download

:return
popd
:done
