using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.Infra.CMS.Context;

namespace VFi.Infra.CMS.Repository;

public class MasterRepository : IMasterRepository
{
    private readonly MasterApiContext _apiContext;
    private readonly string PATH_GET_PROXY = "/api/proxy/get-listview";
    public MasterRepository(MasterApiContext apiContext)
    {
        _apiContext = apiContext;
    }
    public Task<IEnumerable<Proxy>> GetList(string? group)
    {
        var t = _apiContext.Client.Request(PATH_GET_PROXY)
                             .SetQueryParam("group", group)
                             .SetQueryParam("status", 1)
                             .GetJsonAsync<IEnumerable<Proxy>>().Result;
        return Task.FromResult(t);
    }
}
