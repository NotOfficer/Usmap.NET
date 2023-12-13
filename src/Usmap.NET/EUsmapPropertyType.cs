﻿namespace Usmap.NET
{
	public enum EUsmapPropertyType : byte
	{
		ByteProperty,
		BoolProperty,
		IntProperty,
		FloatProperty,
		ObjectProperty,
		NameProperty,
		DelegateProperty,
		DoubleProperty,
		ArrayProperty,
		StructProperty,
		StrProperty,
		TextProperty,
		InterfaceProperty,
		MulticastDelegateProperty,
		WeakObjectProperty,  //
		LazyObjectProperty,  // When deserialized, these 3 properties will be SoftObjects
		AssetObjectProperty, //
		SoftObjectProperty,
		UInt64Property,
		UInt32Property,
		UInt16Property,
		Int64Property,
		Int16Property,
		Int8Property,
		MapProperty,
		SetProperty,
		EnumProperty,
		FieldPathProperty,
		OptionalProperty,

		Unknown = byte.MaxValue
	}
}