#!/bin/bash
SCRIPT_PATH=$(pwd)
echo $SCRIPT_PATH
sed -i "s@http://.*archive.ubuntu.com@http://mirrors.aliyun.com@g" /etc/apt/sources.list
sed -i "s@http://.*security.ubuntu.com@http://mirrors.aliyun.com@g" /etc/apt/sources.list
apt update && apt install -y libopenslide-dev && apt autoclean
echo export LD_LIBRARY_PATH=$SCRIPT_PATH:$LD_LIBRARY_PATH >> /etc/profile
source /etc/profile
echo $LD_LIBRARY_PATH