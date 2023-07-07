SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set SOURCE_DIR=%SCRIPT_HOME%\..\Source
set PublishRoot=%SOURCE_DIR%\..\Publish\

set PublicSourceGeneratorToolProject=%SOURCE_DIR%\Tools\PublicSourceGenerator\Alternet.UI.PublicSourceGenerator.csproj

dotnet run --project "%PublicSourceGeneratorToolProject%" --property WarningLevel=0  -- components
