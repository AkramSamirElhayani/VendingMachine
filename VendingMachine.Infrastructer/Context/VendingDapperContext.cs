using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Infrastructer.Class;
using VendingMachine.Infrastructer.Interfaces;
using Dapper;

namespace VendingMachine.Infrastructer.Context
{
    internal class VendingDapperContext:IVendingDapperContext
    {
        private IDbConnection _connection;

        public VendingDapperContext(VendingConnectionString connectionString)
        {
            _connection = new SqlConnection(connectionString);

        }
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CancellationToken cs = default) => _connection.QueryAsync<T>(sql, param);
        public Task<T> ExecuteScalarAsync<T>(string sql, object? param = null, CancellationToken cs = default) => _connection.ExecuteScalarAsync<T>(sql, param);

   
 
    }
}
