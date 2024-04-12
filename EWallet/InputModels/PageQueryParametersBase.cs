using System.ComponentModel.DataAnnotations;

namespace EWallet.InputModels
{
    public record PageQueryParametersBase
    {
        [Required]
        public int PageNumber { get; set; }

        [Required]
        public int PageSize { get; set; }
    }
}
