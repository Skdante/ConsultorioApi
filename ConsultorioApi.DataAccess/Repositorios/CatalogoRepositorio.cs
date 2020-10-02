using ConsultorioApi.Entities;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultorioApi.DataAccess
{
    public class CatalogoRepositorio : BaseRepository, ICatalogoRepositorio
    {
        public CatalogoRepositorio(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<List<EstadoDB>> GetEstado(int paisId)
        {
            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@paisId", paisId, DbType.Int32, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "spEstados"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<EstadoDB>().ToList();
            });
        }

        public async Task<List<Municipio>> GetMunicipio(int estadoId)
        {
            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@estadoId", estadoId, DbType.Int32, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "spMunicipios"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<Municipio>().ToList();
            });
        }

        public async Task<List<Especialidad>> GetEspecialidad()
        {
            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();

                var records = await query.QueryMultipleAsync(
                    sql: "spEspecialidades"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<Especialidad>().ToList();
            });
        }
    }
}
