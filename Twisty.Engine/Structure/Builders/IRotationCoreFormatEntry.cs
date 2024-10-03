﻿namespace Twisty.Engine.Structure.Builders;

/// <summary>
/// Enum describing the type for RotationCoreFormat.
/// </summary>
public enum RotationCoreFormatEntryType
{
	None,
	Load,
	Union
}

/// <summary>
/// Interface allowing to define a format entry for a RotationCore.
/// </summary>
public interface IRotationCoreFormatEntry
{
	/// <summary>
	///	Type identifier that will allow to know the uniquely identify the format type.
	/// </summary>
	RotationCoreFormatEntryType Type { get; }
}
