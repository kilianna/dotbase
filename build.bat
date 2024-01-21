@echo off


call :convert swiad_wzor


::=============================================================================
goto done
:convert
cd /d %~dp0
cd input
del /f %~dp0%1.docx
..\xml2docx -d %1.json %1.xml
if not errorlevel 1 goto ok
echo -----------------------------------------------
echo                   E R R O R
echo -----------------------------------------------
cd ..
goto done
:ok
start %~dp0%1.docx
cd ..
:done
