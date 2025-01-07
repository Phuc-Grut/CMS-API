using VFi.NetDevPack.Exceptions;

namespace VFi.NetDevPack.Exceptions;

public class DoubleDataTypeNotSupportedException : FopException
{
    public DoubleDataTypeNotSupportedException(string message) : base(message) { }
}
