namespace Twisty.Engine.Structure.Builders;

/// <summary>
/// RotationCoreFormat allowing to merge the provided values from multiple other RotationCoreFormat.
/// </summary>
/// <typeparam name="T"></typeparam>
public class RotationCoreFormatUnion<T> : IRotationCoreFormatEntry<T>
{
	/// <inheritdoc />
	public RotationCoreFormatEntryType Type => RotationCoreFormatEntryType.Union;

	/// <summary>
	/// Gets/Sets the sub-entries that will be joined together.
	/// </summary>
	public IRotationCoreFormatEntry<T>[] Entries { get; set; }

	/// <inheritdoc />
	public IEnumerable<T> GetValues(RotationCoreBuilderContext context)
		=> Entries?.SelectMany(e => e.GetValues(context)) ?? [];
}
