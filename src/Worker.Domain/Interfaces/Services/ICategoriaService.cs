using System.Collections.Generic;
using System.Threading.Tasks;
using Worker.Domain.Entities;
using Worker.Domain.Interfaces.Services.Base;
using Worker.Domain.Model;

namespace Worker.Domain.Interfaces.Services;

public interface ICategoriaService : IServiceBase<Categoria>
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria> GetByIdAsync(int id);
    Task<object> AddAsync(Categoria categoria);
    Task<bool> UpdateAsync(Categoria categoria);
    Task<bool> DeleteAsync(int id);
    Task<Categoria> GetCategoriaByName(string nomeCategoria);
    Task<IEnumerable<CategoriaModel>> BuscarCategoriaSubCategoriaAsync();
}