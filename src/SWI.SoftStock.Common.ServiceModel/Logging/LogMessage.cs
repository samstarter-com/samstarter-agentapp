using System;
using System.Runtime.Serialization;

namespace SWI.SoftStock.Common.Logging
{
	/// <summary>
	/// Represents log message.
	/// </summary>
	[DataContract]
	[KnownType(typeof(LogLevel))]
	[KnownType(typeof(LocationInfo))]
	public sealed class LogMessage
	{
		#region Props

		/// <summary>
		/// Gets Log Level.
		/// </summary>
		/// <value>
		/// Log level.
		/// </value>
		[DataMember]
		public LogLevel Level { get; private set; }

		/// <summary>
		/// Gets Time.
		/// </summary>
		/// <value>
		/// The time stamp.
		/// </value>
		[DataMember]
		public DateTime Time { get; private set; }

		/// <summary>
		/// Gets Message.
		/// </summary>
		/// <value>
		/// The message object.
		/// </value>
		[DataMember]
		public string Message { get; private set; }

		/// <summary>
		/// Gets ExceptionString.
		/// </summary>
		/// <value>
		/// The exception text.
		/// </value>
		[DataMember]
		public string ExceptionString { get; private set; }

		/// <summary>
		/// Gets Location.
		/// </summary>
		/// <value>
		/// The location.
		/// </value>
		[DataMember]
		public LocationInfo Location { get; private set; }

		/// <summary>
		/// Gets MachineName
		/// </summary>
		/// <value>
		/// The exception text.
		/// </value>
		[DataMember]
		public string MachineName { get; private set; }

		/// <summary>
		/// Gets UserName.
		/// </summary>
		/// <value>
		/// The user name.
		/// </value>
		[DataMember]
		public string UserName { get; private set; }

	    /// <summary>
	    /// Gets or sets LevelValue.
	    /// </summary>
	    [DataMember]
	    public int LevelValue { get; set; }

	    /// <summary>
	    /// Gets Category.
	    /// </summary>
	    /// <value>
	    /// The category.
	    /// </value>
	    [DataMember]
        public string Category { get; private set; }

	    #endregion

		#region ctors

	    /// <summary>
	    /// Creates new instance of LogMessage class
	    /// </summary>
	    /// <param name="level"></param>
	    /// <param name="time"></param>
	    /// <param name="message"></param>
	    /// <param name="exceptionText"></param>
	    /// <param name="machineName"></param>
	    /// <param name="location"></param>
	    /// <param name="userName"></param>
	    /// <param name="category">Category</param>
	    /// <returns></returns>
	    public static LogMessage Create(LogLevel level, DateTime time, string message, string exceptionText = null, string machineName = null, LocationInfo location = null, string userName = null, string category = "*")
		{
			return new LogMessage
			{
				Level = level,
				Time = time,
				Message = message,
				ExceptionString = exceptionText,
				MachineName = machineName,
				Location = location,
				UserName = userName,
                LevelValue = (int)level,
                Category = category
			};
		}
		#endregion
	}
}