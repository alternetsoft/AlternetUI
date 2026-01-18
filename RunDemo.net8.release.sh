#!/bin/bash
set -euo pipefail
pushd Install.Scripts
pushd BuildAndRunSamples

./BuildAndRun.ControlsSample.Release.sh
popd
popd
