using FormBuilder.Domain;
using FormBuilder.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBuilder.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public IDbConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; private set; }
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext applicatonDbContext)
        {
            _applicationDbContext = applicatonDbContext;
        }

        public async Task BeginTransactionAsync()
        {
            if (Connection == null)
            {
                Connection = _applicationDbContext.CreateConnection();
                await ((System.Data.Common.DbConnection)Connection).OpenAsync();
            }

            if (Transaction == null)
                Transaction = Connection.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            try
            {
                Transaction?.Commit();
            }
            finally
            {
                await DisposeAsync();
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                Transaction?.Rollback();
            }
            finally
            {
                await DisposeAsync();
            }
        }

        protected async Task DisposeAsync()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }
            if (Connection != null)
            {
                await ((System.Data.Common.DbConnection)Connection).CloseAsync();
                Connection.Dispose();
                Connection = null;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Transaction?.Dispose();
                Connection?.Dispose();
                _disposed = true;
            }
        }
    }
}
