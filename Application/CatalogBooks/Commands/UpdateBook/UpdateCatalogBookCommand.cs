using Application.CatalogBooks.Commands.Dto;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CatalogBooks.Commands.UpdateBook
{
    public record UpdateCatalogBookCommand(Guid Id, string? ISBN, string? Title, IEnumerable<AuthorReqDto>? Authors) : IRequest;

    public class UpdateCatalogBookCommandHandler : IRequestHandler<UpdateCatalogBookCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCatalogBookCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCatalogBookCommand request, CancellationToken cancellationToken)
        {
            var catalogBook = await _context.Catalog
                .Include(c => c.Book)
                .FirstOrDefaultAsync(x => x.Book.Id == request.Id, cancellationToken);

            if (catalogBook == null)
            {
                throw new NotFoundException(nameof(Book), request.Id);
            }

            catalogBook.Book.Title = request.Title;
            catalogBook.Book.ISBN = request.ISBN;

            if (request.Authors != null)
            {
                foreach (var author in request.Authors)
                {
                    var newAuthor =
                        _context.Authors.FirstOrDefault(a =>
                            a.FirstName == author.FirstName && a.LastName == author.LastName)
                        ??
                        new Author(author.FirstName, author.LastName);

                    catalogBook.Book.Authors.Add(newAuthor);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
