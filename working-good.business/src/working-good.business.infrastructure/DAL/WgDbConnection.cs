using System.Data;
using Microsoft.Data.SqlClient;

namespace working_good.business.infrastructure.DAL;

public class WgDbConnection(string connectionString) : IWgDbConnection
{
    public IDbConnection DbConnection { get; } = new SqlConnection(connectionString);
}