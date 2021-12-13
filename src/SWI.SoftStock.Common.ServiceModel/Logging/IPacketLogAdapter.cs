namespace SWI.SoftStock.Common.Logging
{
    /// <summary>
    /// Additional information needed for package realization
    /// </summary>
    public interface IPacketLogAdapter
    {
        /// <summary>
        /// Gets or sets maximum size of log package
        /// </summary>
        /// <value>
        /// The pack size.
        /// </value>
        int PackSize { get; set; }

        /// <summary>
        /// Gets or sets the critical level of log message 
        /// When log message with level >= critical level comes to log adapter
        /// the whole package should be written to the database
        /// </summary>
        /// <value>
        /// The critical level.
        /// </value>
        LogLevel CriticalLevel { get; set; }

        /// <summary>
        /// Gets or sets the send period
        /// </summary>
        /// <value>
        /// The send period.
        /// </value>
        int SendPeriod { get; set; }
    }
}
