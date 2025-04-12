using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using Worker.Domain.Interfaces.Base;
using Worker.Domain.Interfaces.Services;
using Worker.Domain.Interfaces.Services.Base;

namespace Worker.Service.Services.Autenticacao;

public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
{
    private readonly IBaseRepository<TEntity> _repositoryBase;

    //private readonly ILogger<TEntity> _logger;
    public ServiceBase(IBaseRepository<TEntity> repositoryBase)
    {
        _repositoryBase = repositoryBase;
    }

    public async Task<object> SalvarAsync(TEntity entity)
    {
        return await _repositoryBase.SalvarAsync(entity);
    }

    public async Task<IEnumerable<TEntity>> BuscarTodosAsync()
    {
        return await _repositoryBase.BuscarTodosAsync();
    }

    public async Task<TEntity> BuscarAsync(int id)
    {
        return await _repositoryBase.BuscarAsync(id);
    }

    public async Task<TEntity> BuscarAsync(long id)
    {
        return await _repositoryBase.BuscarAsync(id);
    }

    public async Task<IEnumerable<TEntity>> BuscarAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _repositoryBase.BuscarAsync(where);
    }

    public async Task<TEntity> BuscarFirstAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _repositoryBase.BuscarFirstAsync(where);
    }

    public async Task<bool> AtualizarAsync(TEntity entity)
    {
        return await _repositoryBase.AtualizarAsync(entity);
    }

    public async Task<bool> DeletarAsync(TEntity entity)
    {
       return await _repositoryBase.DeletarAsync(entity);
    }

    public async Task<long> CountAsync()
    {
        return await _repositoryBase.CountAsync();
    }

    public async Task<long> GenericTableCountAsync(string conditions, object parameters)
    {
        return await _repositoryBase.GenericTableCountAsync(conditions, parameters);
    }

    public async Task<int> InsertAsync(TEntity entity)
    {
        return await _repositoryBase.InsertAsync(entity);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repositoryBase.GetAllAsync();
    }

    public async Task<TEntity> GetAsync(int id)
    {
        return await _repositoryBase.GetAsync(id);
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        return await _repositoryBase.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        return await _repositoryBase.DeleteAsync(entity);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _repositoryBase.FindAsync(where);
    }
}