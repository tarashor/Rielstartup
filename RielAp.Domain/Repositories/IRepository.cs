using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Validation;

namespace RielAp.Domain.Repositories
{
    public interface IRepository<TModel> : IDisposable
    {
        void Delete(TModel entity);
        void Add(TModel entity);
        void Attach(TModel entity);
        void SetModified(TModel entity);
        void SetAdded(TModel entity);
        TModel GetById(object id);
        bool SaveChanges();
    }
}
