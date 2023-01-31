using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.PersonalizedBooks.Queries
{
    public record GetPersonalizedBooksQuery(Guid Id) : IRequest<List<BookDto>>;
    
    public class GetPersonalizedBooksQueryHandler : IRequestHandler<GetPersonalizedBooksQuery, List<BookDto>>
    {
        private const int RecommendationAmount = 5;

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPersonalizedBooksQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BookDto>> Handle(GetPersonalizedBooksQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.LikedBooks)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken: cancellationToken);

            if (user == null) {  
                throw new NotFoundException(nameof(User), request.Id);
            }

            Dictionary<string, int> genresCount = new Dictionary<string, int>();

            foreach (var b in user.LikedBooks.Where(b => b.Genre != null))
            {
                if(!genresCount.ContainsKey(b.Genre))
                {
                    genresCount.Add(b.Genre, 1);
                }
                else
                {
                    genresCount[b.Genre]++;
                }
            }

            if(genresCount.Count == 0)
            {
                return await _context.Books
                    .Include(b => b.Authors)
                    .Take(RecommendationAmount)
                    .AsNoTracking()
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            }

            string lovedGenre = genresCount.FirstOrDefault(g => g.Value == genresCount.Values.Max()).Key;

            List<Book> recList = await _context.Books
                .Where(b => b.Genre == lovedGenre)
                .Take(RecommendationAmount)
                .ToListAsync(cancellationToken);

            if(recList.Count < RecommendationAmount)
            {
                recList.AddRange(_context.Books
                    .Where(b => !recList.Contains(b))
                    .Take(RecommendationAmount - recList.Count));
            }

            return _mapper.Map<List<Book>, List<BookDto>>(recList);
        }
    }
}
