using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twisty.Engine.Geometry.Rotations;
using Twisty.Engine.Materialization;
using Twisty.Engine.Operations;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;
using Twisty.Engine.Structure.Skewb;
using Twisty.Runner.Models;
using Twisty.Runner.Models.Model3d;

namespace Twisty.Runner.Services
{
	public interface IRotationCoreService
	{
		public RotationCoreObject CreateNewCore(string coreTypeId, Action onRotationChange);

		CoreRotations CalculatePositions(RotationCoreObject core);

		void RunCommand(RotationCoreObject core, string command);
	}

	internal class RotationCoreService : IRotationCoreService
	{
		#region Private Classes

		private class OperationRunnerSpy : IOperationRunner
		{
			private readonly IOperationRunner m_Runner;
			private readonly Action m_OnAfterExecute;

			public OperationRunnerSpy(IOperationRunner runner, Action onAfterExecute)
			{
				m_Runner = runner;
				m_OnAfterExecute = onAfterExecute;
			}

			public void Execute(IEnumerable<IOperation> operations)
			{
				m_Runner.Execute(operations);
				m_OnAfterExecute();
			}

			public void Execute(IOperation operation)
			{
				m_Runner.Execute(operation);
				m_OnAfterExecute();
			}
		}

		#endregion Private Classes

		// Fixed Tools
		private readonly StandardMaterializer m_Materializer;

		// State fields
		private IOperationsParser m_CommandParser;

		public RotationCoreService()
		{
			m_Materializer = new StandardMaterializer();
			m_CommandParser = new RubikOperationsParser();
		}

		public RotationCoreObject CreateNewCore(string coreTypeId, Action onRotationChange)
		{
			RotationCore core = coreTypeId switch
			{
				"Rubik[2]" => new RubikCube(2),
				"Rubik[3]" => new RubikCube(3),
				"Skewb" => new SkewbCube(),
				_ => null,
			};

			var runner = new OperationRunnerSpy(new OperationRunner(core), onRotationChange);
			var core3d = new Core3d(coreTypeId, m_Materializer.Materialize(core).Objects.Select(o => new Core3dObject(o)));

			return new RotationCoreObject(coreTypeId, core, runner, core3d);
		}

		public CoreRotations CalculatePositions(RotationCoreObject core)
		{
			var positions = new Dictionary<string, IReadOnlyList<SimpleRotation3d>>();

			foreach (var block in core.Core.Blocks)
				positions[block.Id] = block.Orientation.GetEulerAngles();

			return new CoreRotations(positions);
		}

		public void RunCommand(RotationCoreObject core, string command)
		{
			if (string.IsNullOrWhiteSpace(command))
				return;

			var operations = m_CommandParser.Parse(command);
			core.Runner.Execute(operations);
		}
	}
}
