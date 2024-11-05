using System.Diagnostics;
using System.Text.Json.Serialization;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure;

/// <summary>
/// Class describing an initial satelite state in the rotation core.
/// </summary>
[DebuggerDisplay("{Id}")]
public class SateliteDefinition
{
	#region ctor(s)

	/// <summary>
	/// Create a new satelite.
	/// </summary>
	/// <param name="id">Unique id of the satelite in the RotationCore.</param>
	/// <param name="initialPosition">Initial position vector of the block in the RotationCore.</param>
	[JsonConstructor]
	public SateliteDefinition(string id, Cartesian3dCoordinate initialPosition)
	{
		ArgumentNullException.ThrowIfNull(id);

		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Id cannot be an empty string.", nameof(id));

		this.Id = id;
		this.InitialPosition = initialPosition;
	}

	#endregion ctor(s)

	#region Public Properties

	/// <summary>
	/// Initial Position of this object.
	/// </summary>
	/// <remarks>
	/// Position is stored using the direction relative to the Form center.
	/// </remarks>
	public Cartesian3dCoordinate InitialPosition { get; }

	/// <summary>
	/// Gets the unique ID of the block.
	/// </summary>
	public string Id { get; }

	#endregion Public Properties
}
