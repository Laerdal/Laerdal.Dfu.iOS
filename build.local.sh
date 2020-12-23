#!/bin/bash

# GITHUB INFORMATION
github_repo_owner=NordicSemiconductor
github_repo=IOS-Pods-DFU-Library
github_release_id=32090814
github_info_file="$github_repo_owner.$github_repo.$github_release_id.info.json"

if [ ! -f "$github_info_file" ]; then
    echo ""
    echo "### DOWNLOADING GITHUB INFORMATION ###"
    echo ""
    github_info_file_url=https://api.github.com/repos/$github_repo_owner/$github_repo/releases/$github_release_id
    echo "Downloading $github_info_file_url to $github_info_file"
    curl -s $github_info_file_url > $github_info_file
fi

# VARIABLES
usage(){
    echo "### Wrong parameters ###"
    echo "usage: ./build.local.sh [-r|--revision build_revision]"
}

build_revision=`date +%m%d%H%M%S`

while [ "$1" != "" ]; do
    case $1 in
        -r | --revision )       shift
                                build_revision=$1
                                ;;
        -h | --help )           usage
                                exit
                                ;;
        * )                     usage
                                exit 1
    esac
    shift
done

echo ""
echo "### INFORMATION ###"
echo ""

# Set version
github_tag_name=`cat $github_info_file | grep '"tag_name":' | sed -E 's/.*"([^"]+)".*/\1/' | sed 's/v//'`
github_short_version=`echo "$github_tag_name" | sed 's/.LTS//'`
build_version=$github_short_version.$build_revision
echo "##vso[build.updatebuildnumber]$build_version"
if [ -z "$github_short_version" ]; then
    echo "Failed : Could not read Version"
    cat $github_info_file
    exit 1
fi

# Static configuration
nuget_project_folder="Laerdal.Xamarin.Dfu.iOS"
nuget_project_name="Laerdal.Xamarin.Dfu.iOS"
source_folder="Laerdal.Xamarin.Dfu.iOS.Source"
source_zip_folder="Laerdal.Xamarin.Dfu.iOS.Zips"
test_project_folder="Laerdal.Xamarin.Dfu.iOS.Test"
test_project_name="Laerdal.Xamarin.Dfu.iOS.Test"

xbuild=/Applications/Xcode.app/Contents/Developer/usr/bin/xcodebuild

# Calculated configuration
nuget_output_folder="$nuget_project_name.Output"
nuget_csproj_path="$nuget_project_folder/$nuget_project_name.csproj"
nuget_filename="$nuget_project_name.$build_version.nupkg"
nuget_output_file="$nuget_output_folder/$nuget_filename"

nuget_frameworks_folder="$nuget_project_folder/Frameworks"

sharpie_output_path=$nuget_project_folder/Sharpie_Generated
sharpie_output_file=$sharpie_output_path/ApiDefinitions.cs

source_zip_file_name="$github_short_version.zip"
source_zip_file="$source_zip_folder/$source_zip_file_name"
source_zip_url="http://github.com/$github_repo_owner/$github_repo/zipball/$github_tag_name"

test_csproj_path="$test_project_folder/$test_project_name.csproj"

# Generates variables
echo "build_version = $build_version"
echo ""
echo "github_repo_owner = $github_repo_owner"
echo "github_repo = $github_repo"
echo "github_release_id = $github_release_id"
echo "github_info_file = $github_info_file"
echo "github_tag_name = $github_tag_name"
echo "github_short_version = $github_short_version"
echo ""
echo "source_zip_folder = $source_zip_folder"
echo "source_zip_file_name = $source_zip_file_name"
echo "source_zip_file = $source_zip_file"
echo "source_zip_url = $source_zip_url"
echo ""
echo "nuget_output_folder = $nuget_output_folder"
echo "nuget_csproj_path = $nuget_csproj_path"
echo "nuget_filename = $nuget_filename"
echo "nuget_output_file = $nuget_output_file"
echo "nuget_frameworks_folder = $nuget_frameworks_folder"
echo ""
echo "sharpie_output_path = $sharpie_output_path"
echo "sharpie_output_file = $sharpie_output_file"

if [ ! -f "$source_zip_file" ]; then

    echo ""
    echo "### DOWNLOAD GITHUB RELEASE FILES ###"
    echo ""

    mkdir -p $source_zip_folder
    curl -L -o $source_zip_file $source_zip_url

    if [ ! -f "$source_zip_file" ]; then
        echo "Failed to download $source_zip_url into $source_zip_file"
        exit 1
    fi

    echo "Downloaded $source_zip_url into $source_zip_file"
fi

echo ""
echo "### UNZIP SOURCE ###"
echo ""

rm -rf $source_folder
unzip -qq -n -d "$source_folder" "$source_zip_file"
if [ ! -d "$source_folder" ]; then
    echo "Failed"
    exit 1
