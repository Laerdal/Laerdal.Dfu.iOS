﻿using System;
using CoreBluetooth;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace Laerdal.Xamarin.Dfu.iOS
{
	// @interface DFUFirmware : NSObject
	[BaseType (typeof(NSObject)), DisableDefaultCtor]
	interface DFUFirmware
	{
		// @property (readonly, copy, nonatomic) NSString * _Nullable fileName;
		[NullAllowed, Export ("fileName")]
		string FileName { get; }

		// @property (readonly, copy, nonatomic) NSURL * _Nullable fileUrl;
		[NullAllowed, Export ("fileUrl", ArgumentSemantic.Copy)]
		NSUrl FileUrl { get; }

		// @property (readonly, nonatomic) BOOL valid;
		[Export ("valid")]
		bool Valid { get; }

		// @property (readonly, nonatomic, strong) DFUFirmwareSize * _Nonnull size;
		[Export ("size", ArgumentSemantic.Strong)]
		DFUFirmwareSize Size { get; }

		// @property (readonly, nonatomic) NSInteger parts;
		[Export ("parts")]
		nint Parts { get; }

		// -(instancetype _Nullable)initWithUrlToZipFile:(NSURL * _Nonnull)urlToZipFile;
		[Export ("initWithUrlToZipFile:")]
		IntPtr Constructor (NSUrl urlToZipFile);

		// -(instancetype _Nullable)initWithUrlToZipFile:(NSURL * _Nonnull)urlToZipFile type:(enum DFUFirmwareType)type __attribute__((objc_designated_initializer));
		[Export ("initWithUrlToZipFile:type:")]
		[DesignatedInitializer]
		IntPtr Constructor (NSUrl urlToZipFile, DFUFirmwareType type);

		// -(instancetype _Nullable)initWithZipFile:(NSData * _Nonnull)zipFile;
		[Export ("initWithZipFile:")]
		IntPtr InitWithZipFile (NSData zipFile);

		// -(instancetype _Nullable)initWithZipFile:(NSData * _Nonnull)zipFile type:(enum DFUFirmwareType)type __attribute__((objc_designated_initializer));
		[Export ("initWithZipFile:type:")]
		[DesignatedInitializer]
		IntPtr InitWithZipFile (NSData zipFile, DFUFirmwareType type);

		// -(instancetype _Nullable)initWithUrlToBinOrHexFile:(NSURL * _Nonnull)urlToBinOrHexFile urlToDatFile:(NSURL * _Nullable)urlToDatFile type:(enum DFUFirmwareType)type __attribute__((objc_designated_initializer));
		[Export ("initWithUrlToBinOrHexFile:urlToDatFile:type:")]
		[DesignatedInitializer]
		IntPtr InitWithUrlToBinOrHexFile (NSUrl urlToBinOrHexFile, [NullAllowed] NSUrl urlToDatFile, DFUFirmwareType type);

		// -(instancetype _Nullable)initWithBinFile:(NSData * _Nonnull)binFile datFile:(NSData * _Nullable)datFile type:(enum DFUFirmwareType)type __attribute__((objc_designated_initializer));
		[Export ("initWithBinFile:datFile:type:")]
		[DesignatedInitializer]
		IntPtr InitWithBinFile (NSData binFile, [NullAllowed] NSData datFile, DFUFirmwareType type);

		// -(instancetype _Nullable)initWithHexFile:(NSData * _Nonnull)hexFile datFile:(NSData * _Nullable)datFile type:(enum DFUFirmwareType)type __attribute__((objc_designated_initializer));
		[Export ("initWithHexFile:datFile:type:")]
		[DesignatedInitializer]
		IntPtr InitWithHexFile (NSData hexFile, [NullAllowed] NSData datFile, DFUFirmwareType type);
	}

	// @interface DFUFirmwareSize : NSObject
	[BaseType (typeof(NSObject)), DisableDefaultCtor]
	interface DFUFirmwareSize
	{
		// @property (readonly, nonatomic) uint32_t softdevice;
		[Export ("softdevice")]
		uint Softdevice { get; }

		// @property (readonly, nonatomic) uint32_t bootloader;
		[Export ("bootloader")]
		uint Bootloader { get; }

		// @property (readonly, nonatomic) uint32_t application;
		[Export ("application")]
		uint Application { get; }
	}

	// @protocol DFUPeripheralSelectorDelegate
	[Protocol, Model, BaseType (typeof(NSObject))]
	interface DFUPeripheralSelectorDelegate
	{
		// @required -(BOOL)select:(CBPeripheral * _Nonnull)peripheral advertisementData:(NSDictionary<NSString *,id> * _Nonnull)advertisementData RSSI:(NSNumber * _Nonnull)RSSI hint:(NSString * _Nullable)name __attribute__((warn_unused_result));
		[Abstract]
		[Export ("select:advertisementData:RSSI:hint:")]
		bool Select (CBPeripheral peripheral, NSDictionary<NSString, NSObject> advertisementData, NSNumber RSSI, [NullAllowed] string name);

		// @required -(NSArray<CBUUID *> * _Nullable)filterByHint:(CBUUID * _Nonnull)dfuServiceUUID __attribute__((warn_unused_result));
		[Abstract]
		[Export ("filterByHint:")]
		[return: NullAllowed]
		CBUUID[] FilterByHint (CBUUID dfuServiceUUID);
	}

	// @interface DFUPeripheralSelector : NSObject <DFUPeripheralSelectorDelegate>
	[BaseType (typeof(NSObject))]
	interface DFUPeripheralSelector
	{
		// -(BOOL)select:(CBPeripheral * _Nonnull)peripheral advertisementData:(NSDictionary<NSString *,id> * _Nonnull)advertisementData RSSI:(NSNumber * _Nonnull)RSSI hint:(NSString * _Nullable)name __attribute__((warn_unused_result));
		[Export ("select:advertisementData:RSSI:hint:")]
		bool Select (CBPeripheral peripheral, NSDictionary<NSString, NSObject> advertisementData, NSNumber RSSI, [NullAllowed] string name);

		// -(NSArray<CBUUID *> * _Nullable)filterByHint:(CBUUID * _Nonnull)dfuServiceUUID __attribute__((warn_unused_result));
		[Export ("filterByHint:")]
		[return: NullAllowed]
		CBUUID[] FilterByHint (CBUUID dfuServiceUUID);
	}

	// @protocol DFUProgressDelegate
	[Protocol, Model, BaseType (typeof(NSObject))]
	interface DFUProgressDelegate
	{
		// @required -(void)dfuProgressDidChangeFor:(NSInteger)part outOf:(NSInteger)totalParts to:(NSInteger)progress currentSpeedBytesPerSecond:(double)currentSpeedBytesPerSecond avgSpeedBytesPerSecond:(double)avgSpeedBytesPerSecond;
		[Abstract]
		[Export ("dfuProgressDidChangeFor:outOf:to:currentSpeedBytesPerSecond:avgSpeedBytesPerSecond:")]
		void OutOf (nint part, nint totalParts, nint progress, double currentSpeedBytesPerSecond, double avgSpeedBytesPerSecond);
	}

	// @interface DFUServiceController : NSObject
	[BaseType (typeof(NSObject)), DisableDefaultCtor]
	interface DFUServiceController
	{
		// -(void)pause;
		[Export ("pause")]
		void Pause ();

		// -(void)resume;
		[Export ("resume")]
		void Resume ();

		// -(BOOL)abort __attribute__((warn_unused_result));
		[Export ("abort")]
		bool Abort ();

		// -(void)restart;
		[Export ("restart")]
		void Restart ();

		// @property (readonly, nonatomic) BOOL paused;
		[Export ("paused")]
		bool Paused { get; }

		// @property (readonly, nonatomic) BOOL aborted;
		[Export ("aborted")]
		bool Aborted { get; }
	}

	// @protocol DFUServiceDelegate
	[Protocol, Model, BaseType (typeof(NSObject))]
	interface DFUServiceDelegate
	{
		// @required -(void)dfuStateDidChangeTo:(enum DFUState)state;
		[Abstract]
		[Export ("dfuStateDidChangeTo:")]
		void DfuStateDidChangeTo (DFUState state);

		// @required -(void)dfuError:(enum DFUError)error didOccurWithMessage:(NSString * _Nonnull)message;
		[Abstract]
		[Export ("dfuError:didOccurWithMessage:")]
		void DfuError (DFUError error, string message);
	}

	// @interface DFUServiceInitiator : NSObject
	[BaseType (typeof(NSObject)), DisableDefaultCtor]
	interface DFUServiceInitiator
	{
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		DFUServiceDelegate Delegate { get; set; }

		// @property (nonatomic, weak) id<DFUServiceDelegate> _Nullable delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakProgressDelegate")]
		[NullAllowed]
		DFUProgressDelegate ProgressDelegate { get; set; }

		// @property (nonatomic, weak) id<DFUProgressDelegate> _Nullable progressDelegate;
		[NullAllowed, Export ("progressDelegate", ArgumentSemantic.Weak)]
		NSObject WeakProgressDelegate { get; set; }

		// @property (nonatomic, weak) id<LoggerDelegate> _Nullable logger;
		[NullAllowed, Export ("logger", ArgumentSemantic.Weak)]
		LoggerDelegate Logger { get; set; }

		// @property (nonatomic, strong) id<DFUPeripheralSelectorDelegate> _Nonnull peripheralSelector;
		[Export ("peripheralSelector", ArgumentSemantic.Strong)]
		DFUPeripheralSelectorDelegate PeripheralSelector { get; set; }

		// @property (nonatomic) uint16_t packetReceiptNotificationParameter;
		[Export ("packetReceiptNotificationParameter")]
		ushort PacketReceiptNotificationParameter { get; set; }

		// @property (nonatomic) BOOL forceDfu;
		[Export ("forceDfu")]
		bool ForceDfu { get; set; }

		// @property (nonatomic) BOOL forceScanningForNewAddressInLegacyDfu;
		[Export ("forceScanningForNewAddressInLegacyDfu")]
		bool ForceScanningForNewAddressInLegacyDfu { get; set; }

		// @property (nonatomic) NSTimeInterval connectionTimeout;
		[Export ("connectionTimeout")]
		double ConnectionTimeout { get; set; }

		// @property (nonatomic) NSTimeInterval dataObjectPreparationDelay;
		[Export ("dataObjectPreparationDelay")]
		double DataObjectPreparationDelay { get; set; }

		// @property (nonatomic) BOOL alternativeAdvertisingNameEnabled;
		[Export ("alternativeAdvertisingNameEnabled")]
		bool AlternativeAdvertisingNameEnabled { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable alternativeAdvertisingName;
		[NullAllowed, Export ("alternativeAdvertisingName")]
		string AlternativeAdvertisingName { get; set; }

		// @property (nonatomic) BOOL enableUnsafeExperimentalButtonlessServiceInSecureDfu;
		[Export ("enableUnsafeExperimentalButtonlessServiceInSecureDfu")]
		bool EnableUnsafeExperimentalButtonlessServiceInSecureDfu { get; set; }

		// @property (nonatomic, strong) DFUUuidHelper * _Nonnull uuidHelper;
		[Export ("uuidHelper", ArgumentSemantic.Strong)]
		DFUUuidHelper UuidHelper { get; set; }

		// @property (nonatomic) BOOL disableResume;
		[Export ("disableResume")]
		bool DisableResume { get; set; }

		// -(instancetype _Nonnull)initWithCentralManager:(CBCentralManager * _Nonnull)centralManager target:(CBPeripheral * _Nonnull)target __attribute__((deprecated("Use init(queue: DispatchQueue?) instead."))) __attribute__((objc_designated_initializer));
		[Export ("initWithCentralManager:target:")]
		[DesignatedInitializer]
		IntPtr Constructor (CBCentralManager centralManager, CBPeripheral target);

		// -(instancetype _Nonnull)initWithQueue:(dispatch_queue_t _Nullable)queue delegateQueue:(dispatch_queue_t _Nonnull)delegateQueue progressQueue:(dispatch_queue_t _Nonnull)progressQueue loggerQueue:(dispatch_queue_t _Nonnull)loggerQueue __attribute__((objc_designated_initializer));
		[Export ("initWithQueue:delegateQueue:progressQueue:loggerQueue:")]
		[DesignatedInitializer]
		IntPtr Constructor ([NullAllowed] DispatchQueue queue, DispatchQueue delegateQueue, DispatchQueue progressQueue, DispatchQueue loggerQueue);

		// -(DFUServiceInitiator * _Nonnull)withFirmware:(DFUFirmware * _Nonnull)file __attribute__((warn_unused_result));
		[Export ("withFirmware:")]
		DFUServiceInitiator WithFirmware (DFUFirmware file);

		// -(DFUServiceController * _Nullable)start __attribute__((deprecated("Use start(target: CBPeripheral) instead."))) __attribute__((warn_unused_result));
		[Export ("start")]
		DFUServiceController Start ();

		// -(DFUServiceController * _Nullable)startWithTarget:(CBPeripheral * _Nonnull)target __attribute__((warn_unused_result));
		[Export ("startWithTarget:")]
		[return: NullAllowed]
		DFUServiceController StartWithTarget (CBPeripheral target);

		// -(DFUServiceController * _Nullable)startWithTargetWithIdentifier:(NSUUID * _Nonnull)uuid __attribute__((warn_unused_result));
		[Export ("startWithTargetWithIdentifier:")]
		[return: NullAllowed]
		DFUServiceController StartWithTargetWithIdentifier (NSUuid uuid);
	}

	// @interface DFUUuid : NSObject
	[BaseType (typeof(NSObject)), DisableDefaultCtor]
	interface DFUUuid
	{
		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull uuid;
		[Export ("uuid", ArgumentSemantic.Strong)]
		CBUUID Uuid { get; }

		// @property (readonly, nonatomic) enum DFUUuidType type;
		[Export ("type")]
		DFUUuidType Type { get; }

		// -(instancetype _Nonnull)initWithUUID:(CBUUID * _Nonnull)withUUID forType:(enum DFUUuidType)forType __attribute__((objc_designated_initializer));
		[Export ("initWithUUID:forType:")]
		[DesignatedInitializer]
		IntPtr Constructor (CBUUID withUUID, DFUUuidType forType);
	}

	// @interface DFUUuidHelper : NSObject
	[BaseType (typeof(NSObject))]
	interface DFUUuidHelper
	{
		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull legacyDFUService;
		[Export ("legacyDFUService", ArgumentSemantic.Strong)]
		CBUUID LegacyDFUService { get; }

		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull legacyDFUControlPoint;
		[Export ("legacyDFUControlPoint", ArgumentSemantic.Strong)]
		CBUUID LegacyDFUControlPoint { get; }

		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull legacyDFUPacket;
		[Export ("legacyDFUPacket", ArgumentSemantic.Strong)]
		CBUUID LegacyDFUPacket { get; }

		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull legacyDFUVersion;
		[Export ("legacyDFUVersion", ArgumentSemantic.Strong)]
		CBUUID LegacyDFUVersion { get; }

		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull secureDFUService;
		[Export ("secureDFUService", ArgumentSemantic.Strong)]
		CBUUID SecureDFUService { get; }

		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull secureDFUControlPoint;
		[Export ("secureDFUControlPoint", ArgumentSemantic.Strong)]
		CBUUID SecureDFUControlPoint { get; }

		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull secureDFUPacket;
		[Export ("secureDFUPacket", ArgumentSemantic.Strong)]
		CBUUID SecureDFUPacket { get; }

		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull buttonlessExperimentalService;
		[Export ("buttonlessExperimentalService", ArgumentSemantic.Strong)]
		CBUUID ButtonlessExperimentalService { get; }

		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull buttonlessExperimentalCharacteristic;
		[Export ("buttonlessExperimentalCharacteristic", ArgumentSemantic.Strong)]
		CBUUID ButtonlessExperimentalCharacteristic { get; }

		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull buttonlessWithoutBonds;
		[Export ("buttonlessWithoutBonds", ArgumentSemantic.Strong)]
		CBUUID ButtonlessWithoutBonds { get; }

		// @property (readonly, nonatomic, strong) CBUUID * _Nonnull buttonlessWithBonds;
		[Export ("buttonlessWithBonds", ArgumentSemantic.Strong)]
		CBUUID ButtonlessWithBonds { get; }

		// -(instancetype _Nonnull)initWithCustomUuids:(NSArray<DFUUuid *> * _Nonnull)uuids;
		[Export ("initWithCustomUuids:")]
		IntPtr Constructor (DFUUuid[] uuids);
	}

	// @protocol LoggerDelegate
	[Protocol, Model, BaseType (typeof(NSObject))]
	interface LoggerDelegate
	{
		// @required -(void)logWith:(enum LogLevel)level message:(NSString * _Nonnull)message;
		[Abstract]
		[Export ("logWith:message:")]
		void Message (LogLevel level, string message);
	}
}
