using Dapper;
using Dommel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Worker.Data.Contexts;
using Worker.Domain.Interfaces.Base;

namespace Worker.Data.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DapperContext _databaseContext;

        public BaseRepository(DapperContext context)
        {
            _databaseContext = context;
        }

        public async Task<IEnumerable<T>> BuscarTodosAsync()
        {
            using IDbConnection con = _databaseContext.GetConnection();
            return await con.GetAllAsync<T>();
        }

        public async Task<T> BuscarAsync(int id)
        {
            using IDbConnection con = _databaseContext.GetConnection();
            return await con.GetAsync<T>(id);
        }

        public async Task<T> BuscarAsync(long id)
        {
            using IDbConnection con = _databaseContext.GetConnection();
            return await con.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> where)
        {
            _ = where ?? throw new ArgumentNullException(nameof(where));

            using IDbConnection con = _databaseContext.GetConnection();
            return await con.SelectAsync(where);
        }

        public async Task<T> BuscarFirstAsync(Expression<Func<T, bool>> where)
        {
            _ = where ?? throw new ArgumentNullException(nameof(where));

            using IDbConnection con = _databaseContext.GetConnection();
            IEnumerable<T> retorno = await con.SelectAsync(where);
            return retorno.FirstOrDefault();
        }

        public async Task<object> SalvarAsync(T entity)
        {
            using IDbConnection con = _databaseContext.GetConnection();

            // Abrir a conexão, caso ainda não esteja aberta
            if (con.State != ConnectionState.Open)
                con.Open();

            // Iniciar uma transação
            using var transaction = con.BeginTransaction();

            try
            {
                // Executar a inserção dentro da transação
                object retorno = await con.InsertAsync(entity, transaction);

                // Confirmar a transação
                transaction.Commit();

                return retorno;
            }
            catch
            {
                // Reverter a transação em caso de erro
                transaction.Rollback();
                throw;
            }
            finally
            {
                // Garantir que a conexão será fechada
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
        }


        public async Task<bool> AtualizarAsync(T entity)
        {
            using IDbConnection con = _databaseContext.GetConnection();
            // Abrir a conexão, caso ainda não esteja aberta
            if (con.State != ConnectionState.Open)
                con.Open();

            // Iniciar uma transação
            using var transaction = con.BeginTransaction();

            try
            {
                // Executar o update dentro da transação
                bool result = await con.UpdateAsync(entity, transaction);

                // Confirmar a transação
                transaction.Commit();

                return result;
            }
            catch
            {
                // Reverter a transação em caso de erro
                transaction.Rollback();
                throw;
            }
            finally
            {
                // Garantir que a conexão será fechada
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
        }


        public async Task<bool> DeletarAsync(T entity)
        {
            using IDbConnection con = _databaseContext.GetConnection();

            // Abrir a conexão, caso ainda não esteja aberta
            if (con.State != ConnectionState.Open)
                con.Open();

            // Iniciar uma transação
            using var transaction = con.BeginTransaction();

            try
            {
                // Executar a exclusão dentro da transação
                bool resultado = await con.DeleteAsync(entity, transaction);

                // Confirmar a transação
                transaction.Commit();

                return resultado;
            }
            catch
            {
                // Reverter a transação em caso de erro
                transaction.Rollback();
                throw;
            }
            finally
            {
                // Garantir que a conexão será fechada
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
        }


        public async Task<long> CountAsync()
        {
            using IDbConnection con = _databaseContext.GetConnection();
            return await con.CountAsync<T>();
        }

        public async Task<long> GenericTableCountAsync(string conditions, object parameters)
        {
            _ = parameters ?? throw new ArgumentNullException(nameof(parameters));

            if (string.IsNullOrEmpty(conditions))
            {
                throw new ArgumentNullException(nameof(conditions));
            }

            using IDbConnection con = _databaseContext.GetConnection();

            Dictionary<string, ISqlBuilder> SqlBuilders = new();
            ISqlBuilder sqlBuilder = new SqlServerSqlBuilder();

            string name = con.GetType().Name;
            if (SqlBuilders.TryGetValue(name.ToLower(), out ISqlBuilder value))
            {
                sqlBuilder = value;
            }

            return await con.QuerySingleAsync<long>($"SELECT COUNT(*) FROM {Resolvers.Table(typeof(T), sqlBuilder)} (NOLOCK) WHERE {conditions};", parameters);
        }

        public async Task<int> InsertAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "A entidade não pode ser nula.");

            using IDbConnection con = _databaseContext.GetConnection();

            // Abrir a conexão, caso ainda não esteja aberta
            if (con.State != ConnectionState.Open)
                con.Open();

            // Iniciar uma transação
            using var transaction = con.BeginTransaction();

            try
            {
                // Executar a inserção dentro da transação
                int insertedId = Convert.ToInt32(await con.InsertAsync(entity, transaction));

                // Confirmar a transação
                transaction.Commit();

                return insertedId;
            }
            catch (Exception ex)
            {
                // Reverter a transação em caso de erro
                transaction.Rollback();
                throw new Exception("Erro ao inserir a entidade.", ex);
            }
            finally
            {
                // Garantir que a conexão será fechada
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IEnumerable<T> allAsync;
            using IDbConnection con = this._databaseContext.GetConnection();
            allAsync = await con.GetAllAsync<T>();
            return allAsync;
        }

        public async Task<T> GetAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(string.Format("ID inválido {0}", id));
            T async;
            using IDbConnection con = this._databaseContext.GetConnection();
            async = await con.GetAsync<T>(id);
            return async;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "A entidade não pode ser nula.");

            using IDbConnection con = _databaseContext.GetConnection();

            // Abrir a conexão, caso ainda não esteja aberta
            if (con.State != ConnectionState.Open)
                con.Open();

            // Iniciar uma transação
            using var transaction = con.BeginTransaction();

            try
            {
                // Executar a atualização dentro da transação
                bool result = await con.UpdateAsync(entity, transaction);

                // Confirmar a transação
                transaction.Commit();

                return result;
            }
            catch (Exception ex)
            {
                // Reverter a transação em caso de erro
                transaction.Rollback();
                throw new Exception("Erro ao atualizar a entidade.", ex);
            }
            finally
            {
                // Garantir que a conexão será fechada
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
        }


        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "A entidade não pode ser nula.");

            using IDbConnection con = _databaseContext.GetConnection();

            // Abrir a conexão, caso ainda não esteja aberta
            if (con.State != ConnectionState.Open)
                con.Open();

            // Iniciar uma transação
            using var transaction = con.BeginTransaction();

            try
            {
                // Executar a exclusão dentro da transação
                bool result = await con.DeleteAsync(entity, transaction);

                // Confirmar a transação
                transaction.Commit();

                return result;
            }
            catch (Exception ex)
            {
                // Reverter a transação em caso de erro
                transaction.Rollback();
                throw new Exception("Erro ao excluir a entidade.", ex);
            }
            finally
            {
                // Garantir que a conexão será fechada
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> where)
        {
            if (where == null)
                throw new ArgumentNullException(nameof(where));
            IEnumerable<T> async;
            using IDbConnection con = this._databaseContext.GetConnection();
            async = await con.SelectAsync<T>(where);
            return async;
        }
    }
}