fi
echo "Unzipped $source_zip_file into $source_folder"

echo ""
echo "### APPLY MAGIC REGEX ###"
echo ""

for i in `find ./Laerdal.Xamarin.Dfu.iOS.Source/ -ipath "*iOSDFULibrary/Classes/*" -iname "*.swift" -type f`; do
    echo "- $i"
    sed -i.old -E 's/@objc (public|internal|open) (class|enum|protocol) ([A-Za-z0-9]*)/@objc(\3) \1 \2 \3/g' $i
done

echo ""
echo "### XBUILD SOURCE ###"
echo ""

$xbuild ONLY_ACTIVE_ARCH=NO -quiet -project $source_folder/**/_Pods.xcodeproj -sdk iphoneos -configuration Release build
$xbuild ONLY_ACTIVE_ARCH=NO -quiet -project $source_folder/**/_Pods.xcodeproj -sdk iphonesimulator -configuration Release build

iOSDFULibrary_iphoneos_framework=`find ./$source_folder/ -ipath "*iphoneos*" -iname "iOSDFULibrary.framework" | head -n 1`
ZIPFoundation_iphoneos_framework=`find ./$source_folder/ -ipath "*iphoneos*" -iname "ZIPFoundation.framework" | head -n 1`
iOSDFULibrary_iphonesimulator_framework=`find ./$source_folder/ -ipath "*iphonesimulator*" -iname "iOSDFULibrary.framework" | head -n 1`
ZIPFoundation_iphonesimulator_framework=`find ./$source_folder/ -ipath "*iphonesimulator*" -iname "ZIPFoundation.framework" | head -n 1`

if [ ! -d "$iOSDFULibrary_iphoneos_framework" ]; then
    echo "Failed : $iOSDFULibrary_iphoneos_framework does not exist"
    exit 1
fi
if [ ! -d "$ZIPFoundation_iphoneos_framework" ]; then
    echo "Failed : $ZIPFoundation_iphoneos_framework does not exist"
    exit 1
fi
if [ ! -d "$iOSDFULibrary_iphonesimulator_framework" ]; then
    echo "Failed : $iOSDFULibrary_iphonesimulator_framework does not exist"
    exit 1
fi
if [ ! -d "$ZIPFoundation_iphonesimulator_framework" ]; then
    echo "Failed : $ZIPFoundation_iphonesimulator_framework does not exist"
    exit 1
fi

echo "Created :"
echo "  - $iOSDFULibrary_iphoneos_framework"
echo "  - $ZIPFoundation_iphoneos_framework"
echo "  - $iOSDFULibrary_iphonesimulator_framework"
echo "  - $ZIPFoundation_iphonesimulator_framework"

echo ""
echo "### LIPO / CREATE FAT LIBRARY ###"
echo ""

rm -rf $nuget_frameworks_folder
cp -a $(dirname $iOSDFULibrary_iphoneos_framework)/. $nuget_frameworks_folder
cp -a $(dirname $ZIPFoundation_iphoneos_framework)/. $nuget_frameworks_folder

rm -rf $nuget_frameworks_folder/iOSDFULibrary.framework/iOSDFULibrary
lipo -create -output $nuget_frameworks_folder/iOSDFULibrary.framework/iOSDFULibrary $iOSDFULibrary_iphoneos_framework/iOSDFULibrary $iOSDFULibrary_iphonesimulator_framework/iOSDFULibrary
lipo -info $nuget_frameworks_folder/iOSDFULibrary.framework/iOSDFULibrary

# TODO : Create Laerdal.Xamarin.ZipFoundation.iOS
#rm -rf $nuget_frameworks_folder/ZIPFoundation.framework/ZIPFoundation
#lipo -create -output $nuget_frameworks_folder/ZIPFoundation.framework/ZIPFoundation $ZIPFoundation_iphoneos_framework/ZIPFoundation $ZIPFoundation_iphonesimulator_framework/ZIPFoundation
lipo -info $nuget_frameworks_folder/ZIPFoundation.framework/ZIPFoundation

echo ""
echo "### SHARPIE ###"
echo ""

echo "sharpie bind -sdk iphoneos -o $sharpie_output_path -n $nuget_project_name -f $nuget_frameworks_folder/iOSDFULibrary.framework"
echo ""
#sharpie bind -sdk iphoneos -o $sharpie_output_path -n $nuget_project_name -f $nuget_frameworks_folder/iOSDFULibrary.framework

echo ""
echo "### MSBUILD ###"
echo ""

rm -rf $nuget_project_folder/bin
rm -rf $nuget_project_folder/obj
msbuild $nuget_csproj_path -t:Rebuild -restore:True -p:Configuration=Release -p:PackageVersion=$build_version

if [ -f "$nuget_output_file" ]; then
    echo ""
    echo "### SUCCESS ###"
    echo ""
else
    echo ""
    echo "### FAILED ###"
    echo ""
    exit 1
fi