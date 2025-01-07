using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Exceptions;

public class CharDataTypeNotSupportedException : FopException
{
    public CharDataTypeNotSupportedException(string message) : base(message) { }
}
