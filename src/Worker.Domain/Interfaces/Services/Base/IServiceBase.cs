using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace Worker.Domain.Interfaces.Services.Base;

public interface IServiceBase<T> where T : class
{
    /// <summary>
    /// Save data (T) in repository
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<object> SalvarAsync(T entity);

    /// <summary>
    /// Get all (T) data
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> BuscarTodosAsync();

    /// <summary>
    /// Get by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> BuscarAsync(int id);

    /// <summary>
    /// Get by long ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> BuscarAsync(long id);

    /// <summary>
    /// Busca customizada de dado genérico (T)
    /// </summary>
    /// <param name="where">Clausure</param>
    /// <returns></returns>
    Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> where);

    Task<T> BuscarFirstAsync(Expression<Func<T, bool>> where);

    /// <summary>
    /// Update data (T) in repository
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> AtualizarAsync(T entity);

    Task<bool> DeletarAsync(T entity);

    Task<long> CountAsync();

    Task<long> GenericTableCountAsync(string conditions, object parameters);

    // Novos métodos
    Task<int> InsertAsync(T entity);

    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetAsync(int id);

    Task<bool> UpdateAsync(T entity);

    Task<bool> DeleteAsync(T entity);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> where);
}