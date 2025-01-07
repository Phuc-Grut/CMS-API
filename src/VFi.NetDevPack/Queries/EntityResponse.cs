using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Queries;

public class EntityResponse<TEntity> : BaseResponse
{
    public EntityResponse(bool status, IList<DetailError> detailErrors) : base(status, detailErrors)
    {
    }

    public EntityResponse()
    {
    }

    public TEntity Data { get; set; }

    public virtual EntityResponse<TEntity> SetData(TEntity data)
    {
        Data = data;
        Status = true;
        return this;
    }
}
