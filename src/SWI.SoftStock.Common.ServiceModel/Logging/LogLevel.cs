using System;
using System.Runtime.Serialization;

namespace SWI.SoftStock.Common.Logging
{
	/// <summary>
	/// Defines the severity level of log message
	/// </summary>
	[DataContract]
	[Serializable]
	public enum LogLevel
	{
		[EnumMember]
		Off=0,

		[EnumMember]
		Debug=1,

		[EnumMember]
		Info=2,

		[EnumMember]
		Warn=3,

		[EnumMember]
		Error=4,

		[EnumMember]
		Fatal=5
	}
}