using EWallet.Entities;
using EWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Services
{
    public abstract class BaseService
    {
        protected async Task<PagedResponse<T>> GeneratedPaginatedResponse<T>(IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken) where T : BaseEntity
        {
            var pagedResponse = new PagedResponse<T>()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = await query.CountAsync(cancellationToken)
            };

            pagedResponse.TotalPages = (int)Math.Ceiling((double)pagedResponse.TotalRecords / pageSize);
            var toSkip = (pageNumber - 1) * pageSize;
            
            pagedResponse.Data = await query.OrderByDescending(x => x.CreationDate)
                                            .Skip(toSkip)
                                            .Take(pagedResponse.PageSize)
                                            .ToListAsync(cancellationToken);
            return pagedResponse;
        }
    }
}
