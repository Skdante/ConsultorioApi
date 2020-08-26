using System;
using System.Data;
using System.Threading.Tasks;

namespace ConsultorioApi.DataAccess
{
    public interface IBaseRepository
    {
        Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData);
    }
}
