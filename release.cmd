@echo off

if [%1]==[] goto usage

git tag -a %1 -m "Version %1"
call .\build.cmd

goto :eof
:usage
echo Specify version
