using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Twisty.Engine.Geometry;
using Twisty.Engine.Geometry.Rotations;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class describing a block moving around the rotation core.
	/// </summary>
	[DebuggerDisplay("{Id}")]
	public abstract class Block : IPositionnedByCartesian3dVector
	{
		private List<BlockFace> m_Faces;

		/// <summary>
		/// Create a new block proposing a single BlockFace.
		/// </summary>
		/// <param name="face">only available BlockFace.</param>
		public Block(BlockFace face)
		{
			if (face == null)
				throw new ArgumentNullException("A block need at least one visible BlockFace", nameof(face));

			m_Faces = new List<BlockFace>();
			m_Faces.Add(face);

			this.Orientation = new RotationMatrix3d();
		}

		/// <summary>
		/// Create a new block proposing multiple faces.
		/// </summary>
		/// <param name="faces">Collection of available faces for this block.</param>
		public Block(IEnumerable<BlockFace> faces)
		{
			if (faces == null)
				throw new ArgumentNullException("A block need at least one visible BlockFace", nameof(faces));

			m_Faces = new List<BlockFace>(faces);
			if (m_Faces.Count == 0)
				throw new ArgumentException("A block need at least one visible BlockFace", nameof(faces));

			this.Orientation = new RotationMatrix3d();
		}

		#region Public Properties

		/// <summary>
		/// Position is stored using the direction relative to the Form center.
		/// </summary>
		public Cartesian3dCoordinate Position { get; set; }

		/// <summary>
		/// Current Orientation of the block.
		/// </summary>
		public RotationMatrix3d Orientation { get; set; }

		/// <summary>
		/// Gets the unique ID of the block.
		/// </summary>
		public abstract string Id { get; }

		/// <summary>
		/// Gets the faces visibles for this block.
		/// </summary>
		public IEnumerable<BlockFace> Faces => m_Faces;

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Rotate this block around the provided axis.
		/// </summary>
		/// <param name="axis">Vector indicating the axis of rotation around which the block will be rotated.</param>
		/// <param name="theta">Angle of the rotation in radians.</param>
		public void RotateAround(Cartesian3dCoordinate axis, double theta) => this.Orientation = this.Orientation.Rotate(new RotationMatrix3d(axis, theta));

		/// <summary>
		/// Gets a block face based on its orientation.
		/// </summary>
		/// <param name="o">Vector indicating the orientation of the face we try to get.</param>
		/// <returns>The searched Blockface if found, otherwise, null is returned.</returns>
		public BlockFace GetBlockFace(SphericalVector o)
		{
			if (o == null)
				throw new ArgumentNullException("Orientation is mandatory", nameof(o));

			return m_Faces.FirstOrDefault(f => this.Orientation.Rotate(f.Position).IsSameVector(CoordinateConverter.ConvertToCartesian(o)));
		}

		/// <summary>
		/// Gets a block face based on its orientation.
		/// </summary>
		/// <param name="cc">Vector indicating the orientation of the face we try to get.</param>
		/// <returns>The searched Blockface if found, otherwise, null is returned.</returns>
		public BlockFace GetBlockFace(Cartesian3dCoordinate cc) => m_Faces.FirstOrDefault(f => this.Orientation.Rotate(f.Position).IsSameVector(cc));

		/// <summary>
		/// Gets a block face based on its Id.
		/// </summary>
		/// <param name="id">Id of the face we try to get.</param>
		/// <returns>The searched Blockface if found, otherwise, null is returned.</returns>
		public BlockFace GetBlockFace(string id)
		{
			if (id == null)
				throw new ArgumentNullException("Id is mandatory", nameof(id));

			return m_Faces.FirstOrDefault(f => f.Id == id);
		}

		#endregion Public Methods
	}
}