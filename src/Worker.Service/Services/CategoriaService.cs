using System.Collections.Generic;
using System.Threading.Tasks;
using Worker.Domain.Entities;
using Worker.Domain.Interfaces.Repositories;
using Worker.Domain.Interfaces.Services;
using Worker.Domain.Model;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private ICategoriaService _categoriaServiceImplementation;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await _categoriaRepository.GetAllAsync();
    }

    public async Task<Categoria> GetByIdAsync(int id)
    {
        return await _categoriaRepository.GetByIdAsync(id);
    }

    public async Task<object> AddAsync(Categoria categoria)
    {
        return await _categoriaRepository.AddAsync(categoria);
    }

    public async Task<bool> UpdateAsync(Categoria categoria)
    {
        return await _categoriaRepository.UpdateAsync(categoria);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _categoriaRepository.DeleteAsync(id);
    }

    public async Task<Categoria> GetCategoriaByName(string nomeCategoria)
    {
        return await _categoriaRepository.GetCategoriaByName(nomeCategoria);
    }

    public async Task<IEnumerable<CategoriaModel>> BuscarCategoriaSubCategoriaAsync()
    {
        return await _categoriaRepository.BuscarCategoriaSubCategoriaAsync();
    }
    
}