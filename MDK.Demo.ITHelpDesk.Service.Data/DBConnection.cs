using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MDK.Demo.ITHelpDesk.Service.Data
{
    public class DBConnection(IConfiguration configuration) : IDBConnection
    {
        private readonly IConfiguration _configuration = configuration;

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
