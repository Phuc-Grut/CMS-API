using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Exceptions;

public class FopException : Exception
{
    public FopException() { }

    public FopException(string message) : base(message) { }

    public FopException(string message, Exception inner) : base(message, inner) { }
}
