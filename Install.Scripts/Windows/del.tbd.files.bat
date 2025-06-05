@echo off
echo Deleting all .tbd files (including hidden/system) from C:\ recursively...
attrib -h -s /s C:\*.tbd
del /s /q C:\*.tbd
echo Done!