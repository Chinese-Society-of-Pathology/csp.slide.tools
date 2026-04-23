#!/bin/bash
SCRIPT_PATH=$(pwd)
echo $SCRIPT_PATH
echo export DYLD_LIBRARY_PATH=$SCRIPT_PATH:$DYLD_LIBRARY_PATH >> ~/.zprofile
source ~/.zprofile
echo $DYLD_LIBRARY_PATH
