@echo off
call buildvars.cmd
setlocal enabledelayedexpansion
set "content="
for /f "usebackq delims=" %%a in ("%filename%") do (
    set "content=!content!%%a"
)
echo Generating setup...
%innosetup% /dMyAppVersion="%content%" setup\setupviz.iss
del dist\AnotherRTSP_v%content%-Setup.exe
ren dist\AnotherRTSP-Setup.exe AnotherRTSP_v%content%-Setup.exe
endlocal
pause