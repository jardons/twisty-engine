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
	#region ctor(s)

	/// <summary>
	/// Create a new block.
	/// </summary>
	/// <param name="definition">Definition of this block initial state.</param>
	public Block(BlockDefinition definition)
	{
		ArgumentNullException.ThrowIfNull(definition);

		Definition = definition;
		Orientation = RotationMatrix3d.Unrotated;
	}

	#endregion ctor(s)

	#region Public Properties

	public BlockDefinition Definition { get; }

	/// <summary>
	/// Position is stored using the direction relative to the Form center.
	/// </summary>
	public Cartesian3dCoordinate Position => this.Orientation.Rotate(this.Definition.InitialPosition);

	/// <summary>
	/// Current Orientation of the block.
	/// </summary>
	public RotationMatrix3d Orientation { get; private set; }

	/// <summary>
	/// Gets the unique ID of the block.
	/// </summary>
	public string Id => Definition.Id;

	/// <summary>
	/// Gets the bandage related to this Block.
	/// </summary>
	public Bandage Bandage { get; set; }

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
		=> Definition.Faces.FirstOrDefault(f => this.Orientation.Rotate(f.Position).IsSameVector(CoordinateConverter.ConvertToCartesian(o)));

	/// <summary>
	/// Gets a block face based on its orientation.
	/// </summary>
	/// <param name="cc">Vector indicating the orientation of the face we try to get.</param>
	/// <returns>The searched Blockface if found, otherwise, null is returned.</returns>
	public BlockFace GetBlockFace(Cartesian3dCoordinate cc)
		=> Definition.Faces.FirstOrDefault(f => this.Orientation.Rotate(f.Position).IsSameVector(cc));

	/// <summary>
	/// Gets a block face based on its Id.
	/// </summary>
	/// <param name="id">Id of the face we try to get.</param>
	/// <returns>The searched Blockface if found, otherwise, null is returned.</returns>
	public BlockFace GetBlockFace(string id)
	{
		if (id is null)
			throw new ArgumentNullException(nameof(id), "Id is mandatory");

		return Definition.Faces.FirstOrDefault(f => f.Id == id);
	}

	#endregion Public Methods
}