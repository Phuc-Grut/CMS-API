using VFi.NetDevPack.Exceptions;

namespace VFi.NetDevPack.Exceptions;

public class FilterOperatorNotFoundException : FopException
{
    public FilterOperatorNotFoundException(string message) : base(message)
    {
    }
}
