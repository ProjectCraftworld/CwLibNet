namespace CwLibNet.EX
{
    /// <summary>
    /// Exception thrown when an error occurs during (de)serialization.
    /// </summary>
    public class SerializationException : Exception
    {
        /// <summary>
        /// Used for adding custom messages when throwing exceptions.
        /// </summary>
        /// <param name="message">Message to display with exception</param>
        public SerializationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Used for adding custom messages when throwing exceptions.
        /// </summary>
        /// <param name="message">Message to display with exception</param>
        /// <summary>
        /// Used for wrapping exception in catch block.
        /// </summary>
        /// <param name="cause">The cause of the exception</param>
        public SerializationException(Throwable cause) : base(cause)
        {
        }

        /// <summary>
        /// Used for adding custom messages when throwing exceptions.
        /// </summary>
        /// <param name="message">Message to display with exception</param>
        /// <summary>
        /// Used for wrapping exception in catch block.
        /// </summary>
        /// <param name="cause">The cause of the exception</param>
        /// <summary>
        /// Used when wrapping exception in catch block and adding custom messages.
        /// </summary>
        /// <param name="message">Message to display with exception</param>
        /// <param name="cause">The cause of the exception</param>
        public SerializationException(string message, Throwable cause) : base(message, cause)
        {
        }
    }
}