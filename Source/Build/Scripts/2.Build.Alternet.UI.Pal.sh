#!/bin/bash
set -euo pipefail

pushd ../../Alternet.UI.Pal/build

./debug.sh
./release.sh

popd



