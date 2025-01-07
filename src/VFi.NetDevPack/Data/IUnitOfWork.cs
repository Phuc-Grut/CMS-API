using System.Threading.Tasks;

namespace VFi.NetDevPack.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}
