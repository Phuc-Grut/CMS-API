using VFi.NetDevPack.Exceptions;

namespace VFi.NetDevPack.Exceptions;

public class EnumDataTypeNotSupportedException : FopException
{
    public EnumDataTypeNotSupportedException(string message) : base(message) { }
}
