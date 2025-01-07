using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.Infra.CMS.Repository;
using VFi.NetDevPack.Queries;

namespace VFi.Application.CMS.Queries;


public class GetCodeQuery : IQuery<string>
{


    public GetCodeQuery(string syntax, int use)
    {
        Syntax = syntax;
        Use = use;
    }

    public string Syntax { get; set; }
    public int Use { get; set; } = 0;
}
public class SyntaxQueryHandler : IQueryHandler<GetCodeQuery, string>
{
    private readonly ISyntaxCodeRepository _synctaxCodeRepository;
    public SyntaxQueryHandler(ISyntaxCodeRepository synctaxCodeRepository)
    {
        _synctaxCodeRepository = synctaxCodeRepository;

    }

    public Task<string> Handle(GetCodeQuery request, CancellationToken cancellationToken)
    {
        return _synctaxCodeRepository.GetCode(request.Syntax, request.Use);
    }
}
