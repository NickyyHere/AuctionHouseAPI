using AuctionHouseAPI.Domain.Interfaces;
using Npgsql;
using System.Data;

namespace AuctionHouseAPI.Domain.Dapper.Repositories
{
    public abstract class DapperBaseRepository<T> : ITransactionRepository, IBaseRepository<T>
    {
        protected readonly DapperContext _context;
        protected IDbConnection? _connection {  get; set; }
        protected IDbTransaction? _currentTransaction { get; private set; }
        public DapperBaseRepository(DapperContext dapperContext) 
        {
            _context = dapperContext;
        }
        protected async Task OpenConnection()
        {
            if (_connection == null)
                _connection = _context.CreateConnection();

            if (_connection.State != ConnectionState.Open)
            {
                if (_connection is NpgsqlConnection pgSqlConnection)
                    await pgSqlConnection.OpenAsync();
                else
                    _connection.Open();
            }
        }

        protected async Task CloseConnection()
        {
            if (_currentTransaction != null)
                return;

            if (_connection != null)
            {
                if (_connection is NpgsqlConnection pgSqlConnection)
                    await pgSqlConnection.CloseAsync();
                else
                    _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        public async Task BeginTransactionAsync()
        {
            _connection = _context.CreateConnection();
            if (_connection is Npgsql.NpgsqlConnection pgSqlConnection)
                await pgSqlConnection.OpenAsync();
            else
                _connection.Open();

            _currentTransaction = _connection.BeginTransaction();
        }
        public Task CommitTransactionAsync()
        {
            _currentTransaction?.Commit();
            return Dispose();
        }
        public Task RollbackTransactionAsync()
        {
            _currentTransaction?.Rollback();
            return Dispose();
        }
        public abstract Task<int> CreateAsync(T entity);

        public abstract Task DeleteAsync(T entity);

        public abstract Task<IEnumerable<T>> GetAllAsync();

        public abstract Task<T?> GetByIdAsync(int id);

        private Task Dispose()
        {
            _currentTransaction?.Dispose();
            _connection?.Dispose();
            _currentTransaction = null;
            _connection = null;
            return Task.CompletedTask;
        }
    }
}
