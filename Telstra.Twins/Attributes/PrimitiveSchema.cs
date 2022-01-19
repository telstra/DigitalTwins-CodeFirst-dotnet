namespace Telstra.Twins.Attributes
{
    public static class PrimitiveSchema
    {
        /// <summary>
        /// A boolean value
        /// </summary>
        public const string Boolean = "boolean";

        /// <summary>
        /// A full-date as defined in section 5.6 of RFC 3339
        /// </summary>
        public const string Date = "date";

        /// <summary>
        /// A date-time as defined in RFC 3339
        /// </summary>
        public const string DateTime = "dateTime";

        /// <summary>
        /// An IEEE 8-byte floating point
        /// </summary>
        public const string Double = "double";

        /// <summary>
        /// A duration in ISO 8601 format
        /// </summary>
        public const string Duration = "duration";

        /// <summary>
        /// An IEEE 4-byte floating point
        /// </summary>
        public const string Float = "float";

        /// <summary>
        /// A signed 4-byte integer
        /// </summary>
        public const string Integer = "integer";

        /// <summary>
        /// A signed 8-byte integer
        /// </summary>
        public const string Long = "long";

        /// <summary>
        /// A UTF8 string
        /// </summary>
        public const string String = "string";

        /// <summary>
        /// A full-time as defined in section 5.6 of RFC 3339
        /// </summary>
        public const string Time = "time";
    }
}
