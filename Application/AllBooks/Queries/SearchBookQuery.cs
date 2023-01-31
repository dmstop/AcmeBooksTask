using Application.Common.Interfaces;
using Application.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AllBooks.Queries
{
    public record SearchBookQuery(string StartsWith) : IRequest<List<BookDto>>;

    public class SearchBookQueryHandler : IRequestHandler<SearchBookQuery, List<BookDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SearchBookQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BookDto>> Handle(SearchBookQuery request, CancellationToken cancellationToken)
        {
            return await _context.Books
                .Include(b => b.Authors)
                .Where(b => b.Title != null && b.Title.ToLower().StartsWith(request.StartsWith))
                .AsNoTracking()
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
