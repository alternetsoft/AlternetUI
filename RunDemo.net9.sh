#!/bin/bash
set -euo pipefail
pushd Install.Scripts
pushd BuildAndRunSamples

./BuildAndRun.ControlsSample.net9.sh
popd
popd
