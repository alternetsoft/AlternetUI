@echo off
echo === MSBuild Diagnostic ===

:: Show full path to msbuild
echo [1] msbuild location:
where msbuild

:: Show full path to nmake (if present)
echo [2] nmake location:
where nmake

:: Show full path to vcvarsall.bat
echo [3] vcvarsall.bat location:
where vcvarsall.bat

:: Show full path to VsDevCmd.bat
echo [4] VsDevCmd.bat location:
where VsDevCmd.bat

:: Show current environment variables related to MSVC
echo [5] Environment variables:
echo VSINSTALLDIR=%VSINSTALLDIR%
echo VSCMD_ARG_TGT_ARCH=%VSCMD_ARG_TGT_ARCH%
echo VSCMD_VER=%VSCMD_VER%
echo VCINSTALLDIR=%VCINSTALLDIR%
echo PATH=%PATH%

pause