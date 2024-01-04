using System.Data;

namespace working_good.business.infrastructure.DAL;

public interface IWgDbConnection
{
    IDbConnection DbConnection { get; }
}