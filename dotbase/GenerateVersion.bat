@echo off

:generate
set TOOLS_OK=n
set GIT_INSTALL=n
set OLD_PATH=%PATH%
call :check_tools_first
call :check_tools %ProgramFiles%\Git\mingw64\bin;%ProgramFiles%\Git\usr\local\bin;%ProgramFiles%\Git\usr\bin
call :check_tools %ProgramFiles(x86)%\Git\mingw64\bin;%ProgramFiles(x86)%\Git\usr\local\bin;%ProgramFiles(x86)%\Git\usr\bin
call :check_tools %ProgramW6432%\Git\mingw64\bin;%ProgramW6432%\Git\usr\local\bin;%ProgramW6432%\Git\usr\bin
call :check_reg HKEY_LOCAL_MACHINE\SOFTWARE\GitForWindows InstallPath /reg:32
call :check_reg HKEY_CURRENT_USER\SOFTWARE\GitForWindows InstallPath /reg:32
call :check_reg HKEY_LOCAL_MACHINE\SOFTWARE\GitForWindows InstallPath /reg:64
call :check_reg HKEY_CURRENT_USER\SOFTWARE\GitForWindows InstallPath /reg:64
call :check_tools %GIT_INSTALL%\mingw64\bin;%GIT_INSTALL%\usr\local\bin;%GIT_INSTALL%\usr\bin
if '%TOOLS_OK%'=='y' goto run_git
set PATH=%OLD_PATH%
echo Nie znaleziono narzedzia git!
exit /b 50

:run_git
cd %2
git log -n 1 --oneline --pretty=format:"%%H %%ci" > "%~1\Resources\GitVersion_tmp.txt"
set GIT_CODE=%ERRORLEVEL%
git update-index --skip-worktree dotbase/Resources/GitVersion.txt
if not '%GIT_CODE%'=='0' goto skip_copy
fc "%~1\Resources\GitVersion_tmp.txt" "%~1\Resources\GitVersion.txt" > NUL
if not errorlevel 1 goto skip_copy
del "%~1\Resources\GitVersion.txt"
copy "%~1\Resources\GitVersion_tmp.txt" "%~1\Resources\GitVersion.txt"
:skip_copy
del "%~1\Resources\GitVersion_tmp.txt"
exit /b %GIT_CODE%

:check_tools
if '%TOOLS_OK%'=='y' goto done
set PATH=%*;%OLD_PATH%
:check_tools_first
git --version
where git > NUL 2> NUL
if errorlevel 1 goto done
set TOOLS_OK=y
goto done

:check_reg
if '%TOOLS_OK%'=='y' goto done
if not "%GIT_INSTALL%"=="n" goto done
FOR /F "tokens=1-2*" %%A IN ('REG QUERY %1 /v %2 %3 2^> nul ^| find /i "%2"') DO set GIT_INSTALL=%%C
goto done

:done
