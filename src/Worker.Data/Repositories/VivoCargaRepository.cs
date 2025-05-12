using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Worker.Data.Contexts;
using Worker.Data.Repositories.Base;
using Worker.Domain.Entities;
using Worker.Domain.Interfaces.Repositories;

namespace Worker.Data.Repositories;

public class VivoCargaRepository : BaseRepository<VivoCarga>, IVivoCargaRepository
{
    private readonly DapperContext _context;
    public VivoCargaRepository(DapperContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VivoCarga>> BuscarPorDataAsync(DateTime data)
    {
        using var conn = new SqlConnection("");
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Data", data, System.Data.DbType.DateTime);
        parameters.Add("@Data", data, System.Data.DbType.Int32);
        parameters.Add("@Data", data, System.Data.DbType.AnsiString, size: 50);

        await conn.OpenAsync();
        var proc = "proc_test";
        var result = await conn.QueryAsync<VivoCarga>(proc, parameters, commandTimeout: 0, commandType: System.Data.CommandType.StoredProcedure);
        return result;
    }
}