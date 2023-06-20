#!/bin/bash
set -euo pipefail

pushd ../../../Samples/$1
dotnet clean
popd
