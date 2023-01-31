using Application.Common.Interfaces;
using Application.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CatalogBooks.Queries.GetCatalogBook
{
    public record GetAllCatalogBooksQuery : IRequest<List<BookDto>>;
    
    public class GetAllCatalogBooksQueryHandler : IRequestHandler<GetAllCatalogBooksQuery, List<BookDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCatalogBooksQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BookDto>> Handle(GetAllCatalogBooksQuery request, CancellationToken cancellationToken)
        {
            return await _context.Catalog
                .Include(c => c.Book)
                .ThenInclude(b => b.Authors)
                .Select(c => c.Book)
                .AsNoTracking()
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
