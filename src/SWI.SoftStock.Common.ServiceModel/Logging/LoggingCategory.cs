namespace SWI.SoftStock.Common.Logging
{
    /// <summary>
    /// Logging categories
    /// </summary>
    public struct LoggingCategory
    {
        /// <summary>
        /// Gets CategoryName.
        /// </summary>
        /// <value>
        /// The category name.
        /// </value>
        public string CategoryName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingCategory"/> struct.
        /// </summary>
        /// <param name="categoryName">
        /// The category name.
        /// </param>
        public LoggingCategory(string categoryName)
            : this()
        {
            CategoryName = categoryName;
        }

        /// <summary>
        /// Gets Default logging category.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static LoggingCategory Default
        {
            get
            {
                return new LoggingCategory("*");
            }
        }
    }
}
