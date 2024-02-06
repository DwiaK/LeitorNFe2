using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace LeitorNFe.Application.Abstractions.Data;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
