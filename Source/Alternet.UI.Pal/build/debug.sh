#!/bin/bash

TargetDirectory=../bin/x64/Debug
if [ ! -d $TargetDirectory ]
then
    mkdir -p $TargetDirectory
fi

cd $TargetDirectory
cmake -DCMAKE_BUILD_TYPE=Debug ../../..
make -j 6