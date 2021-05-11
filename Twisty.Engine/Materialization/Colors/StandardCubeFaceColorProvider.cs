using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Materialization.Colors
{
	public class StandardCubeFaceColorProvider : IFaceColorProvider
	{
		public Color GetColor(in RotationCore core, in Block block, in BlockFace face)
		{
			return face.Id switch
			{
				"F" => Color.Blue,
				"L" => Color.Orange,
				"R" => Color.Red,
				"D" => Color.White,
				"B" => Color.Green,
				"U" => Color.Yellow,
				// Return black when not able to match a color.
				_ => Color.Black
			};
		}
	}
}
