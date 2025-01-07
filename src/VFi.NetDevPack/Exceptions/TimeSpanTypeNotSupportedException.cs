using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Exceptions;

public class TimeSpanTypeNotSupportedException : FopException
{
    public TimeSpanTypeNotSupportedException(string message) : base(message) { }
}
