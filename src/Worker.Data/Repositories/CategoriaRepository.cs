using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.Data.Contexts;
using Worker.Data.Repositories.Base;
using Worker.Domain.Entities;
using Worker.Domain.Interfaces.Repositories;
using Worker.Domain.Model;

namespace Worker.Data.Repositories;

public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
{
    private readonly DapperContext _context;

    public CategoriaRepository(DapperContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await BuscarTodosAsync();
    }

    public async Task<Categoria> GetByIdAsync(int id)
    {
        return await BuscarAsync(id);
    }

    public async Task<object> AddAsync(Categoria categoria)
    {
        return await InsertAsync(categoria);
    }

    public async Task<bool> UpdateAsync(Categoria categoria)
    {
        return await AtualizarAsync(categoria);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Categoria categoria = await GetByIdAsync(id);
        if (categoria == null)
        {
            return false;
        }

        return await DeleteAsync(categoria);
    }

    public async Task<Categoria> GetCategoriaByName(string nomeCategoria)
    {
        await using SqlConnection conn = new(_context.GetConnection().ConnectionString);
        return await conn.QueryFirstOrDefaultAsync<Categoria>("SELECT * FROM ClickBankCategories WHERE CategoryName = @Nome", new { Nome = nomeCategoria });
    }

    public async Task<IEnumerable<CategoriaModel>> BuscarCategoriaSubCategoriaAsync()
    {
        await using SqlConnection conn = new(_context.GetConnection().ConnectionString);
        StringBuilder query = new();
        query.Append(@"
                        SELECT
                          c.CategoryName
                         ,sc.Title
                         ,sc.HopLink
                         ,sc.SubCategoryName
                         ,sc.LinkPresell
                        ,sc.SubCategoryID
                        FROM ClickBankCategories c WITH (NOLOCK)
                        INNER JOIN SubCategories sc WITH (NOLOCK)
                          ON c.CategoryID = sc.CategoryID
                        WHERE sc.LinkPresell IS NULL AND sc.HopLink IS NOT NULL AND sc.HopLink <> ''
                        GROUP BY c.CategoryName
                                ,sc.Title
                                ,sc.HopLink
                                ,sc.SubCategoryName
                                ,sc.LinkPresell
                                ,sc.SubCategoryID");

        return await conn.QueryAsync<CategoriaModel>(query.ToString());
    }

}