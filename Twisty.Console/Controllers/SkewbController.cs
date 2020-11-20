using System;
using System.Linq;
using Twisty.Engine.Geometry;
using Twisty.Engine.Operations.Skewb;
using Twisty.Engine.Presenters.Skewb;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Skewb;

namespace Twisty.Bash.Controllers
{
	public class SkewbController : CubeConsoleController<SkewbCube>
	{
		public SkewbController()
			: base(new SkewbCube(), new SkewbOperationsParser())
		{
		}

		[ConsoleRoute("faces")]
		private void GetFaces()
		{
			foreach (CoreFace f in Core.Faces)
			{
				System.Console.WriteLine($"{f.Id} {FormatCoordinates(f.Coordinates)}");
			}
		}

		[ConsoleRoute("face-content")]
		private void GetFace(string faceId)
		{
			CoreFace f = Core.GetFace(faceId);
			CartesianCoordinatesConverter c = new CartesianCoordinatesConverter(f.Coordinates);

			var blocks = Core.GetBlocksForFace(f.Id)
				.OfType<IPositionnedByCartesian3dVector>()
				.ToList();
			blocks.Sort(new PlanePositionPointComparer(f.Coordinates));

			foreach (Block b in blocks.OfType<Block>())
			{
				Cartesian2dCoordinate c2 = c.ConvertTo2d(b.Position);
				System.Console.WriteLine($"{b.Id} {FormatCoordinates(b.Position)} {FormatCoordinates(c2)}");
			}
		}

		[ConsoleRoute("block")]
		private void GetBlock(string blockId)
		{
			Block b = Core.Blocks.FirstOrDefault(block => block.Id == blockId);

			Console.WriteLine($"{b.Id} {FormatCoordinates(b.Position)}");

			foreach (BlockFace face in b.Faces)
			{
				SphericalVector ccf = CoordinateConverter.ConvertToSpherical(face.Position);
				System.Console.WriteLine($"{face.Id} {FormatCoordinates(face.Position)} {FormatCoordinates(ccf)}");
			}
		}

		/// <summary>
		/// Render the cube to the console.
		/// </summary>
		protected override void Render()
		{
			var presenter = new SkewbTextPresenter(Core);

			SkewbFaceTextView[,] grid = new SkewbFaceTextView[4, 3];

			grid[0, 0] = null;
			grid[1, 0] = presenter.GetFaceAsText(CubicRotationCore.ID_FACE_UP);
			grid[2, 0] = grid[0, 0];
			grid[3, 0] = grid[0, 0];

			grid[0, 1] = presenter.GetFaceAsText(CubicRotationCore.ID_FACE_LEFT);
			grid[1, 1] = presenter.GetFaceAsText(CubicRotationCore.ID_FACE_FRONT);
			grid[2, 1] = presenter.GetFaceAsText(CubicRotationCore.ID_FACE_RIGHT);
			grid[3, 1] = presenter.GetFaceAsText(CubicRotationCore.ID_FACE_BACK);

			grid[0, 2] = grid[0, 0];
			grid[1, 2] = presenter.GetFaceAsText(CubicRotationCore.ID_FACE_DOWN);
			grid[2, 2] = grid[0, 0];
			grid[3, 2] = grid[0, 0];

			for (int gridLine = 0; gridLine < 3; ++gridLine)
			{
				for (int lineIndex = 0; lineIndex < 5; ++lineIndex)
				{
					for (int gridRow = 0; gridRow < 4; ++gridRow)
					{
						var cubeFace = grid[gridRow, gridLine];
						if (cubeFace == null)
						{
							Console.Write("      ");
							continue;
						}

						switch (lineIndex)
						{
							case 0:
								this.WriteContent(cubeFace.TopLeftColor, 2);
								this.WriteContent(cubeFace.CenterColor, 1);
								this.WriteContent(cubeFace.TopRightColor, 2);
								break;
							case 1:
								this.WriteContent(cubeFace.TopLeftColor, 1);
								this.WriteContent(cubeFace.CenterColor, 3);
								this.WriteContent(cubeFace.TopRightColor, 1);
								break;
							case 2:
								this.WriteContent(cubeFace.CenterColor, 5);
								break;
							case 3:
								this.WriteContent(cubeFace.BottomLeftColor, 1);
								this.WriteContent(cubeFace.CenterColor, 3);
								this.WriteContent(cubeFace.BottomRightColor, 1);
								break;
							case 4:
								this.WriteContent(cubeFace.BottomLeftColor, 2);
								this.WriteContent(cubeFace.CenterColor, 1);
								this.WriteContent(cubeFace.BottomRightColor, 2);
								break;
						}

						Console.Write(" ");
					}

					Console.WriteLine("");
				}
				
				Console.ForegroundColor = ConsoleColor.White;
				switch (gridLine)
				{
					case 0:
						Console.WriteLine("                       U");
						break;
					case 1:
						Console.WriteLine("     L     *     R     B");
						break;
				}
			}
		}

		private void WriteContent(string color, int count)
		{
			SetConsolColor(color);
			Console.Write(string.Empty.PadLeft(count, '*'));
		}
	}
}
