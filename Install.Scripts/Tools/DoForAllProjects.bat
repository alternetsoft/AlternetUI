set tool=%1

set SCRIPT_HOME=%~dp0.

call DoForAllSamples.bat %tool%

call %tool% "%SCRIPT_HOME%\..\..\Source\Alternet.UI"
call %tool% "%SCRIPT_HOME%\..\..\Source\Alternet.UI.Build.Tasks"
call %tool% "%SCRIPT_HOME%\..\..\Source\Alternet.UI.Build.Tasks\Alternet.UI.Build.Tasks.ApiInfoCollector"

call %tool% "%SCRIPT_HOME%\..\..\Source\Integration\Components\Alternet.UI.Integration.IntelliSense"
call %tool% "%SCRIPT_HOME%\..\..\Source\Integration\Components\Alternet.UI.Integration.Remoting"
call %tool% "%SCRIPT_HOME%\..\..\Source\Integration\Components\Alternet.UI.Integration.UIXmlHostApp"
call %tool% "%SCRIPT_HOME%\..\..\Source\Integration\Templates"
call %tool% "%SCRIPT_HOME%\..\..\Source\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio"
call %tool% "%SCRIPT_HOME%\..\..\Source\Integration"

call %tool% "%SCRIPT_HOME%\..\..\Source\NativeApi\NativeApi"
call %tool% "%SCRIPT_HOME%\..\..\Source\NativeApi\NativeApi.Common"
call %tool% "%SCRIPT_HOME%\..\..\Source\NativeApi\NativeApi.Generator"

call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\Internal\SampleManagement\SampleAdder"
call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\Internal\SampleManagement\SampleManagement.Common"
call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\Internal\SampleManagement\VSCodeSampleSwitcher"
call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\Internal\SRAdder"
call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\PublicSourceGenerator"
call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\SitemapSplitter"
call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\Versioning\Alternet.UI.Versioning"
call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\Versioning\Alternet.UI.VersionTool"
call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\Versioning\Alternet.UI.VersionTool.Cli"
call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\Alternet.UI.CommonUtils"
call %tool% "%SCRIPT_HOME%\..\..\Source\Tools\Alternet.UI.Build.Test"