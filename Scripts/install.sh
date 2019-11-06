#! /bin/sh

BASE_URL="http://netstorage.unity3d.com/unity/5f859a4cfee5/MacEditorInstaller/Unity.pkg"

echo "Downloading from $BASE_URL"
curl -o Unity.pkg $BASE_URL

echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /
