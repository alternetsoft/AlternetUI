#!/bin/bash
set -euo pipefail

pushd ../../Source/Samples/$1
dotnet clean
popd
