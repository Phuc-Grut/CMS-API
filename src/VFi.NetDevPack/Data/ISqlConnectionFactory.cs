using System.Data;

namespace VFi.NetDevPack.Data;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}
