#!/bin/bash
set -euo pipefail

pushd ../../Alternet.UI.Pal/build

./clean.sh
./debug.sh
./release.sh

popd



