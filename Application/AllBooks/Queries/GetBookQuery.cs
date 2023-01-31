using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AllBooks.Queries
{
    public record GetBookQuery(Guid Id) : IRequest<BookDto>;

    public class GetCatalogBookQueryHandler : IRequestHandler<GetBookQuery, BookDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCatalogBookQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var book = await _context.Books
                .Include(b => b.Authors)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (book == null)
                throw new NotFoundException(nameof(Book), request.Id);

            return _mapper.Map<BookDto>(book);
        }
    }
}
