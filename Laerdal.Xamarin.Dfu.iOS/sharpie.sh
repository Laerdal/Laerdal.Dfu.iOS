#!/bin/bash

rm -rf Sharpie_Generated
process_header()
{
    for header_file_name in "$@"
    do
        output_folder_path="Sharpie_Generated"
        framework_name="iOSDFULibrary"

        header_folder_path="Frameworks/$framework_name.framework/Headers"
        header_file_path="$header_folder_path/$header_file_name.h"

        output_file_prefix="$header_file_name"

        namespace="Laerdal.Xamarin.Dfu.iOS" 

        echo "Sharpying $header_file_path into $output_folder_path/$output_file_prefix"

        mkdir -p $output_folder_path
        sharpie bind -n $namespace -p $output_file_prefix -sdk iphoneos -o $output_folder_path  -scope $header_folder_path/ $header_file_path
    done
}

# Generates Objective Sharp bindings for these header files in the mobileffmpeg.framework
process_header iOSDFULibrary-Swift