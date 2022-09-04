@echo off

:find7zip
set TOOLS_OK=n
set TOOLS_INSTALL=n
set OLD_PATH=%PATH%
call :check_tools_first
call :check_tools %ProgramFiles%\7-Zip
call :check_tools %ProgramFiles(x86)%\7-Zip
call :check_tools %ProgramW6432%\7-Zip
call :check_reg HKCU\SOFTWARE\7-Zip Path /reg:32
call :check_reg HKCU\SOFTWARE\7-Zip Path64 /reg:32
call :check_reg HKLM\SOFTWARE\7-Zip Path /reg:32
call :check_reg HKLM\SOFTWARE\7-Zip Path64 /reg:32
call :check_reg HKCU\SOFTWARE\7-Zip Path /reg:64
call :check_reg HKCU\SOFTWARE\7-Zip Path64 /reg:64
call :check_reg HKLM\SOFTWARE\7-Zip Path /reg:64
call :check_reg HKLM\SOFTWARE\7-Zip Path64 /reg:64
call :check_tools %TOOLS_INSTALL%
if '%TOOLS_OK%'=='y' goto findvs
set PATH=%OLD_PATH%
echo Nie znaleziono narzedzia kompresującego 7-Zip!
exit /b 50

:check_tools
if '%TOOLS_OK%'=='y' goto done
set PATH=%*;%OLD_PATH%
:check_tools_first
where 7z > NUL 2> NUL
if errorlevel 1 goto done
set TOOLS_OK=y
goto done

:findvs
set TOOLS_OK=n
set TOOLS_INSTALL=n
set OLD_PATH=%PATH%
call :check_vs_first
call :check_vs %ProgramFiles%\Microsoft Visual Studio 10.0\Common7\IDE
call :check_vs %ProgramFiles(x86)%\Microsoft Visual Studio 10.0\Common7\IDE
call :check_vs %ProgramW6432%\Microsoft Visual Studio 10.0\Common7\IDE
call :check_vs %ProgramFiles%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
call :check_vs %ProgramFiles%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\amd64
call :check_vs %ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin
call :check_vs %ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\amd64
call :check_vs %ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
call :check_vs %ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\amd64
call :check_vs %ProgramFiles(x86)%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin
call :check_vs %ProgramFiles(x86)%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\amd64
call :check_vs %ProgramW6432%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin
call :check_vs %ProgramW6432%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\amd64
call :check_vs %ProgramW6432%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin
call :check_vs %ProgramW6432%\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\amd64
call :check_reg HKCU\Software\Microsoft\VCSExpress\10.0_Config InstallDir /reg:32
call :check_reg HKCU\Software\Microsoft\VCSExpress\10.0_Config InstallDir /reg:64
call :check_reg HKLM\SOFTWARE\Microsoft\VCSExpress\10.0 InstallDir /reg:32
call :check_reg HKLM\SOFTWARE\Microsoft\VCSExpress\10.0 InstallDir /reg:64
call :check_vs %TOOLS_INSTALL%
if '%TOOLS_OK%'=='y' goto run_dist
set PATH=%OLD_PATH%
echo Nie znaleziono narzedzia kompilatora!
exit /b 50

:check_vs
if '%TOOLS_OK%'=='y' goto done
set PATH=%*;%OLD_PATH%
:check_vs_first
where VCSExpress > NUL 2> NUL
if errorlevel 1 goto check_vs_msbuild
set TOOLS_OK=y
goto done
:check_vs_msbuild
where msbuild > NUL 2> NUL
if errorlevel 1 goto done
set TOOLS_OK=y
goto done

:run_dist
where 7z
where msbuild > NUL 2> NUL
if errorlevel 1 goto use_vsexpress

where msbuild
msbuild DotBase.sln -p:Configuration=Release
if errorlevel 1 exit /b %ERRORLEVEL%
goto create_package

:use_vsexpress
where VCSExpress
del /s /q %~dp0\dotbase\bin\Release
del /s /q %~dp0\dotbase\obj
rmdir /s /q %~dp0\dotbase\bin\Release
rmdir /s /q %~dp0\dotbase\obj
VCSExpress %~dp0\dotbase\DotBase.csproj /build Release
if not exist %~dp0\dotbase\bin\Release\DotBase.exe goto compile_error

:create_package
del /s /q %~dp0\dist
rmdir /s /q %~dp0\dist
mkdir %~dp0\dist
xcopy %~dp0\dotbase\bin\Release %~dp0\dist\Release\ /E /Q /R /Y
xcopy %~dp0\dotbase\bin\szablony %~dp0\dist\szablony\ /E /Q /R /Y
xcopy %~dp0\dotbase\bin\wyniki %~dp0\dist\wyniki\ /E /Q /R /Y
del /q %~dp0\dist\Release\*.pdb
del /s /q %~dp0\dist\wyniki\*.html
%~dp0\dist\Release\DotBase --version > %~dp0\dist\VERSION.txt
if errorlevel 1 goto compile_error
set /p VERSION=<%~dp0\dist\VERSION.txt
7z a %~dp0\dist\DotBase-%VERSION%.7z %~dp0\dist\Release %~dp0\dist\szablony %~dp0\dist\wyniki
exit /b 0

:compile_error
echo Compilation error.
echo Compile project manually to see errors.
exit /b 1

:check_reg
if '%TOOLS_OK%'=='y' goto done
if not "%TOOLS_INSTALL%"=="n" goto done
FOR /F "tokens=1-2*" %%A IN ('REG QUERY %1 /v %2 %3 2^> nul ^| find /i "%2"') DO set TOOLS_INSTALL=%%C
goto done

:done
