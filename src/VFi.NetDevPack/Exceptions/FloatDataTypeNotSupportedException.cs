using VFi.NetDevPack.Exceptions;

namespace VFi.NetDevPack.Exceptions;

public class FloatDataTypeNotSupportedException : FopException
{
    public FloatDataTypeNotSupportedException(string message) : base(message) { }
}
