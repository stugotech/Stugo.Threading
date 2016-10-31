@echo off

set ARGS=/t:clean,build
if not [%1]==[] set ARGS=%*

"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /p:Configuration=Release %ARGS% "Stugo.Threading.sln"
