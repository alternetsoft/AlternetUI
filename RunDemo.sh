#!/bin/bash
set -euo pipefail
pushd Install.Scripts
pushd BuildAndRunSamples

./BuildAndRun.ControlsSample.sh
popd
popd
