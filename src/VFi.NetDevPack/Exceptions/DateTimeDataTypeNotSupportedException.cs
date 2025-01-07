using VFi.NetDevPack.Exceptions;

namespace VFi.NetDevPack.Exceptions;

public class DateTimeDataTypeNotSupportedException : FopException
{
    public DateTimeDataTypeNotSupportedException(string message) : base(message) { }

}
