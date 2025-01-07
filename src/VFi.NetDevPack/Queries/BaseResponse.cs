using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Queries;

public class DetailError

{
    public DetailError(string errorCode, IDictionary<string, string> parameters)
    {
        ErrorCode = errorCode;
        Parameters = parameters;
    }

    public string ErrorCode { get; }
    public IDictionary<string, string> Parameters { get; }
}

public class BaseResponse
{
    public BaseResponse(bool status, IList<DetailError> detailErrors)
    {
        Status = status;
        DetailErrors = detailErrors;
    }

    public BaseResponse()
    {
    }

    public bool Status { get; set; }

    public IList<DetailError> DetailErrors { get; set; }

    public void Successful()
    {
        Status = true;
    }

    public void Fail(IList<DetailError> detailErrors)
    {
        Status = false;
        DetailErrors = detailErrors;
    }

    public void Fail(string errorCode)
    {
        Status = false;
        DetailErrors = new List<DetailError>(){
            new DetailError(errorCode, null)
        };
    }
}
