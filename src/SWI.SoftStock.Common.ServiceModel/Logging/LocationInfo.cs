using System;
using System.Runtime.Serialization;

namespace SWI.SoftStock.Common.Logging
{
	/// <summary>
	/// The internal representation of caller location information.
	/// </summary>
	[DataContract]
	public class LocationInfo
	{
		#region defs

		private const string FullInfoStringFormat = "{0}.{1}({2}:{3})";
		#endregion

		#region props

		/// <summary>
		/// Gets or sets the fully qualified class name of the caller making the logging request. 
		/// </summary>
		/// <value>
		/// The class name.
		/// </value>
		[DataMember]
		public string ClassName { get; private set; }

		/// <summary>
		/// Gets or sets the method name of the caller.
		/// </summary>
		/// <value>
		/// The method name.
		/// </value>
		[DataMember]
		public string MethodName { get; private set; }

		/// <summary>
		/// Gets or sets the file name of the caller.
		/// </summary>
		/// <value>
		/// The file name.
		/// </value>
		[DataMember]
		public string FileName { get; private set; }

		/// <summary>
		/// Gets or sets the line number of the caller.
		/// </summary>
		/// <value>
		/// The line number.
		/// </value>
		[DataMember]
		public int LineNumber { get; private set; }

		/// <summary>
		/// Gets or sets all available caller information.
		/// </summary>
		/// <value>
		/// The full info.
		/// </value>
		[DataMember]
		public string FullInfo
		{
			get { return string.Format(FullInfoStringFormat, ClassName, MethodName, FileName, LineNumber); }
			private set
			{
				string className, methodName, fileName;
				int lineNumber;
				if (ParseFullInfo(value, out className, out methodName, out fileName, out lineNumber) == false)
					return;

				ClassName = className;
				MethodName = methodName;
				FileName = fileName;
				LineNumber = lineNumber;
			}
		}
		#endregion

		#region ctors

		/// <summary>
		/// Initializes new instance of <see cref="LocationInfo"/> with specified parameters
		/// </summary>
		/// <param name="className"></param>
		/// <param name="methodName"></param>
		/// <param name="fileName"></param>
		/// <param name="lineNumber"></param>
		public LocationInfo(string className, string methodName, string fileName, int lineNumber)
		{
			ClassName = className;
			FileName = fileName;
			LineNumber = lineNumber;
			MethodName = methodName;
		}

		/// <summary>
		/// Parses specified string into <see cref="LocationInfo"/>
		/// </summary>
		/// <param name="fullInfo"></param>
		/// <returns></returns>
		public static LocationInfo FromFullInfo(string fullInfo)
		{
			string className, methodName, fileName;
			int lineNumber;
			return ParseFullInfo(fullInfo, out className, out methodName, out fileName, out lineNumber)
					? new LocationInfo(className, methodName, fileName, lineNumber)
					: null;
		}

		/// <summary>
		/// Parses specified input string (<see cref="fullInfo"/>) into <see cref="LocationInfo"/> fields
		/// </summary>
		/// <param name="fullInfo"></param>
		/// <param name="className"></param>
		/// <param name="methodName"></param>
		/// <param name="fileName"></param>
		/// <param name="lineNumber"></param>
		/// <returns></returns>
		public static bool ParseFullInfo(string fullInfo, out string className, out string methodName, out string fileName, out int lineNumber)
		{
			className = methodName = fileName = null;
			lineNumber = -1;
			if (string.IsNullOrEmpty(fullInfo))
				return false;

			var parts1 = fullInfo.Split(new[] { "(" }, StringSplitOptions.RemoveEmptyEntries);
			if (parts1.Length != 2)
				return false;

			var leftPart = parts1[0].Trim('(');
			var rightPart = parts1[1].Trim('(', ')');
			if (leftPart.Contains(".") == false || rightPart.Contains(":") == false)
				return false;

			var lastDotIndex = leftPart.LastIndexOf(".");
			className = leftPart.Substring(0, lastDotIndex);
			methodName = leftPart.Substring(lastDotIndex + 1);

			var lastSemicolonIndex = rightPart.LastIndexOf(":");
			fileName = rightPart.Substring(0, lastSemicolonIndex);

			return Int32.TryParse(rightPart.Substring(lastSemicolonIndex + 1), out lineNumber);
		}
		#endregion

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return string.Format("[{0}: {1}]", GetType().Name, FullInfo);
		}
	}
}