using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SWI.SoftStock.Common.Logging
{
	/// <summary>
	/// Provides <see cref="LogMessage"/> to string formatting
	/// </summary>
	public class LogMessageFormatString
	{
		#region default

		private static LogMessageFormatString _default;

		/// <summary>
		/// Gets default <see cref="LogMessageFormatString"/>
		/// </summary>
		public static LogMessageFormatString Default
		{
			get { return _default ?? (_default = new LogMessageFormatString("{%level%}({%time%}): {%message%} (Exception: {%ExceptionString%}){%newline%}")); }
		}
		#endregion

		#region defs

		private static readonly IDictionary<string, Func<LogMessage, object>> NamedValues = new Dictionary<string, Func<LogMessage, object>>();
		private const string NewLinePatternString = "newline";

		private readonly string _formatString;
		private readonly IList<Func<LogMessage, object>> _formatParameters = new List<Func<LogMessage, object>>();
		#endregion

		#region ctors

		static LogMessageFormatString()
		{
			foreach (var publicPropInfo in typeof(LogMessage).GetProperties())
			{
				var getMethod = publicPropInfo.GetGetMethod();
				if (getMethod == null || getMethod.IsPublic == false)
					continue;

				NamedValues.Add(publicPropInfo.Name.ToLower(), message => getMethod.Invoke(message, null));
			}
		}

		/// <summary>
		/// Initializes new instance of <see cref="LogMessageFormatString"/> based on given <see cref="formatString"/>
		/// </summary>
		/// <param name="formatString"></param>
		public LogMessageFormatString(string formatString)
		{
			_formatString = formatString;

			var regex = new Regex("\\%[_0-9a-zA-Z]*\\%");
			var matches = regex.Matches(_formatString);

			for (var i = 0; i < matches.Count; i++)
			{
				var matchValue = matches[i].Value;
				_formatString = _formatString.Replace(matchValue, i.ToString());
				var propName = matchValue.Trim('%').ToLower();
				if (String.Compare(propName, NewLinePatternString, StringComparison.InvariantCultureIgnoreCase) == 0)
					_formatParameters.Add(message => Environment.NewLine);
				else if (NamedValues.ContainsKey(propName) == false)
					_formatParameters.Add(message => "unknown property " + propName);
				else
					_formatParameters.Add(NamedValues[propName]);
			}
		}
		#endregion

		/// <summary>
		/// Formats <see cref="logMessage"/> to string
		/// </summary>
		/// <param name="logMessage"></param>
		/// <returns></returns>
		public string FormatLogMessage(LogMessage logMessage)
		{
			if (logMessage == null)
			{
				System.Diagnostics.Debug.WriteLine(
					"logMessage is null",
					String.Format("{0}.{1}", GetType().Name, "FormatLogMessage")
					);
				return null;
			}

			return String.Format(_formatString, _formatParameters.Select(func => func(logMessage)).ToArray());
		}
	}
}