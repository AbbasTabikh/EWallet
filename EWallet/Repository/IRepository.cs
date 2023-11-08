﻿using EWallet.Entities;
using System.Linq.Expressions;

namespace EWallet.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll(string additionalProperties, CancellationToken token);
        Task<T?> GetByID(Guid id);
        Task<T?> GetSingleByExpression(Expression<Func<T, bool>> filter, string additionalProperties, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetManyByExpression(Expression<Func<T, bool>> filter, string additionalProperties, CancellationToken cancellationToken);
        Task<T> Add(T entity, CancellationToken cancellationToken);
        Task Save(CancellationToken cancellationToken);
        T Update(T entity, T newEntity);
        void Delete(T entity);
        void BulkDelete(IEnumerable<T> entities);
    }
}
