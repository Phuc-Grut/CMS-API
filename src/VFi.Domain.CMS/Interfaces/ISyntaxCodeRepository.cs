using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.CMS.Interfaces;

public interface ISyntaxCodeRepository
{
    Task<string> GetCode(string syntax, int use);
    Task<int> UseCode(string syntax, string code);
}
