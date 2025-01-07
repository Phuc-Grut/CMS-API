using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Domain.CMS.Models;

namespace VFi.Domain.CMS.Interfaces;

public interface IMasterRepository
{
    Task<IEnumerable<Proxy>> GetList(string? group);
}
