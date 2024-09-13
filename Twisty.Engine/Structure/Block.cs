using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Twisty.Engine.Geometry;
using Twisty.Engine.Geometry.Rotations;

namespace Twisty.Engine.Structure;

/// <summary>
/// Class describing a block moving around the rotation core.
/// </summary>
[DebuggerDisplay("{Id}")]
public class Block : IPositionnedByCartesian3dVector
{
	private readonly IReadOnlyCollection<BlockFace> m_Faces;

	#region ctor(s)

	/// <summary>
	/// Create a new block proposing a single BlockFace.
	/// </summary>
	/// <param name="id">Unique id of the block in the cube.</param>
	/// <param name="initialPosition">Initial position vector of the block in the cube.</param>
	/// <param name="face">only available BlockFace.</param>
	public Block(string id, Cartesian3dCoordinate initialPosition, BlockFace face)
	{
		if (id is null)
			throw new ArgumentNullException(nameof(id), "Id is mandatory.");
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Id cannot be an empty string.", nameof(id));

		if (face is null)
			throw new ArgumentNullException(nameof(face), "A block need at least one visible BlockFace");

		m_Faces = [face];
		this.Id = id;
		this.InitialPosition = initialPosition;
		this.Orientation = RotationMatrix3d.Unrotated;
	}

	/// <summary>
	/// Create a new block proposing multiple faces.
	/// </summary>
	/// <param name="id">Unique id of the block in the cube.</param>
	/// <param name="initialPosition">Initial position vector of the block in the cube.</param>
	/// <param name="faces">Collection of available faces for this block.</param>
	public Block(string id, Cartesian3dCoordinate initialPosition, params BlockFace[] faces)
	{
		if (id is null)
			throw new ArgumentNullException(nameof(id), "Id is mandatory.");
		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Id cannot be an empty string.", nameof(id));

		if (faces is null)
			throw new ArgumentNullException(nameof(faces), "A block need at least one visible BlockFace");

		m_Faces = new List<BlockFace>(faces.OrderBy((f) => f.Id));
		if (m_Faces.Count == 0)
			throw new ArgumentException("A block need at least one visible BlockFace", nameof(faces));

		this.Id = id;
		this.InitialPosition = initialPosition;
		this.Orientation = RotationMatrix3d.Unrotated;
	}

	#endregion ctor(s)

	#region Public Properties

	/// <summary>
	/// Initial Position is stored using the direction relative to the Form center.
	/// </summary>
	public Cartesian3dCoordinate InitialPosition { get; init; }

	/// <summary>
	/// Position is stored using the direction relative to the Form center.
	/// </summary>
	public Cartesian3dCoordinate Position => this.Orientation.Rotate(this.InitialPosition);

	/// <summary>
	/// Current Orientation of the block.
	/// </summary>
	public RotationMatrix3d Orientation { get; private set; }

	/// <summary>
	/// Gets the unique ID of the block.
	/// </summary>
	public string Id { get; }

	/// <summary>
	/// Gets the faces visibles for this block, ordered per face id.
	/// </summary>
	public IReadOnlyCollection<BlockFace> Faces => m_Faces;

	/// <summary>
	/// Gets the TopologicId for this block that will uniquely describe the block structure, ignoring orientation of faces colors.
	/// </summary>
	public string TopologicId
	{
		get
		{
			var list = this.m_Faces.Select(f => f.Position).ToList();
			var plane = new Plane(this.InitialPosition, Cartesian3dCoordinate.Zero);
			list.Sort(new CircularVectorComparer(plane));

			string[] idParts = new string[list.Count * 2 - 1];

			idParts[0] = double.Round(plane.GetThetaTo(list[0]) * 100.0).ToString();

			for (int i = 1; i < list.Count; i++)
			{
				idParts[i * 2 - 1] = double.Round(list[i - 1].GetThetaTo(list[i]) * 100.0).ToString();
				idParts[i * 2] = double.Round(plane.GetThetaTo(list[i]) * 100.0).ToString();
			}

			return string.Join('-', idParts);
		}
	}

	#endregion Public Properties

	#region Public Methods

	/// <summary>
	/// Rotate this block around the provided axis.
	/// </summary>
	/// <param name="axis">Vector indicating the axis of rotation around which the block will be rotated.</param>
	/// <param name="theta">Angle of the rotation in radians.</param>
	public void RotateAround(Cartesian3dCoordinate axis, double theta)
		=> this.Orientation = this.Orientation.Rotate(new RotationMatrix3d(axis, theta));

	/// <summary>
	/// Gets a block face based on its orientation.
	/// </summary>
	/// <param name="o">Vector indicating the orientation of the face we try to get.</param>
	/// <returns>The searched Blockface if found, otherwise, null is returned.</returns>
	public BlockFace GetBlockFace(SphericalVector o)
		=> m_Faces.FirstOrDefault(f => this.Orientation.Rotate(f.Position).IsSameVector(CoordinateConverter.ConvertToCartesian(o)));

	/// <summary>
	/// Gets a block face based on its orientation.
	/// </summary>
	/// <param name="cc">Vector indicating the orientation of the face we try to get.</param>
	/// <returns>The searched Blockface if found, otherwise, null is returned.</returns>
	public BlockFace GetBlockFace(Cartesian3dCoordinate cc)
		=> m_Faces.FirstOrDefault(f => this.Orientation.Rotate(f.Position).IsSameVector(cc));

	/// <summary>
	/// Gets a block face based on its Id.
	/// </summary>
	/// <param name="id">Id of the face we try to get.</param>
	/// <returns>The searched Blockface if found, otherwise, null is returned.</returns>
	public BlockFace GetBlockFace(string id)
	{
		if (id is null)
			throw new ArgumentNullException(nameof(id), "Id is mandatory");

		return m_Faces.FirstOrDefault(f => f.Id == id);
	}

	#endregion Public Methods
}