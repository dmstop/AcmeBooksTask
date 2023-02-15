using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class AcmeCatalogBook
    {
        [Key]
        public Guid Id { get; set; }

        public double Price { get; set; }

        public Book Book { get; set; }
    }
}
