using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsultorioApi.DataAccess.Extensiones
{
    /// <summary>
    /// DapperExtensions Class
    /// </summary>
    public static class DapperExtensions
    {
        /// <summary>
        /// Gets the SQL.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="spName">Name of the store procedure.</param>
        /// <returns>Sql command text</returns>
        public static string GetSql(this DynamicParameters args, string spName)
        {
            List<string> parameters = new List<string>();
            foreach (var name in args.ParameterNames)
            {
                var pValue = args.Get<dynamic>(name);
                var type = pValue.GetType();
                if (type == typeof(DateTime))
                {
                    parameters.Add($"@{name}='{pValue.ToString("yyyy-MM-dd HH:mm:ss.fff")}'");
                }
                else if (type == typeof(bool))
                {
                    parameters.Add($"@{name}={((bool)pValue ? 1 : 0)}");
                }
                else if (type == typeof(int))
                {
                    parameters.Add($"@{name}={pValue}");
                }
                else if (type == typeof(List<int>))
                {
                    parameters.Add($"@{name}={string.Join(",", (List<int>)pValue)}");
                }
                else
                {
                    parameters.Add($"@{name}='{pValue.ToString()}'");
                }
            }
            return string.Format("EXEC {0} {1}", spName, string.Join(", ", from p in parameters select p));
        }
    }
}
