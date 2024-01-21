@echo off

cd /d %~dp0\..

mkdir output > NUL 2> NUL
for %%i in (output\page-*.png) do copy /y scripts\loader.img %%i > NUL 2> NUL
del /f "output\%~n1.pdf" > NUL 2> NUL
scripts\xml2docx.exe -d "%~dpn1.json" "%1" "output\%~n1.docx" || goto error
.venv\Scripts\docx2pdf.exe "output\%~n1.docx" || goto error
scripts\gs\bin\gswin64c.exe -dSAFER -dBATCH -dNOPAUSE -sDEVICE=png16m -r150 -dTextAlphaBits=4 "-sOutputFile=output\page-%%00d.png" "output\%~n1.pdf"
goto done
:error
for %%i in (output\page-*.png) do copy /y scripts\error.img %%i > NUL 2> NUL
exit /b 1
:done