using VFi.NetDevPack.Exceptions;

namespace VFi.NetDevPack.Exceptions;

public class DecimalDataTypeNotSupportedException : FopException
{
    public DecimalDataTypeNotSupportedException(string message) : base(message) { }
}
