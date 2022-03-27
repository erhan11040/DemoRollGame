using DemoRollGame.DbCore.Repositories.Interfaces;
using DemoRollGame.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DemoRollGame.DbCore.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T>
       where T : class
    {
        private readonly demo_roll_gameContext context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(demo_roll_gameContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public virtual EntityState Add(T entity)
        {

            return dbSet.Add(entity).State;
        }

        public T Get<TKey>(TKey id)
        {
            return dbSet.Find(id);
        }

        public async Task<T> GetAsync<TKey>(TKey id)
        {
            return await dbSet.FindAsync(id);
        }

        public T Get(params object[] keyValues)
        {
            return dbSet.Find(keyValues);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public virtual EntityState SoftDelete(T entity)
        {
            entity.GetType().GetProperty("IsActive")?.SetValue(entity, false);
            return dbSet.Update(entity).State;
        }

        public virtual EntityState HardDelete(T entity)
        {
            return this.dbSet.Remove(entity).State;
        }
        public virtual EntityState Update(T entity)
        {
            return dbSet.Update(entity).State;
        }
    }
}
