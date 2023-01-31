using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Dto
{
    public class BookDto : IMapFrom<Book>
    {
        public Guid Id { get; set; }

        public string? ISBN { get; set; }

        public string? Title { get; set; }

        public string? Genre { get; set; }

        public List<AuthorDto> Authors { get; set; } = new();
    }
}
