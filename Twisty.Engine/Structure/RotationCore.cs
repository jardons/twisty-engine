using System;
using System.Collections.Generic;
using System.Linq;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class describing the object that will represent the central rotation point around which blocks will rotate.
	/// </summary>
	public abstract class RotationCore
	{
		#region Private Members

		private List<Block> m_Blocks;

		private Dictionary<string, RotationAxis> m_Axes;

		private Dictionary<string, CoreFace> m_Faces;

		#endregion Private Members

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

		#region Public Properties

		/// <summary>
		/// Gets the list of blocks available in this core.
		/// </summary>
		public IEnumerable<Block> Blocks => m_Blocks;

		/// <summary>
		/// Gets the list of axes available for rotation around this core.
		/// </summary>
		public IEnumerable<RotationAxis> Axes => m_Axes.Values;

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

			return m_Blocks.Where(b => b.GetBlockFace(m_Faces[faceId].Coordinates.Normal) != null);
		}

		/// <summary>
		/// Get the Blocks for a specific face of the twisty puzzle based on the current blocks positions.
		/// </summary>
		/// <param name="v">Orientation of the face of the puzzle.</param>
		/// <returns>The list of blocks currently visible on the requested face.</returns>
		/// <remarks>Logic is only valid for standard forms. Any shapeshifting form would need to improve this logic.</remarks>
		public IEnumerable<Block> GetBlocksForFace(SphericalVector v)
		{
			if (v == null)
				throw new ArgumentNullException("Orientation is mandatory", nameof(v));

			return m_Blocks.Where(b => b.GetBlockFace(v) != null);
		}

		/// <summary>
		/// Gets an axis using its id.
		/// </summary>
		/// <param name="axisId">Id of the axis we are looking up.</param>
		/// <returns>Axis for the corresponding or null if not found.</returns>
		public RotationAxis GetAxis(string axisId) => m_Axes[axisId];

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		/// Perform the switch of positions of blocks and their rotation on themselve.
		/// </summary>
		/// <typeparam name="T">Type of blocks available in the collection to switch and rotate.</typeparam>
		/// <param name="blocks">Sorted collection for which position will be switched. Each block will take the position of the next one.</param>
		/// <param name="rotationAxis">Axis used for the rotation of the blocks.</param>
		/// <param name="theta">Angle in radians of the rotations to execute on each blocks.</param>
		protected void SwitchAndRotate<T>(IList<T> blocks, Cartesian3dCoordinate rotationAxis, double theta)
			where T : Block
		{
			// No switch to perform if their is not at least 2 blocks.
			if (blocks.Count > 1)
			{
				// Store position of the first block that need to be set to the last one.
				var firstPosition = blocks[0].Position;

				// Update all intermediate blocks positions.
				for (int i = 0; i < blocks.Count - 1; ++i)
					blocks[i].Position = blocks[i + 1].Position;

				// Finalize the switch by replacing the position of the last one with the one from the first one.
				blocks[blocks.Count - 1].Position = firstPosition;
			}

			if (!theta.IsZero())
			{
				// Rotate the block aroung themselve.
				foreach (var b in blocks)
					b.RotateAround(rotationAxis, theta);
			}
		}

		#endregion Protected Methods
	}
}