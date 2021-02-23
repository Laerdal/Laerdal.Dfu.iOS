# Laerdal.Xamarin.Dfu.iOS

This is an Xamarin binding library for the Nordic Semiconductors iOS library for updating the firmware of their devices over the air via Bluetooth Low Energy.
The native iOS Pod library is located here: https://github.com/NordicSemiconductor/IOS-Pods-DFU-Library

[![Build status](https://dev.azure.com/LaerdalMedical/Healthcare%20Responders/_apis/build/status/Github/Laerdal.Xamarin.Dfu.iOS)](https://dev.azure.com/LaerdalMedical/Healthcare%20Responders/_build/latest?definitionId=99)
[![NuGet Badge](https://buildstats.info/nuget/Laerdal.Xamarin.Dfu.iOS)](https://www.nuget.org/packages/Laerdal.Xamarin.Dfu.iOS/)

## Folder structure

- NordicSemiconductor/IOS-Pods-DFU-Library = Submodule containing [Nordic's code](https://github.com/NordicSemiconductor/IOS-Pods-DFU-Library)
- Laerdal.Xamarin.Dfu.iOS = Xamarin ObjectiveC Binding Library project and nuget files
- Laerdal.Xamarin.Dfu.iOS.Output = Build output from building *Laerdal.Xamarin.Dfu.iOS*

## Local build

### Requirements

You'll need :

- **MacOS**
  - with **XCode**
  - with **Xamarin.iOS** (obviously)
  - [with **ObjectiveSharpie**] :

```bash
brew cask install objectivesharpie
```

[More about Objective Sharpie](https://docs.microsoft.com/en-us/xamarin/cross-platform/macios/binding/objective-sharpie/get-started)

### Steps to build

#### 1) Checkout with submodule

```bash
git clone --recurse-submodules https://github.com/Laerdal/Laerdal.Xamarin.Dfu.iOS.git
```

Feel free to update the submodule reference / Pull to the latest release from Nordic.

#### 2) Run **make**

There is a *makefile* included that does everything for you, feel free to read it to know more.

To use it simply run :

```bash
make
```

Note :

> A fat library is simply a library with multiple architectures. In our case it will contain x86 and arm architectures. The proper name is ‘Universal Static Library’. But we will stick with ‘fat library’ since its smaller to write and that is exactly what our resultant library would be. Fat!!! with multiple architectures in it.
>
> -- <cite>[@hassanahmedkhan](https://medium.com/@hassanahmedkhan/a-noobs-guide-to-creating-a-fat-library-for-ios-bafe8452b84b)</cite>

### Clean

To clean the output files and restart the process run :

```bash
make clean
```

### Known issues

> [**Invalid Swift support when submitted to the Apple AppStore**](https://github.com/Laerdal/Laerdal.Xamarin.Dfu.iOS/issues/3)
> 
> Fix : https://github.com/Laerdal/Laerdal.Xamarin.Dfu.iOS/issues/3#issuecomment-783298581

> [**ObjCRuntime.RuntimeException: Can't register the class XXX when the dynamic registrar has been linked away"**](https://github.com/Laerdal/Laerdal.Xamarin.Dfu.iOS/issues/1)
> 
> Fix : You might need to add "--optimize=-remove-dynamic-registrar" to your apps mtouch args.

