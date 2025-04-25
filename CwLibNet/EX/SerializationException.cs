namespace CwLibNet.EX;

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
}