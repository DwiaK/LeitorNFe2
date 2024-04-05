using Microsoft.Data.SqlClient;

namespace LeitorNFe.Application.Abstractions.Data;

public interface IDbConnection
{
    SqlConnection CreateConnection();
}
