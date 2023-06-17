#!/bin/sh
set -euo pipefail

SCRIPT_HOME=$(cd "$(dirname "$0")"; pwd -P)

export UI_ARTIFACTS_PATH=$SCRIPT_HOME/../../../Publish/Artifacts/Build/macOS/UI/
export UI_PAL_ARTIFACTS_PATH="$UI_ARTIFACTS_PATH/PAL"
