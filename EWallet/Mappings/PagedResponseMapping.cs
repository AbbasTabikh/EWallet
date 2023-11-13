using EWallet.Dtos;
using EWallet.Entities;
using EWallet.Models;

namespace EWallet.Mappings
{
    public static class PagedResponseMapping
    {
        public static PagedResponse<BudgetDto> ToDto(this PagedResponse<Budget> pagedResponse)
        {
            return new PagedResponse<BudgetDto>()
            {
                CurrentPage = pagedResponse.CurrentPage,
                PageSize = pagedResponse.PageSize,
                TotalPages = pagedResponse.TotalPages,
                TotalRecords = pagedResponse.TotalRecords,
                Data = pagedResponse.Data?.Select(x => new BudgetDto
                {
                    CreationDate = x.CreationDate!.Value.ToString("dd/MM/yyyy"),
                    ID = x.ID,
                    Total = x.Total,
                })
            };
        }
    }
}
