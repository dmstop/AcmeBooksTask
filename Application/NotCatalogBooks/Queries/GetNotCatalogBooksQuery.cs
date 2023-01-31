using Application.Common.Interfaces;
using Application.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.NotCatalogBooks.Queries
{
    public record GetNotCatalogBooksQuery : IRequest<List<BookDto>>;
    
    public class GetNotCatalogBooksQueryHandler : IRequestHandler<GetNotCatalogBooksQuery, List<BookDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetNotCatalogBooksQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BookDto>> Handle(GetNotCatalogBooksQuery request, CancellationToken cancellationToken)
        {
            return await _context.Books
                .Include(b => b.Authors)
                .Where(b => !_context.Catalog.Any(c => c.Book == b))
                .AsNoTracking()
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
