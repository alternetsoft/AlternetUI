#!/bin/bash
TargetDirectory=../bin/x64/Release
if [ ! -d $TargetDirectory ]
then
    mkdir -p $TargetDirectory
fi

cd $TargetDirectory

cmake -DCMAKE_BUILD_TYPE=Release ../../..
#cmake -DCMAKE_BUILD_TYPE=RelWithDebInfo ../../..

make -j 6