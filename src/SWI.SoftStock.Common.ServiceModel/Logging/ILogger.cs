using System.Collections.Generic;

namespace SWI.SoftStock.Common.Logging
{
	/// <summary>
	/// Interface for logger
	/// </summary>
	public interface ILogger
	{
		//	Properties

		/// <summary>
		/// Gets or sets logger name
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets list of registered adapters
		/// </summary>
		IEnumerable<ILogAdapter> Adapters { get; }

		/// <summary>
		/// Gets or sets minimium log message level
		/// </summary>
		LogLevel MinLevel { get; set; }

		/// <summary>
		/// Gets or sets maximium log message level
		/// </summary>
		LogLevel MaxLevel { get; set; }

		//	Methods

		/// <summary>
		/// Registers <see cref="adapterToAdd"/>
		/// </summary>
		/// <param name="adapterToAdd"></param>
		void AddAdapter(ILogAdapter adapterToAdd);

		/// <summary>
		/// Deregisters <see cref="adapterToRemove"/>
		/// </summary>
		/// <param name="adapterToRemove"></param>
		void RemoveAdapter(ILogAdapter adapterToRemove);

		/// <summary>
		/// Logs message. Means - command all registered adapters to log message
		/// </summary>
		/// <param name="logMessage"></param>
		void Log(LogMessage logMessage);
	}
}
