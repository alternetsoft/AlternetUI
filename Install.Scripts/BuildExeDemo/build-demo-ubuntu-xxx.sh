#!/bin/bash
set -euo pipefail
SCRIPT_HOME=$(cd "$(dirname "$0")"; pwd -P)

DEMO_FRAMEWORK=$1
DEMO_NAME=$2
DEMO_PLATFORM=$3

# Build
pushd $SCRIPT_HOME

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true
dotnet msbuild /t:BuildExeDemo -tl:off -property:DEMO_NAME=$DEMO_NAME";"DEMO_FRAMEWORK=$DEMO_FRAMEWORK";"DEMO_PLATFORM=$DEMO_PLATFORM build-demo.proj

popd
