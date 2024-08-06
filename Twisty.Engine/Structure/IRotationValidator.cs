﻿using System.Collections.Generic;

namespace Twisty.Engine.Structure;

public interface IRotationValidator
{
	/// <summary>
	/// Checks if a rotation is possible.
	/// </summary>
	/// <param name="axis"></param>
	/// <param name="blocks"></param>
	/// <returns></returns>
	bool CanRotateAround(RotationAxis axis, double theta, IEnumerable<Block> blocks);
}
