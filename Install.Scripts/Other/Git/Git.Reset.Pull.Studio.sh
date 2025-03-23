#!/bin/bash

pushd AlternetStudio
git reset --hard
git pull
popd

./chmode.sh