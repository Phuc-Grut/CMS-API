using VFi.NetDevPack.Exceptions;

namespace VFi.NetDevPack.Exceptions;

public class IntDataTypeNotSupportedException : FopException
{
    public IntDataTypeNotSupportedException(string message) : base(message) { }
}
