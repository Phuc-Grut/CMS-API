using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.QueryModels;
using VFi.Infra.CMS.Context;
using VFi.NetDevPack.Context;

namespace VFi.Infra.CMS.Repository;

public class SyntaxCodeRepository : ISyntaxCodeRepository
{

    private readonly MasterApiContext _apiContext;
    private readonly string PATH_GET_CODE = "/api/codesyntax/get-code";
    private readonly string PATH_USE_CODE = "/api/codesyntax/usecode";
    private readonly IContextUser _context;
    public SyntaxCodeRepository(MasterApiContext apiContext, IContextUser context)
    {
        _apiContext = apiContext;
        _context = context;
    }


    public Task<string> GetCode(string syntax, int use)
    {
        _apiContext.Token = _context.GetToken();
        var t = _apiContext.Client.Request(PATH_GET_CODE)
                             .SetQueryParam("$syntaxCode", syntax)
                             .SetQueryParam("$status", use)
                             .GetJsonAsync().Result;
        return Task.FromResult(t.code);
    }

    public Task<int> UseCode(string syntax, string code)
    {
        _apiContext.Token = _context.GetToken();
        var t = _apiContext.Client.Request(PATH_USE_CODE)
                         .SetQueryParam("syntax", syntax)
                         .SetQueryParam("code", code)
                         .PostAsync().Result;
        return Task.FromResult(t.StatusCode);
    }
}
