#!/bin/bash
set -euo pipefail
pushd Install.Scripts
pushd BuildAndRunSamples

./BuildAndRun.ControlsSample.net10.sh
popd
popd
