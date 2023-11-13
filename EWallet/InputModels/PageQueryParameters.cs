using System.ComponentModel.DataAnnotations;

namespace EWallet.InputModels
{
    public class PageQueryParameters
    {
        [Required]
        public int PageNumber { get; set; }

        [Required]
        public int PageSize { get; set; }
    }
}
