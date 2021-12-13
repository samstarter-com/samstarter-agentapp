namespace SWI.SoftStock.Common.Logging
{
	/// <summary>
	/// Performs writing log messages to different destinations
	/// </summary>
	public interface ILogAdapter
	{
		//	Properties
		/// <summary>
		/// Gets or sets Name
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets minimum level of messages to log
		/// </summary>
		LogLevel MinLevel { get; set; }

		/// <summary>
		/// Gets or sets maximum level of messages to log
		/// </summary>
		LogLevel MaxLevel { get; set; }

		/// <summary>
		/// Gets or sets <see cref="LogMessageFormatString"/>
		/// </summary>
		LogMessageFormatString MessageFormatString { get; set; }

		//	Methods
		/// <summary>
		/// Writes <see cref=" logMessage"/> to destination
		/// </summary>
		/// <param name="logMessage"></param>
		void Log(LogMessage logMessage);
	}
}