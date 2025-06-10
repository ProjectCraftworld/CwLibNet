using static CwLibNet4Hub.IO.Serializer.Serializer;
using CwLibNet4Hub.IO.Serializer;
namespace CwLibNet4Hub.EX;

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