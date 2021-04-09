﻿using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class describing a center block in the cube.
	/// Center are the only place that never change of position and stay in their position on the rotation axis.
	/// </summary>
	public class EdgeBlock : Block
	{
		/// <summary>
		/// Create a standard Cube Edge.
		/// </summary>
		/// <param name="initialPosition">Initial position vector of the block in the cube.</param>
		/// <param name="face1">First Visible face of the block.</param>
		/// <param name="face2">Second Visible face of the block.</param>
		public EdgeBlock(Cartesian3dCoordinate initialPosition, BlockFace face1, BlockFace face2)
			: base($"E_{face1.Id}{face2.Id}", initialPosition, new BlockFace[] { face1, face2 })
		{
			base.InitialPosition = initialPosition;
		}
	}
}
