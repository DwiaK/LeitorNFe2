using LeitorNFe.Application.Abstractions.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LeitorNFe.Persistence;

public class DbConnectionFactory : IDbConnection
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SqlConnection CreateConnection() =>
        new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
}
