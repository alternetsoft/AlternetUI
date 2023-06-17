set tool=%1

set SCRIPT_HOME=%~dp0.

call %tool% "%SCRIPT_HOME%\..\..\..\Samples\CommonDialogsSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\ControlsSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\ControlsTest"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\CustomControlsSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\DataBindingSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\DragAndDropSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\DrawingSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\EmployeeFormSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\ExplorerUISample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\HelloWorldSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\InputSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\LayoutSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\MenuSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\PaintSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\PrintingSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\ThreadingSample"
call %tool% "%SCRIPT_HOME%\..\..\..\Samples\WindowPropertiesSample"

call %tool% "%SCRIPT_HOME%\..\..\..\Tests\ControlsTest"

call %tool% "%SCRIPT_HOME%\..\..\..\Alternet.UI"
call %tool% "%SCRIPT_HOME%\..\..\..\Alternet.UI.Build.Tasks"
call %tool% "%SCRIPT_HOME%\..\..\..\Alternet.UI.Build.Tasks.ApiInfoCollector"

call %tool% "%SCRIPT_HOME%\..\..\..\Integration\Components\Alternet.UI.Integration.IntelliSense"
call %tool% "%SCRIPT_HOME%\..\..\..\Integration\Components\Alternet.UI.Integration.Remoting"
call %tool% "%SCRIPT_HOME%\..\..\..\Integration\Components\Alternet.UI.Integration.UIXmlHostApp"
call %tool% "%SCRIPT_HOME%\..\..\..\Integration\Templates"
call %tool% "%SCRIPT_HOME%\..\..\..\Integration\VisualStudio\Alternet.UI.Integration.VisualStudio"
call %tool% "%SCRIPT_HOME%\..\..\..\Integration"

call %tool% "%SCRIPT_HOME%\..\..\..\NativeApi\NativeApi"
call %tool% "%SCRIPT_HOME%\..\..\..\NativeApi\NativeApi.Common"
call %tool% "%SCRIPT_HOME%\..\..\..\NativeApi\NativeApi.Generator"

call %tool% "%SCRIPT_HOME%\..\..\..\Tools\Internal\SampleManagement\SampleAdder"
call %tool% "%SCRIPT_HOME%\..\..\..\Tools\Internal\SampleManagement\SampleManagement.Common"
call %tool% "%SCRIPT_HOME%\..\..\..\Tools\Internal\SampleManagement\VSCodeSampleSwitcher"
call %tool% "%SCRIPT_HOME%\..\..\..\Tools\Internal\SRAdder"
call %tool% "%SCRIPT_HOME%\..\..\..\Tools\PublicSourceGenerator"
call %tool% "%SCRIPT_HOME%\..\..\..\Tools\SitemapSplitter"
call %tool% "%SCRIPT_HOME%\..\..\..\Tools\Versioning\Alternet.UI.Versioning"
call %tool% "%SCRIPT_HOME%\..\..\..\Tools\Versioning\Alternet.UI.VersionTool"
call %tool% "%SCRIPT_HOME%\..\..\..\Tools\Versioning\Alternet.UI.VersionTool.Cli"

