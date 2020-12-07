#!/bin/bash

xbuild=/Applications/Xcode.app/Contents/Developer/usr/bin/xcodebuild
ios_native_folder="NordicSemiconductor/IOS-Pods-DFU-Library"
output="Laerdal.Xamarin.Dfu.iOS/Frameworks"


# Building for SDK iphoneos
$xbuild ONLY_ACTIVE_ARCH=NO -project $ios_native_folder/_Pods.xcodeproj -sdk iphoneos -configuration Release build
# Building for SDK iphonesimulator
$xbuild ONLY_ACTIVE_ARCH=NO -project $ios_native_folder/_Pods.xcodeproj -sdk iphonesimulator -configuration Release build

# Copy to output
mkdir -p $output
cp -a $ios_native_folder/Example/build/Release-iphoneos/iOSDFULibrary-iOS/. $output
cp -a $ios_native_folder/Example/build/Release-iphoneos/ZIPFoundation-iOS/. $output

# Building fat lib
lib_fat=$output/iOSDFULibrary.framework/iOSDFULibrary
lib_iphoneos=$ios_native_folder/Example/build/Release-iphoneos/iOSDFULibrary-iOS/iOSDFULibrary.framework/iOSDFULibrary
lib_iphonesimulator=$ios_native_folder/Example/build/Release-iphonesimulator/iOSDFULibrary-iOS/iOSDFULibrary.framework/iOSDFULibrary

rm $lib_fat
lipo -create -output "$lib_fat" "$lib_iphoneos" "$lib_iphonesimulator"

# Building nuget
MSBuild Laerdal.Xamarin.Dfu.iOS/*.csproj -t:Rebuild -restore:True -p:Configuration=Release