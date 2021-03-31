using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class defining a layer interval along a rotation axis.
	/// </summary>
	public class LayerInterval
	{
		public LayerInterval(int layerIndex)
		{
			this.AboveLayerIndex = layerIndex;
			this.BelowLayerIndex = layerIndex;
		}

		public LayerInterval(int aboveLayerIndex, int belowLayerIndex)
		{
			this.AboveLayerIndex = aboveLayerIndex;
			this.BelowLayerIndex = belowLayerIndex;
		}

		public int BelowLayerIndex { get; }

		public int AboveLayerIndex { get; }
	}
}
