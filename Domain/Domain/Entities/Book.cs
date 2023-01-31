using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? ISBN { get; set; }

        public string? Title { get; set; }

        public string? Genre { get; set; }

        public List<Author> Authors { get; set; } = new();
    }
}