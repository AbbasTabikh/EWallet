using EWallet.Data;
using EWallet.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EWallet.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> Add(T entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity, cancellationToken);
            return entity;
        }

        public void BulkDelete(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAll(string additionalProperties, CancellationToken token)
        {
            if (string.IsNullOrEmpty(additionalProperties))
            {
                return await _dbSet.ToListAsync(token);
            }

            var properties = additionalProperties.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
            var query = _dbSet.AsQueryable();

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync(token);
        }

        public async Task<T?> GetByID(Guid id)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.ID == id);
        }

        public async Task<IEnumerable<T>> GetManyByExpression(Expression<Func<T, bool>> filter, string additionalProperties, CancellationToken cancellationToken)
        {
            var query = _dbSet.Where(filter);
            
            if (string.IsNullOrEmpty(additionalProperties))
            {
                return await query.ToListAsync(cancellationToken);
            }

            var properties = additionalProperties.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetSingleByExpression(Expression<Func<T, bool>> filter, string additionalProperties, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(additionalProperties))
            {
                return await _dbSet.SingleOrDefaultAsync(filter, cancellationToken);
            }

            var properties = additionalProperties.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
            var query = _dbSet.AsQueryable();

            foreach (var property in properties)
            {
                query = query.Include(property);
            }

            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public T Update(T oldentity, T newEntity)
        {
            _context.Entry(oldentity).CurrentValues.SetValues(newEntity);
            return oldentity;
        }
    }
}
