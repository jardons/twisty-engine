using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Materialization.Colors
{
	/// <summary>
	/// Interface providing basic block coloring options.
	/// </summary>
	public interface IFaceColorProvider
	{
		public Color GetColor(in RotationCore core, in Block block, in BlockFace face);
	}
}
