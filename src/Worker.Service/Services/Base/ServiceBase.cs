using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Worker.Domain.Interfaces.Base;
using Worker.Domain.Interfaces.Services.Base;

namespace Worker.Service.Services.Base
{
    public class ServiceBase<T> : IServiceBase<T> where T : class
    {
        private readonly IBaseRepository<T> _repository;

        public ServiceBase(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> BuscarTodosAsync()
        {
            return await _repository.BuscarTodosAsync();
        }

        public async Task<T> BuscarAsync(int id)
        {
            return await _repository.BuscarAsync(id);
        }

        public async Task<T> BuscarAsync(long id)
        {
            return await _repository.BuscarAsync(id);
        }

        public async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> where)
        {
            return await _repository.BuscarAsync(where);
        }

        public async Task<T> BuscarFirstAsync(Expression<Func<T, bool>> where)
        {
            return await _repository.BuscarFirstAsync(where);
        }

        public async Task<object> SalvarAsync(T entity)
        {
            return await _repository.SalvarAsync(entity);
        }

        public async Task<bool> AtualizarAsync(T entity)
        {
            return await _repository.AtualizarAsync(entity);
        }

        public async Task<bool> DeletarAsync(T entity)
        {
            return await _repository.DeletarAsync(entity);
        }

        public async Task<long> CountAsync()
        {
            return await _repository.CountAsync();
        }

        public async Task<long> GenericTableCountAsync(string conditions, object parameters)
        {
            return await _repository.GenericTableCountAsync(conditions, parameters);
        }

        public async Task<int> InsertAsync(T entity)
        {
            return await _repository.InsertAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            return await _repository.DeleteAsync(entity);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> where)
        {
            return await _repository.FindAsync(where);
        }
    }

}
