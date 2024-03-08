using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Infrastructer.Interfaces;

internal interface IVendingDapperContext
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, CancellationToken cs = default);
    Task<T> ExecuteScalarAsync<T>(string sql, object? param = null ,CancellationToken cs = default);
}
