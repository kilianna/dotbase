@echo off
::echo %time%
call npx ts-node test_xml.ts
::call npx esbuild test_xml.ts --platform=node --bundle --format=esm --outfile=test_xml.mjs
::node test_xml.mjs
start doc.docx
::echo %time%
