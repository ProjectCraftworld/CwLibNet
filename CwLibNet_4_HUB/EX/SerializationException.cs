using static CwLibNet.IO.Serializer.Serializer;
using CwLibNet.IO.Serializer;
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

    public SerializationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}