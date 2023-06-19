set tool=%1

set SCRIPT_HOME=%~dp0.

call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\CommonDialogsSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\ControlsSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\ControlsTest"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\CustomControlsSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\DataBindingSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\DragAndDropSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\DrawingSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\EmployeeFormSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\ExplorerUISample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\HelloWorldSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\InputSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\LayoutSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\MenuSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\PaintSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\PrintingSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\ThreadingSample"
call %tool% "%SCRIPT_HOME%\..\..\Source\Samples\WindowPropertiesSample"

call %tool% "%SCRIPT_HOME%\..\..\Source\Tests\ControlsTest"

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
