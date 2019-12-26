using System;

namespace Twisty.Engine.Operations
{
	/// <summary>
	/// Exception raised when an operation command cannot be succesfuly parsed.
	/// </summary>
	public class OperationParsingException : Exception
	{
		/// <summary>
		/// Create a new OperationParsingException.
		/// </summary>
		/// <param name="command">Command that was parsed while the exception occured.</param>
		/// <param name="index">Index of the char causing the Exception.</param>
		public OperationParsingException(string command, int index)
			: base("Command cannot be parsed.")
		{
			this.Command = command;
			this.Index = index;
		}

		/// <summary>
		/// Gets the command that was parsed while the exception occured.
		/// </summary>
		public string Command { get; }

		/// <summary>
		/// Gets the index of the char causing the Exception.
		/// </summary>
		public int Index { get; }
	}
}
