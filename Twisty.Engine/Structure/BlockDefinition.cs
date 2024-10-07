using System.Diagnostics;
using System.Text.Json.Serialization;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure;

/// <summary>
/// Class describing an initial block state in the rotation core.
/// </summary>
[DebuggerDisplay("{Id}")]
public class BlockDefinition
{
	private readonly IReadOnlyCollection<BlockFace> m_Faces;

	#region ctor(s)

	/// <summary>
	/// Create a new block proposing multiple faces.
	/// </summary>
	/// <param name="id">Unique id of the block in the cube.</param>
	/// <param name="initialPosition">Initial position vector of the block in the cube.</param>
	/// <param name="faces">Collection of available faces for this block.</param>
	public BlockDefinition(string id, Cartesian3dCoordinate initialPosition, params BlockFace[] faces)
	{
		ArgumentNullException.ThrowIfNull(id);
		ArgumentNullException.ThrowIfNull(faces);

		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Id cannot be an empty string.", nameof(id));

		m_Faces = new List<BlockFace>(faces.Where(f => f is not null).OrderBy((f) => f.Id));
		if (m_Faces.Count == 0)
			throw new ArgumentException("A block need at least one visible BlockFace", nameof(faces));

		this.Id = id;
		this.InitialPosition = initialPosition;
	}

	/// <summary>
	/// Create a new block proposing multiple faces.
	/// </summary>
	/// <param name="id">Unique id of the block in the cube.</param>
	/// <param name="initialPosition">Initial position vector of the block in the cube.</param>
	/// <param name="faces">Collection of available faces for this block.</param>
	[JsonConstructor]
	public BlockDefinition(string id, Cartesian3dCoordinate initialPosition, IReadOnlyCollection<BlockFace> faces)
	{
		ArgumentNullException.ThrowIfNull(id);
		ArgumentNullException.ThrowIfNull(faces);

		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Id cannot be an empty string.", nameof(id));

		m_Faces = new List<BlockFace>(faces.Where(f => f is not null).OrderBy((f) => f.Id));
		if (m_Faces.Count == 0)
			throw new ArgumentException("A block need at least one visible BlockFace", nameof(faces));

		this.Id = id;
		this.InitialPosition = initialPosition;
	}

	#endregion ctor(s)

	#region Public Properties

	/// <summary>
	/// Initial Position is stored using the direction relative to the Form center.
	/// </summary>
	public Cartesian3dCoordinate InitialPosition { get; }

	/// <summary>
	/// Gets the unique ID of the block.
	/// </summary>
	public string Id { get; }

	/// <summary>
	/// Gets the faces visibles for this block, ordered per face id.
	/// </summary>
	public IReadOnlyCollection<BlockFace> Faces => m_Faces;

	#endregion Public Properties
}
