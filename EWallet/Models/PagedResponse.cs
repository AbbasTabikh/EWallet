using EWallet.Entities;

namespace EWallet.Models
{
    public class PagedResponse<T> where T : class 
    {
        public IEnumerable<T>? Data { get; set; } 
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
}
