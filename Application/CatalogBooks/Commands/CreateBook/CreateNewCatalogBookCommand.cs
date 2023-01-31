using Application.CatalogBooks.Commands.Dto;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CatalogBooks.Commands.CreateBook;

public record CreateNewCatalogBookCommand(string Title, string ISBN, string Genre, IEnumerable<AuthorReqDto> Authors) : IRequest<Guid>;

public class CreateNewCatalogBookCommandHandler : IRequestHandler<CreateNewCatalogBookCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateNewCatalogBookCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateNewCatalogBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            ISBN = request.ISBN,
            Title = request.Title,
            Genre = request.Genre
        };

        foreach (var author in request.Authors)
        {
            var newAuthor =
                _context.Authors.FirstOrDefault(a =>
                    a.FirstName == author.FirstName && a.LastName == author.LastName)
                ??
                new Author(author.FirstName, author.LastName);

            book.Authors.Add(newAuthor);
        }

        _context.Catalog.Add(new AcmeCatalogBook { Book = book });

        await _context.SaveChangesAsync(cancellationToken);

        return book.Id;
    }
}