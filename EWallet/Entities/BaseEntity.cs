using System.ComponentModel.DataAnnotations;

namespace EWallet.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
