using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Dto
{
    public class UserDto: IMapFrom<User>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Name { get; set; }

        public List<BookDto> LikedBooks { get; set; } = new();
    }
}
