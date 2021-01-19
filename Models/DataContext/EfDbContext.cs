
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Models.DataModels;
using Models.Securites;
using System.Data;
using System.Threading.Tasks;

namespace Models.DataContext
{
    public class EfDbContext : DbContext, IEfDbContext
    {
        #region Constructor

        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {

        }
        public EfDbContext() : base()
        {
        }

        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        #region DbSet

        public DbSet<User> User { get; set; }
        public DbSet<Genre> Genre { get; set; }
        
        #endregion

        #region Transactions

        private IDbContextTransaction _transaction;

        public void BeginTran()
        {
            _transaction = Database.BeginTransaction();
        }
        public async Task BeginTranAsync()
        {
            _transaction = await Database.BeginTransactionAsync();
        }

        public void CommitTran()
        {
            try
            {
                SaveChanges();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
            }
        }
        public async Task CommitTranAsync()
        {
            try
            {
                await SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public void RollbackTran()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }
        public async Task RollbackTranAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
        #endregion

        /// <summary>
        /// This is to generate json by stored procedure.
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public async Task<string> SqlQueryToGetJson(string sqlQuery)
        {
            var table = new DataTable();
            var conn = Database.GetDbConnection();
            if (conn.State.ToString() != "Open")
            {
                await conn.OpenAsync();
            }

            var command = conn.CreateCommand();
            command.CommandText = sqlQuery;

            using (var reader = await command.ExecuteReaderAsync())
            {
                table.Load(reader);
            }
            if (conn.State.ToString() == "Open")
            {
                await conn.CloseAsync();
            }
            return table.Rows[0][0].ToString();
        }
    }
}
