@echo off
::echo %time%
call npx ts-node xml2docx.ts
::call npx esbuild xml2docx.ts --platform=node --bundle --format=esm --outfile=xml2docx.mjs
::node xml2docx.mjs
start doc.docx
::echo %time%
