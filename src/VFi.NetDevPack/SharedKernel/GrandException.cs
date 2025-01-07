using System;

namespace VFi.NetDevPack.SharedKernel;

/// <summary>
/// Represents errors that occur during application execution
/// </summary>
[Serializable]
public class NetDevPackException : Exception
{
    /// <summary>
    /// Initializes a new instance of the Exception class.
    /// </summary>
    public NetDevPackException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the Exception class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NetDevPackException(string message)
        : base(message)
    {
    }

}
