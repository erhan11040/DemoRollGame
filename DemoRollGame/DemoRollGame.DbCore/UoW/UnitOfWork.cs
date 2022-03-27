using DemoRollGame.DbCore.Repositories.Base;
using DemoRollGame.DbCore.Repositories.Interfaces;
using DemoRollGame.Models.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoRollGame.DbCore.UoW
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private demo_roll_gameContext _dbContext;

        private Dictionary<Type, object> repos;

        public UnitOfWork(demo_roll_gameContext dBConnectionContext)
        {
            _dbContext = dBConnectionContext;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
        {
            if (repos == null)
            {
                repos = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repos.ContainsKey(type))
            {
                repos[type] = new GenericRepository<TEntity>(_dbContext);
            }

            return (IGenericRepository<TEntity>)repos[type];
        }
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            return _dbContext.SaveChanges();
        }
        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(obj: this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }
        public IDbContextTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            throw new NotImplementedException();
        }
    }
}
