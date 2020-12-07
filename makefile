
xbuild=/Applications/Xcode.app/Contents/Developer/usr/bin/xcodebuild
xbuild_output=Laerdal.Xamarin.Dfu.iOS/Frameworks
ios_native_folder=NordicSemiconductor/IOS-Pods-DFU-Library

lib_fat=$(xbuild_output)/iOSDFULibrary.framework/iOSDFULibrary
lib_iphoneos=$(ios_native_folder)/Example/build/Release-iphoneos/iOSDFULibrary-iOS/iOSDFULibrary.framework/iOSDFULibrary
lib_iphonesimulator=$(ios_native_folder)/Example/build/Release-iphonesimulator/iOSDFULibrary-iOS/iOSDFULibrary.framework/iOSDFULibrary

version=1.0.0
output=Laerdal.Xamarin.Dfu.iOS.Output

all: $(output)


$(ios_native_folder)/Example/build/Release-iphoneos/:
	# Building for SDK iphoneos
	$(xbuild) ONLY_ACTIVE_ARCH=NO -project $(ios_native_folder)/_Pods.xcodeproj -sdk iphoneos -configuration Release build

$(ios_native_folder)/Example/build/Release-iphonesimulator/:
	# Building for SDK iphoneos
	$(xbuild) ONLY_ACTIVE_ARCH=NO -project $(ios_native_folder)/_Pods.xcodeproj -sdk iphonesimulator -configuration Release build

$(xbuild_output)/: $(ios_native_folder)/Example/build/Release-iphoneos/ $(ios_native_folder)/Example/build/Release-iphonesimulator/
	# Copy to $(xbuild_output)
	mkdir -p $(xbuild_output)
	cp -a $(ios_native_folder)/Example/build/Release-iphoneos/iOSDFULibrary-iOS/. $(xbuild_output)
	cp -a $(ios_native_folder)/Example/build/Release-iphoneos/ZIPFoundation-iOS/. $(xbuild_output)

	# Building fat lib
	rm -rf $(lib_fat)
	lipo -create -output $(lib_fat) $(lib_iphoneos) $(lib_iphonesimulator)

$(output): $(xbuild_output)
	# Building nuget
	MSBuild Laerdal.Xamarin.Dfu.iOS/*.csproj -t:Rebuild -restore:True -p:Configuration=Release -p:PackageVersion=$(version) -p:PackageOutputPath=../$(output)

clean:
	rm -rf $(output)
	rm -rf $(xbuild_output)
	$(xbuild) ONLY_ACTIVE_ARCH=NO -project $(ios_native_folder)/_Pods.xcodeproj clean
	# Cleaning MSBuild output
	MSBuild Laerdal.Xamarin.Dfu.iOS/*.csproj -t:clean


