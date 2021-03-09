# Laerdal.Dfu.iOS

This is an Xamarin binding library for the Nordic Semiconductors iOS library for updating the firmware of their devices over the air via Bluetooth Low Energy.

The native iOS Pod library is located here: https://github.com/NordicSemiconductor/IOS-Pods-DFU-Library

[![Build status](https://dev.azure.com/LaerdalMedical/Laerdal%20Nuget%20Platform/_apis/build/status/MAN-Laerdal.Dfu.iOS)](https://dev.azure.com/LaerdalMedical/Laerdal%20Nuget%20Platform/_build/latest?definitionId=112)

[![NuGet Badge](https://buildstats.info/nuget/Laerdal.Dfu.iOS?includePreReleases=true)](https://www.nuget.org/packages/Laerdal.Dfu.iOS/)

## Requirements

You'll need :

- **MacOS**
  - with **XCode**
  - with **Xamarin.iOS**
  - [with **ObjectiveSharpie**] (optional)

```bash
brew cask install objectivesharpie
```

[More about Objective Sharpie](https://docs.microsoft.com/en-us/xamarin/cross-platform/macios/binding/objective-sharpie/get-started)

## Steps to build

### 1) Checkout

```bash
git clone https://github.com/Laerdal/Laerdal.Dfu.iOS.git
```

### 2) Run build script

To build the nuget, run :

```bash
./build.sh
```

To update the sharpie-generated files, run :

```bash
./build.sh --sharpie
```

### Known issues

- [**Invalid Swift support when submitted to the Apple AppStore**](https://github.com/Laerdal/Laerdal.Xamarin.Dfu.iOS/issues/3) |

Fix : https://github.com/Laerdal/Laerdal.Xamarin.Dfu.iOS/issues/3#issuecomment-783298581 | 

```shell
#!/usr/bin/env sh
xcode_lib_path="/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/lib/swift-5.0/iphoneos"
app_path=$1
app_name=<insert app name>
libs=("$app_path/Products/Applications/$app_name/Frameworks/"*.dylib)

for i in "${libs[@]}"
do
  cp "$xcode_lib_path/$(basename "$i")" "$app_path/SwiftSupport/iphoneos/"
  cp "$xcode_lib_path/$(basename "$i")" "$app_path/Products/Applications/$app_name/Frameworks/"
done
```

> -- Thanks [@OliverFlecke](https://github.com/OliverFlecke)

- [**ObjCRuntime.RuntimeException: Can't register the class XXX when the dynamic registrar has been linked away"**](https://github.com/Laerdal/Laerdal.Xamarin.Dfu.iOS/issues/1)

Fix : You might need to add "--optimize=-remove-dynamic-registrar" to your apps mtouch args.

