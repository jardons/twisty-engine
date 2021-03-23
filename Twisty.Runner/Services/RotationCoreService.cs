using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry.Rotations;
using Twisty.Engine.Materialization;
using Twisty.Engine.Operations;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;
using Twisty.Runner.Models;
using Twisty.Runner.Models.Model3d;

namespace Twisty.Runner.Services
{
	public interface IRotationCoreService
	{
		public RotationCoreObject CreateNewCore(string coreTypeId, Action onRotationChange);

		MaterializedCore Materialize(RotationCoreObject core);

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
			RotationCore core = new RubikCube(3);
			var runner = new OperationRunnerSpy(new OperationRunner(core), onRotationChange);

			return new RotationCoreObject(core, runner);
		}

		public MaterializedCore Materialize(RotationCoreObject core)
			=> m_Materializer.Materialize(core.Core);

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
