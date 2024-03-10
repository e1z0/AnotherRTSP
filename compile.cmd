@echo off
call buildvars.cmd
setlocal enabledelayedexpansion
echo Building Project...
"%msbuildPath%" Src\AnotherRTSP.sln /p:Configuration=Release
endlocal