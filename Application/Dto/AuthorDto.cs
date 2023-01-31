using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Dto
{
    public class AuthorDto: IMapFrom<Author>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
