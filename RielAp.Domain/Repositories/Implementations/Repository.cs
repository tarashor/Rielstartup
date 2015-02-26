using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq.Expressions;

namespace RielAp.Domain.Repositories.Implementations
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IModel
    {
        private bool _isDisposed;

        protected Repository(DbContext context)
        {
            Context = (RealtyDBContext)context;
            Context.Configuration.LazyLoadingEnabled = true;
        }

        protected RealtyDBContext Context { get; set; }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var entry = Context.Entry(entity);
            if (entry != null)
            {
                Context.Set<TEntity>().Remove(entity);
            }
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void Attach(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
            Context.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public void SetModified(TEntity entity)
        {
            Context.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public void SetAdded(TEntity entity)
        {
            Context.Entry(entity).State = System.Data.EntityState.Added;
        }

        public TEntity GetById(object id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public bool SaveChanges()
        {
            Context.SaveChanges();
            return true;
        }

        protected IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (Context != null)
                    {
                        Context.Dispose();
                        Context = null;
                    }
                }
                _isDisposed = true;
            }
        }

        ~Repository()
        {
            Dispose(false);
        }
    }
}
