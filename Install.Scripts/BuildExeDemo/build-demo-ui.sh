#!/bin/bash
set -euo pipefail
SCRIPT_HOME=$(cd "$(dirname "$0")"; pwd -P)

pushd $SCRIPT_HOME

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
dotnet msbuild /t:BuildExeDemo -tl:off -property:DEMO_NAME_CODE=ui";" build-demo.proj

popd
