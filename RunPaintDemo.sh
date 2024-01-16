#!/bin/bash
set -euo pipefail
pushd Install.Scripts
pushd BuildAndRunSamples

./BuildAndRun.PaintSample.sh
popd
popd