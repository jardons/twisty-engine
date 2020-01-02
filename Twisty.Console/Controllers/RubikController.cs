using System;
using System.Linq;
using Twisty.Engine.Geometry;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Presenters.Rubiks;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;

namespace Twisty.Bash.Controllers
{
	public class RubikController : BaseConsoleControler<RubikCube>
	{
		public RubikController()
			: base(new RubikCube(3), new RubikOperationsParser())
		{
		}

		[ConsoleRoute("faces")]
		private void GetFaces()
		{
			foreach (RotationAxis a in Core.Axes)
			{
				Cartesian3dCoordinate cc = CoordinateConverter.ConvertToCartesian(a.Vector);
				System.Console.WriteLine($"{a.Id} ({a.Vector.Phi}, {a.Vector.Theta}) ({cc.X}, {cc.Y}, {cc.Z})");
			}
		}

		[ConsoleRoute("face-content")]
		private void GetFace(string faceId)
		{
			RotationAxis a = Core.GetAxis(faceId);
			Plane p = new Plane(
				CoordinateConverter.ConvertToCartesian(a.Vector),
				CoordinateConverter.ConvertToCartesian(Core.GetBlocksForFace(a.Vector).FirstOrDefault().Position));
			CartesianCoordinatesConverter c = new CartesianCoordinatesConverter(p);

			var blocks = Core.GetBlocksForFace(a.Vector)
				.OfType<IPositionnedBySphericalVector>()
				.ToList();
			blocks.Sort(new PlanePositionPointComparer(p));

			foreach (Block b in blocks.OfType<Block>())
			{
				Cartesian3dCoordinate cc = CoordinateConverter.ConvertToCartesian(b.Position);
				Cartesian2dCoordinate c2 = c.ConvertTo2d(cc);
				System.Console.WriteLine($"{b.Id} ({b.Position.Phi}, {b.Position.Theta}) ({cc.X}, {cc.Y}, {cc.Z}) ({c2.X}, {c2.Y})");
			}
		}

		[ConsoleRoute("block")]
		private void GetBlock(string blockId)
		{
			Block b = Core.Blocks.FirstOrDefault(block => block.Id == blockId);

			Cartesian3dCoordinate cc = CoordinateConverter.ConvertToCartesian(b.Position);
			Console.WriteLine($"{b.Id} ({b.Position.Phi}, {b.Position.Theta}) ({cc.X}, {cc.Y}, {cc.Z})");

			foreach (BlockFace face in b.Faces)
			{
				Cartesian3dCoordinate ccf = CoordinateConverter.ConvertToCartesian(face.Position);
				System.Console.WriteLine($"{face.Id} ({face.Position.Phi}, {face.Position.Theta}) ({ccf.X}, {ccf.Y}, {ccf.Z})");
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
			grid[1, 0] = presenter.GetFaceAsText(RubikCube.FACE_ID_UP);
			grid[2, 0] = grid[0, 0];
			grid[3, 0] = grid[0, 0];

			grid[0, 1] = presenter.GetFaceAsText(RubikCube.FACE_ID_LEFT);
			grid[1, 1] = presenter.GetFaceAsText(RubikCube.FACE_ID_FRONT);
			grid[2, 1] = presenter.GetFaceAsText(RubikCube.FACE_ID_RIGHT);
			grid[3, 1] = presenter.GetFaceAsText(RubikCube.FACE_ID_BACK);

			grid[0, 2] = grid[0, 0];
			grid[1, 2] = presenter.GetFaceAsText(RubikCube.FACE_ID_DOWN);
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

		/// <summary>
		/// Sets the Color used to print in the console.
		/// </summary>
		/// <param name="id">Id of the color that will be used to print in the console.</param>
		private void SetConsolColor(string id)
		{
			switch (id)
			{
				case RubikCube.FACE_ID_UP:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case RubikCube.FACE_ID_LEFT:
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					break;
				case RubikCube.FACE_ID_RIGHT:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case RubikCube.FACE_ID_FRONT:
					Console.ForegroundColor = ConsoleColor.Blue;
					break;
				case RubikCube.FACE_ID_BACK:
					Console.ForegroundColor = ConsoleColor.Green;
					break;
				case RubikCube.FACE_ID_DOWN:
				default:
					Console.ForegroundColor = ConsoleColor.White;
					break;
			}
		}
	}
}
