using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Models.Securites;
using Models.Utility;
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
            //base.OnModelCreating(builder);
            builder.Entity<UserSeller>().ToTable("tbl_user_seller");

            builder.Entity<MobileVersion>().ToTable("tbl_version_mobile");
            builder.Entity<Online>().ToTable("tbl_online");
            builder.Entity<DigitLenght>().ToTable("tbl_digit_lenght");
            builder.Entity<LotteryNumber>().ToTable("tbl_lottery_number");
            builder.Entity<BillDetail>().ToTable("tbl_bill_detail");
            builder.Entity<Bill>().ToTable("tbl_bill");
            builder.Entity<DeadLineLottery>().ToTable("tbl_dead_line_lottery");
            //builder.Entity<UserSeller>().ToTable("tbl_bill_cancel");
            builder.Entity<Device>().ToTable("tbl_device");
            builder.Entity<ClearTime>().ToTable("tbl_clear_time");
        }

        #region DbSet

        public DbSet<User> User { get; set; }
        public DbSet<UserSeller> UserSeller { get; set; }

        public DbSet<MobileVersion> MobileVersion { get; set; }
        public DbSet<Online> Online { get; set; }
        public DbSet<DigitLenght> DigitLenght { get; set; }
        public DbSet<LotteryNumber> LotteryNumber { get; set; }
        public DbSet<BillDetail> BillDetail { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<DeadLineLottery> DeadLineLottery { get; set; }
        public DbSet<Device> UserSeDeviceller { get; set; }
        public DbSet<ClearTime> ClearTime { get; set; }

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
