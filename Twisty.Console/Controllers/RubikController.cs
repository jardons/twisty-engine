using System;
using System.Linq;
using Twisty.Engine.Geometry;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Presenters.Rubiks;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;

namespace Twisty.Bash.Controllers
{
	public class RubikController : CubeConsoleController<RubikCube>
	{
		public RubikController(int n = 3)
			: base(new RubikCube(n), new RubikOperationsParser())
		{
		}

		[ConsoleRoute("faces")]
		private void GetFaces()
		{
			foreach (CoreFace f in Core.Faces)
			{
				System.Console.WriteLine($"{f.Id} {FormatCoordinates(f.Plane)}");
			}
		}

		[ConsoleRoute("face-content")]
		private void GetFace(string faceId)
		{
			CoreFace f = Core.GetFace(faceId);
			CartesianCoordinatesFlattener c = new CartesianCoordinatesFlattener(f.Plane);

			var blocks = Core.GetBlocksForFace(f.Id)
				.OfType<IPositionnedByCartesian3dVector>()
				.ToList();
			blocks.Sort(new PlanePositionPointComparer(f.Plane));

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
			var presenter = new RubikCubeTextPresenter(Core);

			RubikFaceTextView[,] grid = new RubikFaceTextView[4, 3];

			grid[0, 0] = presenter.GetFacePlaceHolder();
			grid[1, 0] = presenter.GetFaceAsText(RubikCube.ID_FACE_UP);
			grid[2, 0] = grid[0, 0];
			grid[3, 0] = grid[0, 0];

			grid[0, 1] = presenter.GetFaceAsText(RubikCube.ID_FACE_LEFT);
			grid[1, 1] = presenter.GetFaceAsText(RubikCube.ID_FACE_FRONT);
			grid[2, 1] = presenter.GetFaceAsText(RubikCube.ID_FACE_RIGHT);
			grid[3, 1] = presenter.GetFaceAsText(RubikCube.ID_FACE_BACK);

			grid[0, 2] = grid[0, 0];
			grid[1, 2] = presenter.GetFaceAsText(RubikCube.ID_FACE_DOWN);
			grid[2, 2] = grid[0, 0];
			grid[3, 2] = grid[0, 0];

			for (int gridLine = 0; gridLine < 3; ++gridLine)
			{
				for (int lineIndex = 0; lineIndex < presenter.LinesPerBlockFace * Core.N; ++lineIndex)
				{
					for (int gridRow = 0; gridRow < 4; ++gridRow)
					{
						var cubeFace = grid[gridRow, gridLine];
						for (int y = 0; y < Core.N; ++y)
						{
							var blockFace = cubeFace[lineIndex / presenter.LinesPerBlockFace, y];

							SetConsolColor(blockFace.Color);
							Console.Write(blockFace.GetLine(lineIndex % presenter.LinesPerBlockFace));
						}
					}

					Console.WriteLine("");
				}
			}
		}
	}
}
