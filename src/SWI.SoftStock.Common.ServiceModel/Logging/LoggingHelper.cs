using System;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SWI.SoftStock.Common.Logging
{
    using System.Diagnostics;

    /// <summary>
	/// <see cref="ILogger"/> extensions
	/// </summary>
	public static class LoggingHelper
    {
        ///// <summary>
        ///// Log a debug message.
        ///// </summary>
        //[Obsolete("Используйте метод принимающий параметр LoggingCategory")]
        //public static void Debug(this ILogger logger, string messageFormat, params object[] parameters)
        //{
        //    logger.Log(LogLevel.Debug, LoggingCategory.Default, messageFormat, null, 2, parameters);
        //}

        /// <summary>
        /// Log a debug message.
        /// </summary>
        public static void Debug(this ILogger logger, LoggingCategory category, string messageFormat, params object[] parameters)
        {
            logger.Log(LogLevel.Debug, category, messageFormat, null, 2, parameters);
        }

		/// <summary>
		/// Log an informational message.
		/// </summary>
        [Obsolete("Используйте метод принимающий параметр LoggingCategory")]
        public static void Info(this ILogger logger, string messageFormat, params object[] parameters)
		{
            logger.Log(LogLevel.Info, LoggingCategory.Default, messageFormat, null, 2, parameters);
		}

        /// <summary>
        /// Log an informational message.
        /// </summary>
        public static void Info(this ILogger logger, LoggingCategory category, string messageFormat, params object[] parameters)
        {
            logger.Log(LogLevel.Info, category, messageFormat, null, 2, parameters);
        }

		/// <summary>
		/// Log a warning message.
		/// </summary>
        [Obsolete("Используйте метод принимающий параметр LoggingCategory")]
        public static void Warn(this ILogger logger, string messageFormat, params object[] parameters)
		{
            logger.Log(LogLevel.Warn, LoggingCategory.Default, messageFormat, null, 2, parameters);
		}

        /// <summary>
        /// Log a warning message.
        /// </summary>
        public static void Warn(this ILogger logger, LoggingCategory category, string messageFormat, params object[] parameters)
        {
            logger.Log(LogLevel.Warn, category, messageFormat, null, 2, parameters);
        }

		/// <summary>
		/// Log a warning message.
		/// </summary>
        public static void Warn(this ILogger logger, Exception exc, LoggingCategory category, string messageFormat, params object[] parameters)
		{
            logger.Log(LogLevel.Warn, category, messageFormat, exc, 2, parameters);
		}

        ///// <summary>
        ///// Log a warning message.
        ///// </summary>
        //[Obsolete("Используйте метод принимающий параметр LoggingCategory")]
        //public static void Warn(this ILogger logger, Exception exc, string messageFormat, params object[] parameters)
        //{
        //    logger.Log(LogLevel.Warn, LoggingCategory.Default, messageFormat, exc, 2, parameters);
        //}

        ///// <summary>
        ///// Log an error message.
        ///// </summary>
        //[Obsolete("Используйте метод принимающий параметр LoggingCategory")]
        //public static void Error(this ILogger logger, string messageFormat, params object[] parameters)
        //{
        //    logger.Log(LogLevel.Error, LoggingCategory.Default, messageFormat, null, 2, parameters);
        //}

        /// <summary>
        /// Log an error message.
        /// </summary>
        public static void Error(this ILogger logger, LoggingCategory category, string messageFormat, params object[] parameters)
        {
            logger.Log(LogLevel.Error, category, messageFormat, null, 2, parameters);
        }

        ///// <summary>
        ///// Log an error message.
        ///// </summary>
        //[Obsolete("Используйте метод принимающий параметр LoggingCategory")]
        //public static void Error(this ILogger logger, Exception exc, string messageFormat, params object[] parameters)
        //{
        //    logger.Log(LogLevel.Error, LoggingCategory.Default, messageFormat, exc, 2, parameters);
        //}

        /// <summary>
        /// Log an error message.
        /// </summary>
        public static void Error(this ILogger logger, Exception exc, LoggingCategory category, string messageFormat, params object[] parameters)
        {
            logger.Log(LogLevel.Error, category, messageFormat, exc, 2, parameters);
        }

        ///// <summary>
        ///// Log a fatal message.
        ///// </summary>
        //[Obsolete("Используйте метод принимающий параметр LoggingCategory")]
        //public static void Fatal(this ILogger logger, string messageFormat, params object[] parameters)
        //{
        //    logger.Log(LogLevel.Fatal, LoggingCategory.Default, messageFormat, null, 2, parameters);
        //}

        /// <summary>
        /// Log a fatal message.
        /// </summary>
        public static void Fatal(this ILogger logger, LoggingCategory category, string messageFormat, params object[] parameters)
        {
            logger.Log(LogLevel.Fatal, category, messageFormat, null, 2, parameters);
        }

        ///// <summary>
        ///// Log a fatal message.
        ///// </summary>
        //[Obsolete("Используйте метод принимающий параметр LoggingCategory")]
        //public static void Fatal(this ILogger logger, Exception exc, string messageFormat, params object[] parameters)
        //{
        //    logger.Log(LogLevel.Fatal, LoggingCategory.Default, messageFormat, exc, 2, parameters);
        //}

        /// <summary>
        /// Log a fatal message.
        /// </summary>
        public static void Fatal(this ILogger logger, Exception exc, LoggingCategory category, string messageFormat, params object[] parameters)
        {
            logger.Log(LogLevel.Fatal, category, messageFormat, exc, 2, parameters);
        }

		/// <summary>
		/// Checks, if specified <see cref="level"/> is between <see cref="minLevel"/> and <see cref="maxLevel"/>
		/// </summary>
		/// <param name="level"></param>
		/// <param name="minLevel"></param>
		/// <param name="maxLevel"></param>
		/// <returns></returns>
		public static bool IsLevelInside(LogLevel level, LogLevel minLevel, LogLevel maxLevel)
		{
			var levelInt = (int)level;
			var minInt = (int)minLevel;
			var maxInt = (int)maxLevel;

			return levelInt >= minInt && levelInt <= maxInt;
		}

        /// <summary>
        /// Logs message, created by specified parameters
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="level"></param>
        /// <param name="category"></param>
        /// <param name="messageFormat"></param>
        /// <param name="exception"></param>
        /// <param name="parameters"></param>
        public static void Log(this ILogger logger, LogLevel level, LoggingCategory category, string messageFormat, Exception exception, params object[] parameters)
		{
			Log(logger, level, category, messageFormat, exception, 1, parameters);
		}

	    private static void Log(
	        this ILogger logger,
	        LogLevel level,
            LoggingCategory category,
	        string messageFormat,
	        Exception exception,
	        int skipFrames,
	        params object[] parameters)
		{
			if (logger == null)
				return;

			var trace = new StackTrace(skipFrames + 1, true);
			var fileName = trace.GetFrame(0).GetFileName();
			var method = trace.GetFrame(0).GetMethod();
			var className = method != null && method.DeclaringType != null ? method.DeclaringType.Name : "<anonymous method>";
			var methodName = method != null ? method.Name : "null method";
			var lineNumber = trace.GetFrame(0).GetFileLineNumber();

			var location = new LocationInfo(className, methodName, fileName, lineNumber);

	        var messageToLog = LogMessage.Create(
	            level,
	            DateTime.Now,
	            CreateMessageString(messageFormat, parameters),
	            exception != null ? string.Format("{0}", exception) : null,
	            GetMachineName(),
	            location,
	            GetUserName(),
	            category.CategoryName);
			logger.Log(messageToLog);
		}

		/// <summary>
		/// Creates formatted message string
		/// </summary>
		/// <param name="messageStringFormat"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static string CreateMessageString(string messageStringFormat, params object[] parameters)
		{
			try
			{
				return string.Format(messageStringFormat, parameters);
			}
			catch (FormatException)
			{
				return string.Format("{0}({1})", messageStringFormat, parameters.ListToString());
			}
		}

		private static string GetMachineName()
		{
			try
			{
				if (OperationContext.Current != null)
				{
					//	we're in wcf service
					var messageProperty = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as
										  RemoteEndpointMessageProperty;
					if (messageProperty != null)
						return Dns.GetHostEntry(messageProperty.Address).HostName;
				}
				else
				{
					//	we're in some local app
					return Environment.MachineName;
				}
			}
			catch (Exception exc)
			{
				System.Diagnostics.Debug.WriteLine(
					string.Format("Could not get Machine name: {0}", exc),
					"LoggingHelper"
					);
			}

			return "Unknown";
		}

		private static string GetUserName()
		{
			try
			{
				if (OperationContext.Current != null)
				{
					//	we're in wcf service
					if (ServiceSecurityContext.Current != null)
						return ServiceSecurityContext.Current.WindowsIdentity.Name;
				}
				else
				{
					var currentIdentity = WindowsIdentity.GetCurrent();
					if (currentIdentity != null)
						return currentIdentity.Name;
				}
			}
			catch (Exception exc)
			{
				System.Diagnostics.Debug.WriteLine(
					string.Format("Could not get Caller name: {0}", exc),
					"LoggingHelper"
					);
			}

			return "Unknown";
		}
	}
}