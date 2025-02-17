SETLOCAL EnableDelayedExpansion

set SCRIPT_HOME=%~dp0.
set PACKAGES_DIR=%SCRIPT_HOME%\..\Publish\Packages
set SOURCE_DIR=%SCRIPT_HOME%\..\Source
set VersionToolProject=%SOURCE_DIR%\Tools\Versioning\Alternet.UI.VersionTool.Cli\Alternet.UI.VersionTool.Cli.csproj
set DOCS_DIR=%SCRIPT_HOME%\..\Documentation\Alternet.UI.Documentation\site\

pushd %DOCS_DIR%

del %PACKAGES_DIR%\PublicDocumentation*.zip
del %PACKAGES_DIR%\PublicDocumentation*.7z
"C:\Program Files\WinRar\WinRAR.exe" a -r "%PACKAGES_DIR%\PublicDocumentation.zip" "*.*"

"C:\Program Files\7-Zip\7z.exe" a -r "%PACKAGES_DIR%\PublicDocumentation.7z" "*.*"

dotnet run --project "%VersionToolProject%" --property WarningLevel=0 -- append-version-suffix "%PACKAGES_DIR%\PublicDocumentation.zip"
dotnet run --project "%VersionToolProject%" --property WarningLevel=0 -- append-version-suffix "%PACKAGES_DIR%\PublicDocumentation.7z"

popd
