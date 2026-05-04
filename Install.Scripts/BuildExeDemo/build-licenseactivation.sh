#!/bin/bash
set -euo pipefail
SCRIPT_HOME=$(cd "$(dirname "$0")"; pwd -P)

pushd $SCRIPT_HOME

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
dotnet msbuild /t:BuildExeDemo -tl:off -property:DEMO_NAME_CODE=licenseactivation -property:VERSION_SUFFIX=10.1.0 -property:TARGET_CONFIG=Release build-demo.proj

popd
