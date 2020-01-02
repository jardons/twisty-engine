using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure.Rubiks
{
	/// <summary>
	/// Class describing a center block in the Rubiks' cube.
	/// Center are the only place that never change of position and stay in their position on the rotation axis.
	/// </summary>
	public class RubikEdgeBlock : Block
	{
		/// <summary>
		/// Create a standard Rubik Edge.
		/// </summary>
		/// <param name="initialPosition">Initial position vector of the block in the cube.</param>
		/// <param name="face1">First Visible face of the block.</param>
		/// <param name="face2">Second Visible face of the block.</param>
		public RubikEdgeBlock(SphericalVector initialPosition, BlockFace face1, BlockFace face2)
			: base(new BlockFace[] { face1, face2 })
		{
			base.Position = initialPosition;

			// Each Edge block is unique in the cube and be identified by his face.
			Id = $"E_{face1.Id}{face2.Id}";
		}

		/// <summary>
		/// Create a standard Rubik Edge.
		/// </summary>
		/// <param name="initialPosition">Initial position vector of the block in the cube.</param>
		/// <param name="face1">First Visible face of the block.</param>
		/// <param name="face2">Second Visible face of the block.</param>
		public RubikEdgeBlock(Cartesian3dCoordinate initialPosition, BlockFace face1, BlockFace face2)
			: base(new BlockFace[] { face1, face2 })
		{
			base.Position = CoordinateConverter.ConvertToSpherical(initialPosition);

			// Each Edge block is unique in the cube and be identified by his face.
			Id = $"E_{face1.Id}{face2.Id}";
		}

		#region Block Members

		/// <summary>
		/// Gets the unique ID of the block.
		/// </summary>
		public override string Id { get; }

		#endregion Block Members
	}
}
