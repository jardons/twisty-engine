using System;
using System.Collections.Generic;
using System.Linq;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class describing the object that will represent the central rotation point around which blocks will rotate.
	/// </summary>
	public abstract class RotationCore : IRotable
	{
		#region Private Members

		private List<Block> m_Blocks;

		private Dictionary<string, RotationAxis> m_Axes;

		private Dictionary<string, CoreFace> m_Faces;

		#endregion Private Members

		#region ctor(s)

		/// <summary>
		/// Create a new RotationCore object
		/// </summary>
		/// <param name="blocks">List of blocks available around the center of the RotationCore.</param>
		/// <param name="axes">List of Rotation Axes proposed around the rotation core.</param>
		/// <param name="faces">List of faces proposed for this RotationCore in his solved state.</param>
		protected RotationCore(IEnumerable<Block> blocks, IEnumerable<RotationAxis> axes, IEnumerable<CoreFace> faces)
		{
			m_Blocks = new List<Block>(blocks);
			m_Axes = new Dictionary<string, RotationAxis>();
			foreach (var axis in axes)
				m_Axes.Add(axis.Id, axis);

			m_Faces = new Dictionary<string, CoreFace>();
			foreach (var f in faces)
				m_Faces.Add(f.Id, f);
		}

		#endregion ctor(s)

		#region Public Properties

		/// <summary>
		/// Gets the list of blocks available in this core.
		/// </summary>
		public IEnumerable<Block> Blocks => m_Blocks;

		/// <summary>
		/// Gets the list of axes available for rotation around this core.
		/// </summary>
		public IEnumerable<RotationAxis> Axes => m_Axes.Values;

		/// <summary>
		/// Gets the list of faces available for rotation around this core.
		/// </summary>
		public IEnumerable<CoreFace> Faces => m_Faces.Values;

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// Get the Blocks for a specific face of the twisty puzzle based on the current blocks positions.
		/// </summary>
		/// <param name="faceId">Orientation of the face of the puzzle.</param>
		/// <returns>The list of blocks currently visible on the requested face.</returns>
		/// <remarks>Logic is only valid for standard forms. Any shapeshifting form would need to improve this logic.</remarks>
		public IEnumerable<Block> GetBlocksForFace(string faceId)
		{
			if (!m_Faces.ContainsKey(faceId))
				throw new ArgumentNullException("Face Id should exist.", nameof(faceId));

			return m_Blocks.Where(b => b.GetBlockFace(m_Faces[faceId].Plane.Normal) != null);
		}

		/// <summary>
		/// Get the Blocks for a specific face of the twisty puzzle based on the current blocks positions.
		/// </summary>
		/// <param name="v">Orientation of the face of the puzzle.</param>
		/// <returns>The list of blocks currently visible on the requested face.</returns>
		/// <remarks>Logic is only valid for standard forms. Any shapeshifting form would need to improve this logic.</remarks>
		public IEnumerable<Block> GetBlocksForFace(Cartesian3dCoordinate v) => m_Blocks.Where(b => b.GetBlockFace(v) != null);

		/// <summary>
		/// Gets an axis using its id.
		/// </summary>
		/// <param name="axisId">Id of the axis we are looking up.</param>
		/// <returns>Axis for the corresponding or null if not found.</returns>
		public RotationAxis GetAxis(string axisId) => m_Axes[axisId];

		/// <summary>
		/// Gets a block using its id.
		/// </summary>
		/// <param name="blockId">Id of the block we are looking up.</param>
		/// <returns>Block for the corresponding or null if not found.</returns>
		public Block GetBlock(string blockId) => m_Blocks.FirstOrDefault(b => b.Id == blockId);

		/// <summary>
		/// Gets a face using its id.
		/// </summary>
		/// <param name="faceId">Id of the face we are looking up.</param>
		/// <returns>Face for the corresponding or null if not found.</returns>
		public CoreFace GetFace(string faceId) => m_Faces[faceId];

		/// <summary>
		/// Rotate a face around a specified rotation axis.
		/// </summary>
		/// <param name="axis">Rotation axis aroung which the rotation will be executed.</param>
		/// <param name="theta">Angle of the rotation to execute.</param>
		/// <param name="distance">
		/// Distance of the center above which blocks will be rotated.
		/// If null, All blocks around the axis are rotated.
		/// </param>
		public void RotateAround(RotationAxis axis, double theta, LayerSeparator aboveLayer = null)
		{
			if (axis == null)
				throw new ArgumentNullException(nameof(axis));

			if (aboveLayer == null)
				aboveLayer = axis.GetUpperLayer();

			var blocks = GetBlocksAbove(aboveLayer.Plane);
			this.Rotate(blocks, axis.Vector, theta);
		}

		#endregion Public Methods

		#region Private Members

		/// <summary>
		/// Gets all blocks of the core above a specific plane.
		/// </summary>
		/// <param name="p">Plane above which the selected blocks should be present.</param>
		/// <returns>Collection of blocks above the provided Plane.</returns>
		private IEnumerable<Block> GetBlocksAbove(Plane p)
			=> this.Blocks.Where(b => p.IsAbovePlane(b.Position));

		/// <summary>
		/// Gets all blocks of the core between two specific plane.
		/// </summary>
		/// <param name="above">Plane above which the selected blocks should be present.</param>
		/// <param name="below">Plane below which the selected blocks should be present.</param>
		/// <returns>Collection of blocks between both the provided Planes.</returns>
		private IEnumerable<Block> GetBlocksBetween(IPlanar above, IPlanar below)
			=> this.Blocks.Where(b => below.Plane.IsAbovePlane(b.Position) && above.Plane.IsBelowPlane(b.Position));

		/// <summary>
		/// Perform the rotations of blocks around the axis.
		/// </summary>
		/// <typeparam name="T">Type of blocks available in the collection to rotate.</typeparam>
		/// <param name="blocks">Collection for which position will be rotated.</param>
		/// <param name="rotationAxis">Axis used for the rotation of the blocks.</param>
		/// <param name="theta">Angle in radians of the rotations to execute on each blocks.</param>
		private void Rotate(IEnumerable<Block> blocks, Cartesian3dCoordinate rotationAxis, double theta)
		{
			if (theta.IsZero())
				return;

			// Rotate the block aroung themselve.
			foreach (var b in blocks)
				b.RotateAround(rotationAxis, theta);
		}

		#endregion Private Members
	}
}